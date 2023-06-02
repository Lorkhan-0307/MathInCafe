using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPopupAnimation
{
    UniTask AnimationIn();
    UniTask AnimationOut();
}
