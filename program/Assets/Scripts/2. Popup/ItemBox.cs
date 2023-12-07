using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;


public class ItemBox : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;

    public ItemData itemData;

    public void Setup(ItemData data)
    {
        itemData = data;
        itemNameText.text = data.itemName;
        itemImage.sprite = data.itemSprite;
        this.GetComponent<Button>().onClick.AddListener(OnClickItemBox);
    }


    public void OnClickItemBox()
    {
        // 현재 페이지가 무엇인지 확인해서, 각각 다른 팝업을 보이도록 설정
        if (PageManager.CurrentPage.GetName().Equals("InventoryPage"))
        {
            if(itemData.isUpgradable) PopupManager.Show(nameof(InventoryUpgradableItemDescriptionPopup), itemData);
            else PopupManager.Show(nameof(ItemPlacementPopup), itemData);
        }
        else
        {
            if(itemData.isUpgradable) PopupManager.Show(nameof(UpgradableItemDescriptionPopup), itemData);
            else PopupManager.Show(nameof(ItemDescriptionPopup), itemData);
        }
        
    }
}
