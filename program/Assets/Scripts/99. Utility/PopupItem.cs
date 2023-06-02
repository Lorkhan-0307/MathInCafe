using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupItem
{
    public IPopupHandler popup;
    public object param;
    public Action<object> closeCallback;
    public PopupItem nextPopupItem;

    public PopupItem(IPopupHandler popup, object param, Action<object> closeCallback)
    {
        this.popup = popup;
        this.param = param;
        this.closeCallback = closeCallback;
    }

    public void QueuePopup(PopupItem popupItem)
    {
        nextPopupItem = popupItem;
    }

    public void CallCloseCallback(object param)
    {
        closeCallback?.Invoke(param);
    }
}
