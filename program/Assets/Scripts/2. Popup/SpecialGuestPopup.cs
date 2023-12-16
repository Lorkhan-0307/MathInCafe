using System;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;

public class SpecialGuestPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject MovableComponent;



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
        return UniTask.CompletedTask;
    }

    public UniTask AnimationOut()
    {
        return UniTask.CompletedTask;
    }
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        quitButton.onClick.AddListener(OnClickQuit);
        //Setup();
    }
    
    private void OnClickQuit()
    {
        PopupManager.Close();
    }
<<<<<<< HEAD

    public void OnClickStart(int idx)
    {
        switch (idx)
        {
            case 0:
                PopupManager.Close();
                PopupManager.Show(nameof(DialoguePopup), "sheep");
                break;
            
            case 1:
                PopupManager.Close();
                PopupManager.Show(nameof(DialoguePopup), "tiger");
                break;
            case 2:
                PopupManager.Close();
                PopupManager.Show(nameof(DialoguePopup), "cat");
                break;
            default:
                break;
            
        }
    }
=======
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
}
