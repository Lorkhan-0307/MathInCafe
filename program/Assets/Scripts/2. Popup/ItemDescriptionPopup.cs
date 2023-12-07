using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image itemImage;
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        quitButton.onClick.AddListener(OnClickQuit);
        Setup(param as ItemData);
    }

    public void Setup(ItemData data)
    {
        itemName.text = data.itemName;
        itemDescription.text = data.itemDescription;
        itemImage.sprite = data.itemSprite;
    }

    private void OnClickQuit()
    {
        PopupManager.Close();
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

    public void OnClickPurchase()
    {
        
    }
    
    
}
