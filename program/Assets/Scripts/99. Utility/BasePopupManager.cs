using System.Collections.Generic;
using UnityEngine;

public abstract class BasePopupManager<T> : Singleton<PopupManager> where T : BasePopup
{
    public T currentPopup; // 현재 표시되고 있는 팝업
    public Queue<T> popupQueue = new Queue<T>(); // 팝업 큐

    public virtual void ShowPopup(T popup)
    {
        if (!popup.gameObject.activeSelf)
        {
            if (currentPopup == null)
            {
                ShowPopupInternal(popup);
            }
            else
            {
                EnqueuePopup(popup);
            }
        }
    }
    
    

    public virtual void Close(T popup)
    {
        if (popup.gameObject.activeSelf)
        {
            popup.gameObject.SetActive(false);

            if (currentPopup == popup)
            {
                currentPopup = null;
                DequeuePopup();
            }
        }
    }

    public void EnqueuePopup(T popup)
    {
        if (!popupQueue.Contains(popup))
        {
            popupQueue.Enqueue(popup);
        }
    }

    public void DequeuePopup()
    {
        if (popupQueue.Count > 0 && currentPopup == null)
        {
            T nextPopup = popupQueue.Dequeue();
            ShowPopupInternal(nextPopup);
        }
    }

    protected void ShowPopupInternal(T popup)
    {
        //popup.gameObject.SetActive(true);
        currentPopup = popup;
        popup.OnEnterPopup();
    }
    
    protected virtual void OnPopupShown(T popup)
    {
        // Popup이 화면에 보여질 때 수행할 작업을 구현하세요.
    }

    protected virtual void OnPopupHidden(T popup)
    {
        // Popup이 화면에서 사라질 때 수행할 작업을 구현하세요.
    }
}