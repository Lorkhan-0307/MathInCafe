using System;
using UnityEngine;



public abstract class PopupHandler : MonoBehaviour, IPopupHandler
{
    public abstract string GetName();

    protected virtual void Awake()
    {
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual bool IsActivePopup()
    {
        return gameObject.activeSelf;
    }

    public virtual int GetSortingOrder()
    {
        var canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            return canvas.sortingOrder;
        }
        else
        {
            return 0;
        }
    }

    public virtual void AddSortingOrder(int order)
    {
        var canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder += order;
        }
    }

    public virtual void ResetSortingOrder()
    {
        var canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 0;
        }
    }

    public virtual IPopupAnimation GetAnimation()
    {
        return null;
    }

    public virtual void OnWillEnter(object param)
    {
        // Override in derived classes if needed
    }

    public virtual void OnDidEnter(object param)
    {
        // Override in derived classes if needed
    }

    public virtual void OnWillLeave()
    {
        // Override in derived classes if needed
    }

    public virtual void OnDidLeave()
    {
        // Override in derived classes if needed
    }
}