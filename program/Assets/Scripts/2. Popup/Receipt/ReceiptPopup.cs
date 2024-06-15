using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;

public class ReceiptPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject completedMenu;
    [SerializeField] private Transform panel;

    [SerializeField] private TMP_Text totalGold;
    
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
        Debug.Log("ENTERING RECEIPT");
        base.OnWillEnter(param);
        quitButton.onClick.AddListener(OnClickQuit);

        Setup(param as List<MenuData>);
        SimpleSound.Play("receipt");

    }

    private void Setup(List<MenuData> datas)
    {
        totalGold.text = datas[0].totalGaingold.ToString() + " G";
        //CompleteMenu에 PlayPage에서 받아온 값들을 넘겨주기.
        for (int i = 0; i < datas.Count; i++)
        {
            MenuData data = datas[i];
            var newMenu = Instantiate(completedMenu, panel);
            newMenu.GetComponent<CompletedMenu>().Setup(data);
        }
    }

    private void OnClickQuit()
    {
        PopupManager.Close();
        SwitchSceneManager.Instance.SwitchScene("Title", "MainScene", () => {
            PageManager.ChangeImmediate("MainPage");
        });
    }

}
