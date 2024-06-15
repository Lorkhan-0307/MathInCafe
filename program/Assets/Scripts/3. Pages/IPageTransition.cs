using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPageTransition {
    UniTask TransitionIn(IPageHandler page);
    UniTask TransitionOut(IPageHandler page);
}