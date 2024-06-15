using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlacementPopup :  CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private ItemData itemdata;

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
        Debug.Log("ENTERING Tutorial");
        base.OnWillEnter(param);


        Setup(param as ItemData);
    }

    private void Setup(ItemData data)
    {
        itemdata = data;
        itemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{data.itemID}");
        itemName.text = LocalizationManager.Instance.Translate(data.itemName, "Shop_Trans");
        itemDescription.text = LocalizationManager.Instance.Translate(data.itemDescription, "Shop_Trans");
    }

    public void Apply()
    {
        int spriteNum = 0;
        if (itemdata.itemType == 2)
        {
            switch (itemdata.itemID)
            {
                case 10:
                    spriteNum = 3;

                    break;
                case 11:
                    spriteNum = 2;
                    break;
                case 12:
                    spriteNum = 1;
                    break;
                default:
                    break;
            }
            PlayerPrefs.SetInt("SkinInfo", spriteNum);
            PopupManager.Close();
            PageManager.ChangeImmediate(nameof(MainPage));
        }
    }
    
    public void OnClickQuit()
    {
        SimpleSound.Play("touch");
        PopupManager.Close();
    }

}
