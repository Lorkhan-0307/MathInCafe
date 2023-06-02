using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalPopupAnimation : IPopupAnimation
{
    bool CanAnimateIn(string popupName);
    bool CanAnimateOut(string popupName);
}
