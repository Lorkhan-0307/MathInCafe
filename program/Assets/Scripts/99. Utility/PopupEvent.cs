using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PopupEvent
{
    public static event Action OnWillEnter;
    public static event Action OnDidEnter;
    public static event Action OnWillLeave;
    public static event Action OnDidLeave;

    public static void PublishOnWillEnter()
    {
        OnWillEnter?.Invoke();
    }

    public static void PublishOnDidEnter()
    {
        OnDidEnter?.Invoke();
    }

    public static void PublishOnWillLeave()
    {
        OnWillLeave?.Invoke();
    }

    public static void PublishOnDidLeave()
    {
        OnDidLeave?.Invoke();
    }
}
