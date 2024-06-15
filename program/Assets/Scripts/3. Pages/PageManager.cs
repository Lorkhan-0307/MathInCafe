using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;


/// <summary>
/// Page를 관리한다.
/// 페이지는 한개만 등장할 수 있다.
/// </summary>
public static class PageManager {
    public static IPageTransition Transition { get; set; }

    public static bool IsChanging { get; private set; } = false;

    // 페이지는 한개만 보여질 수 있다.
    public static IPageHandler CurrentPage { get; private set; }

	private static readonly Dictionary<string, IPageHandler> pages = new Dictionary<string, IPageHandler>();

	public static int GetPageCount() {
		return pages.Count;
	}
    
    
    /// <summary>
    /// 현재 보여지고 있는 페이지를 바로 닫는다.
    /// </summary>
	public static void Clear() {
        if (CurrentPage != null) {
            InvokeOnWillLeave();
            InvokeOnDidLeave();
            CurrentPage = null;
        }
		IsChanging = false;
	}

	// 트랜지션 없이 씬을 전환한다.
	public static void ChangeImmediate(string name, object param = null) {
		if (IsChanging) return;
		
		ChangeAsync(name, param, false).Forget();
	}

	public static void Change(string name, object param = null) {
		if (IsChanging) return;
		
        ChangeAsync(name, param, true).Forget();
	}

	private static async UniTask ChangeAsync(string name, object param, bool enableTransition) {
		Assert.IsTrue(pages.ContainsKey(name));

		IsChanging = true;
        //EscapeButtonHandler.Instance.Lock();
        //ScreenLock.Lock();

		if (CurrentPage != null) {
			InvokeOnWillLeave();

			if (enableTransition && Transition != null) {
				await Transition.TransitionOut(CurrentPage);
			}

            InvokeOnDidLeave();
            CurrentPage.Hide();
            //EscapeButtonHandler.Instance.Remove(0);
		}

		CurrentPage = pages[name];
		CurrentPage.Show();
        InvokeOnWillEnter(param);

        if (enableTransition && Transition != null) {
			await Transition.TransitionIn(CurrentPage);
		}

        //EscapeButtonHandler.Instance.Insert(0, CurrentPage);
        InvokeOnDidEnter(param);
        IsChanging = false;
        //EscapeButtonHandler.Instance.Unlock();
        //ScreenLock.Unlock();
	}

	private static void InvokeOnDidEnter(object param) {
		CurrentPage.OnDidEnter(param);
		GameSceneEvent.PublishOnDidEnter();
	}

	private static void InvokeOnWillEnter(object param) {
		CurrentPage.OnWillEnter(param);
		GameSceneEvent.PublishOnWillEnter();
	}

	private static void InvokeOnDidLeave() {
		CurrentPage.OnDidLeave();
		GameSceneEvent.PublishOnDidLeave();
	}

	private static void InvokeOnWillLeave() {
		CurrentPage.OnWillLeave();
		GameSceneEvent.PublishOnWillLeave();
	}

	public static void Add(string pageName, IPageHandler page) {
		pages.Add(pageName, page);
	}

	public static void Remove(string pageName) {
		pages.Remove(pageName);
	}
}