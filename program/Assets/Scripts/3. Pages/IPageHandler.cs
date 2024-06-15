using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPageHandler : IHandler
{
    void Show();

    void Hide();

    string GetName();
}
