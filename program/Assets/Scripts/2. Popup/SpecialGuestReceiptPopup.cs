using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;

public class SpecialGuestReceiptPopup : CanvasPopupHandler, IPopupAnimation
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
        Debug.Log("ENTERING Special Guest RECEIPT");
        base.OnWillEnter(param);
        quitButton.onClick.AddListener(OnClickQuit);

        Setup(param as List<SpecialGuestMenuData>);

    }

    private void Setup(List<SpecialGuestMenuData> datas)
    {
        int totalGainGold = 0;
        
        //CompleteMenu에 PlayPage에서 받아온 값들을 넘겨주기.
        for (int i = 0; i < datas.Count; i++)
        {
            Debug.Log($"SPECIAL GUEST RECEIPT {i} / {datas.Count}");
            SpecialGuestMenuData data = datas[i];
            var newMenu = Instantiate(completedMenu, panel);
            newMenu.GetComponent<CompletedSpecialGuestMenu>().Setup(data);
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                    if (data.isSuccess)
                    {
                        totalGainGold += 1000;
                        ItemManager.AddHeart(5);
                    }
                    else
                    {
                        totalGainGold -= 500;
                    }
                    break;
                case 3:
                case 4:
                    if (data.isSuccess)
                    {
                        totalGainGold += 1400;
                        ItemManager.AddHeart(7);
                    }
                    else
                    {
                        totalGainGold -= 700;
                    }
                    
                    break;
                
                case 5:
                case 6:
                    if (data.isSuccess)
                    {
                        totalGainGold += 2000;
                        ItemManager.AddHeart(10);
                    }
                    else
                    {
                        totalGainGold -= 1000;
                    }
                    break;

                default:
                    break;
                
            }
            ItemManager.AddGold(totalGainGold);
            totalGold.text = totalGainGold.ToString() + " G";
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
