using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopupHandler : IHandler
{
    void Show();

    void Hide();

    string GetName();

    bool IsActivePopup();

    int GetSortingOrder();

    void AddSortingOrder(int order);

    void ResetSortingOrder();

    IPopupAnimation GetAnimation();
}
