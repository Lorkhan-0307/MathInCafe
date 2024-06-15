using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Popup을 관리한다.
/// </summary>
public static class PopupManager {
    public static IPopupHandler CurrentPopup {
        get {
            if (popupStack.Count == 0) return null;
            return popupStack.Last().popup;
        }
    }

    // 광고 같이 모든 팝업에 적용되어야 하는 애니메이션을 설정할 수 있다.
    public static IGlobalPopupAnimation GlobalAnimation { get; set; }
    
    /// <summary>
    /// 현재 로드된 모든 팝업의 리스트를 관리한다. 
    /// </summary>
    private static readonly Dictionary<string, IPopupHandler> popups = new Dictionary<string, IPopupHandler>();
    
    /// <summary>
    /// 현재 화면이 등장하는 팝업을 관리한다. 
    /// </summary>
    private static readonly LinkedList<PopupItem> popupStack = new LinkedList<PopupItem>();

    /// <summary>
    /// 팝업을 중간에 로딩할때 어떤 정책을 사용할 지 결정한다.
    /// </summary>
    public static IPopupMemoryPolicy MemoryPolicy { get; set; } = new PopupInstantiatePolicy();
    
    public static bool IsChanging { get; private set; } = false;

    public static void ClearPopups() {
        popups.Clear();
    }
    
    public static void Clear() {
        if (CurrentPopup != null) {
            InvokeOnWillLeave(CurrentPopup);
            InvokeOnDidLeave(CurrentPopup);
        }

        // callback 호출을 대기하고 있을 수 있으므로, callback을 전부 호출해준다.
        foreach (var item in popupStack) {
            item.CallCloseCallback(null);
        }

        popupStack.Clear();
        IsChanging = false;
    }

	public static void Show(string popupName) {
        ShowAsync(popupName).Forget();
	}

	public static void Show(string popupName, object param) {
        ShowAsync(popupName, param).Forget();
	}


    public static void Show(string popupName, Action<object> closeCallback) {
        Show(popupName, null, closeCallback);
    }


    public static async void Show(string popupName, object param, Action<object> closeCallback) {
        await LoadOrCreatePopup(popupName);
        Assert.IsTrue(popups.ContainsKey(popupName));
        
        await ShowInternal(new PopupItem(popups[popupName], param, closeCallback));
    }
    
    public static async UniTask<object> ShowAsync(string popupName, object param = null) {
        await LoadOrCreatePopup(popupName);
        
        var returnObject = new UniTaskCompletionSource<object>();
        Show(popupName, param, returnParam => {
            returnObject.TrySetResult(returnParam);
        });

        return await returnObject.Task;
    }
    
    private static async UniTask ShowInternal(PopupItem popupItem) {
        if (IsChanging) {
            Debug.LogWarning("Popup IsChanging");
            return;
        }
        if (popupItem.popup.IsActivePopup()) {
            Debug.LogWarning("Target Popup is Active Already.");
            return;
        }

        await ShowInternalAsync(popupItem);
    }

    public static async void Queue(string popupName, object param = null, Action<object> closeCallback = null) {
        await LoadOrCreatePopup(popupName);
        Assert.IsTrue(popups.ContainsKey(popupName));

        if (popups.ContainsKey(popupName) == false) {
            Debug.LogError($"POPUP NOT EXIST: {popupName}");
            return;
        }
        
        await QueueInternal(new PopupItem(popups[popupName], param, closeCallback));
    }

    public static async UniTask<object> QueueAsync(string popupName, object param = null) {
        await LoadOrCreatePopup(popupName);    
        
        var returnObject = new UniTaskCompletionSource<object>();
        Queue(popupName, param, returnParam => {
            returnObject.TrySetResult(returnParam);
        });

        return await returnObject.Task;
    }
    
    private static async UniTask QueueInternal(PopupItem popupItem) {
        if (popupStack.Count == 0) {
            // 현재 떠있는 팝업이 없을때는 바로 팝업을 보여준다.
            await ShowInternal(popupItem);
        } else {
            // 현재 떠있는 팝업이 닫힌 후에 실행되도록 추가한다.
            popupStack.Last().QueuePopup(popupItem);
        }
    }
    

    private static async UniTask ShowInternalAsync(PopupItem popupItem) {
        Debug.Log("PopupManager.ShowInternalAsync START");
        IsChanging = true;
        //EscapeButtonHandler.Instance.Lock();
        //ScreenMultiLock.Lock();

        // Set Top SortingOrder to New CurrentPopup
        SetTopSortingOrder(popupItem);

        popupStack.AddLast(popupItem);
        
        CurrentPopup.Show();
        InvokeOnWillEnter(popupItem);

        if (GlobalAnimation != null && GlobalAnimation.CanAnimateIn(CurrentPopup.GetName())) {
            await GlobalAnimation.AnimationIn();
        } else if (CurrentPopup.GetAnimation() != null) {
            await CurrentPopup.GetAnimation().AnimationIn();
        } 
        
        InvokeOnDidEnter(popupItem);
        IsChanging = false;
        //EscapeButtonHandler.Instance.PushListener(CurrentPopup);
        //EscapeButtonHandler.Instance.Unlock();
        //ScreenMultiLock.Unlock();
    }

    private static void InvokeOnDidEnter(PopupItem popupItem) {
        CurrentPopup.OnDidEnter(popupItem.param);
        PopupEvent.PublishOnDidEnter();
    }

    private static void InvokeOnWillEnter(PopupItem popupItem) {
        CurrentPopup.OnWillEnter(popupItem.param);
        PopupEvent.PublishOnWillEnter();
    }

    private static void SetTopSortingOrder(PopupItem newPopup) {
        var topSortingOrder = 0;
        if (CurrentPopup != null) {
            topSortingOrder = CurrentPopup.GetSortingOrder();
        }

        newPopup.popup.AddSortingOrder(topSortingOrder + 1);
    }

    public static async void CloseByName(string popupName, object param = null, bool destroy = true) {
        if (IsChanging) return;

        if (GetPopupStackCount() == 0) {
            Assert.IsTrue(false, "PopupStack is Empty!");
            return;
        }
        
        var popupItem = popupStack.FirstOrDefault(w => w.popup.GetName() == popupName);
        Assert.IsTrue(popupItem != null);

        popupStack.Remove(popupItem);
        await CloseInternalAsync(popupItem, param);
        
        if (destroy) {
            DestroyPopup(popupName);	
        }
    }
    
    private static void DestroyPopup(string popupName) {
        if (MemoryPolicy != null && MemoryPolicy.DestroyPopup(popupName)) {
            Remove(popupName);
        }
    }

    public static async void CloseAll(object param = null, bool destroy = true) {
        if (IsChanging) return;

        while (popupStack.Count > 0) {
            var popupItem = popupStack.Last();
            var popupName = popupItem.popup.GetName();
            popupStack.RemoveLast();
            await CloseInternalAsync(popupItem, param);
            
            if (destroy) {
                DestroyPopup(popupName);
            }
        }
    }
    
    public static void CloseAllImmediate(object param = null, bool destroy = true) {
        var openedPopupCount = GetPopupStackCount();
        for (var i = 0; i < openedPopupCount; i++) {
            var popupName = popupStack.Last().popup.GetName();
            CloseImmediate(param);
            if (destroy) {
                DestroyPopup(popupName);
            }                
        }
    }        

    public static async void Close(object param = null, bool destroy = true) {
        if (IsChanging) return;
        if (popupStack.Any() == false) return;

        var popupItem = popupStack.Last();
        var popupName = popupItem.popup.GetName();
        popupStack.RemoveLast();
        await CloseInternalAsync(popupItem, param);
        
        if (destroy) {
            DestroyPopup(popupName);
        }
    }

    /// <summary>
    /// 팝업을 종료 연출 없이 바로 닫는다.
    /// </summary>
    /// <param name="param"></param>
    public static void CloseImmediate(object param = null) {
        if (IsChanging) return;
        if (popupStack.Any() == false) return;

        var popupItem = popupStack.Last();
        popupStack.RemoveLast();
        CloseBefore(popupItem);
        CloseAfter(popupItem, param);
    }

    private static async UniTask CloseInternalAsync(PopupItem popupItem, object param) {
        Debug.Log("PopupManager.CloseInternalAsync START");
        CloseBefore(popupItem);

        if (GlobalAnimation != null && GlobalAnimation.CanAnimateOut(popupItem.popup.GetName())) {
            await GlobalAnimation.AnimationOut();
        } else if (popupItem.popup.GetAnimation() != null) {
            await popupItem.popup.GetAnimation().AnimationOut();
        } 

        CloseAfter(popupItem, param);
        await CheckNextPopup(popupItem);
    }

    private static async UniTask CheckNextPopup(PopupItem popupItem) {
        if (popupItem.nextPopupItem != null) {
            if (IsChanging) {
                // 여기서 IsChanging이 바뀌었다면 closeCallback에서 팝업을 추가로 호출한 것이다.
                // 팝업이 닫힌 후에 QueuePopup을 보여준다.
                await QueueInternal(popupItem.nextPopupItem);
            } else {
                // 현재 팝업에 QueuePopup으로 추가된 팝업이 있다면 보여준다.
                await ShowInternal(popupItem.nextPopupItem);
            }
        }
    }

    private static void CloseAfter(PopupItem popupItem, object param) {
        Debug.Log("PopupManager.CloseAfter START");
        InvokeOnDidLeave(popupItem.popup);
        popupItem.popup.ResetSortingOrder();
        popupItem.popup.Hide();
        //EscapeButtonHandler.Instance.PopListener();

        // Call CloseCallback.

        IsChanging = false;
        //EscapeButtonHandler.Instance.Unlock();
        //ScreenMultiLock.Unlock();

        popupItem.closeCallback?.Invoke(param);
    }

    private static void InvokeOnDidLeave(IHandler popup) {
        popup.OnDidLeave();
        PopupEvent.PublishOnDidLeave();
    }

    private static void CloseBefore(PopupItem popupItem) {
        IsChanging = true;
        //EscapeButtonHandler.Instance.Lock();
        //ScreenMultiLock.Lock();

        InvokeOnWillLeave(popupItem.popup);
    }

    private static void InvokeOnWillLeave(IHandler popup) {
        popup.OnWillLeave();
        PopupEvent.PublishOnWillLeave();
    }

    public static void Add(string popupName, IPopupHandler popup) {
        popups.Add(popupName, popup);
    }

    public static void Remove(string popupName) {
        popups.Remove(popupName);
    }

    public static bool Contains(string popupName) {
        return popups.ContainsKey(popupName);
    }

    public static int GetPopupCount() {
        return popups.Count;
    }

    public static int GetPopupStackCount() {
        return popupStack.Count;
    }
    
    private static async UniTask LoadOrCreatePopup(string popupName) {
        //이미 해당 팝업이 로딩되어 있을 경우 생성하지 않는다.
        if (Contains(popupName)) return;
        if (MemoryPolicy == null) return;

        var popupHandler = await MemoryPolicy.LoadPopup(popupName);
        if (popupHandler == null) {
            Debug.LogError("LoadOrCreatePopup: PopupHandler == null");
            return;
        }
			
        Add(popupName, popupHandler);
    }
}
