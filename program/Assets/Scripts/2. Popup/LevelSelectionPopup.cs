using System;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class LevelSelectionPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    
    
    public override IPopupAnimation GetAnimation()
    {
        return this;
    }
    public override string GetName()
    {
        return this.name;
    }

    public UniTask AnimationIn()
    {
        throw new System.NotImplementedException();
    }

    public UniTask AnimationOut()
    {
        throw new System.NotImplementedException();
    }
}
