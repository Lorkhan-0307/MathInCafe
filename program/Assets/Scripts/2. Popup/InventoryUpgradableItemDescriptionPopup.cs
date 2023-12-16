using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUpgradableItemDescriptionPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescriptionA;
    [SerializeField] private TMP_Text itemDescriptionB;
    [SerializeField] private Image itemImage;
    
    private int price;
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        Setup(param as ItemData);
    }
    
    public void OnClickQuit()
    {
        SimpleSound.Play("touch");
        PopupManager.Close();
    }

    public void Setup(ItemData data)
    {
        itemName.text = LocalizationManager.Instance.Translate(data.itemName, "Shop_Trans");
        string description = LocalizationManager.Instance.Translate(data.itemDescription, "Shop_Trans");
        string[] parts = description.Split('*');
        itemDescriptionA.text = parts[0];
        itemDescriptionB.text = parts[1];
        itemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{data.itemID}");
    }
    
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
}
