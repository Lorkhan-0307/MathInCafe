using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandler
{
    void OnWillEnter(object param);

    void OnDidEnter(object param);

    void OnWillLeave();

    void OnDidLeave();
}
