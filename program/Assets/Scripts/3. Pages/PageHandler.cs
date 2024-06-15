using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageHandler : MonoBehaviour,IPageHandler
{
    public virtual void OnWillEnter(object param)
    {
    }

    public virtual void OnDidEnter(object param)
    {
    }

    public virtual void OnWillLeave()
    {
    }

    public virtual void OnDidLeave()
    {
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public string GetName()
    {
        return name;
    }
}
