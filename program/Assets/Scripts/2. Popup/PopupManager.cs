using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Unity;

public class PopupManager : BasePopupManager<BasePopup>
{
    private Dictionary<string, int> popupPriorityMap = new Dictionary<string, int>(); // 팝업 우선순위 맵핑

    public void ShowPopup(string popupName)
    {
        // 팝업 Prefab 로드
        GameObject popupPrefab = Resources.Load<GameObject>(popupName);

        if (popupPrefab == null)
        {
            Debug.LogError($"Popup prefab '{popupName}' not found!");
            return;
        }

        // 팝업 인스턴스 생성
        GameObject popupObject = Instantiate(popupPrefab);
        BasePopup popup = popupObject.GetComponent<BasePopup>();

        if (popup == null)
        {
            Debug.LogError($"Popup component not found in prefab '{popupName}'!");
            Destroy(popupObject);
            return;
        }

        // 팝업 표시
        ShowPopup(popup);
    }
    
    public override void ShowPopup(BasePopup popup)
    {
        base.ShowPopup(popup);
        popup.OnEnterPopup();
    }
    
    public override void Close(BasePopup popup)
    {
        base.Close(popup);
    }

    protected override void OnPopupShown(BasePopup popup)
    {
        base.OnPopupShown(popup);

        // if (popup is CustomPopup customPopup)
        // {
        //     // 팝업 애니메이션 재생
        //     customPopup.PlayShowAnimation();
        //
        //     // 다국어 지원
        //     customPopup.UpdateTextLocalization();
        // }
    }

    protected override void OnPopupHidden(BasePopup popup)
    {
        base.OnPopupHidden(popup);

        // if (popup is CustomPopup customPopup)
        // {
        //     // 팝업 애니메이션 재생
        //     customPopup.PlayHideAnimation();
        //
        //     // 숨겨진 팝업 삭제
        //     Destroy(customPopup.gameObject);
        // }
    }

    public void SetPopupPriority(BasePopup popup, int priority)
    {
        if (popupPriorityMap.ContainsKey(popup.popupName))
        {
            popupPriorityMap[popup.popupName] = priority;
        }
        else
        {
            popupPriorityMap.Add(popup.popupName, priority);
        }
    }

    public void EnqueueHighestPriorityPopup()
    {
        if (popupQueue.Count > 0 && currentPopup == null)
        {
            BasePopup highestPriorityPopup = null;
            int highestPriority = int.MinValue;

            foreach (BasePopup popup in popupQueue)
            {
                if (popup is BasePopup customPopup && popupPriorityMap.ContainsKey(popup.popupName))
                {
                    int priority = popupPriorityMap[popup.popupName];
                    if (priority > highestPriority)
                    {
                        highestPriority = priority;
                        highestPriorityPopup = customPopup;
                    }
                }
            }

            // if (highestPriorityPopup != null)
            // {
            //     popupQueue.Remove(highestPriorityPopup);
            //     ShowInternal(highestPriorityPopup);
            // }
        }
    }
}