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
    [SerializeField] private TMP_Text itemPriceText;
    private int price;

    public ItemData itemData;

    public void Setup(ItemData data)
    {
        itemData = data;
        itemNameText.text = LocalizationManager.Instance.Translate(data.itemName, "Shop_trans");
        itemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{data.itemID}");
        Debug.Log($"ItemSprite/{data.itemID}");
        this.GetComponent<Button>().onClick.AddListener(OnClickItemBox);

        switch (itemData.itemID)
        {
            case 0:
                price = 200;
                break;
            case 1:
                price = 400;
                break;
            case 2:
                price = 500;
                break;
            case 3:
                price = 3000;
                break;
            case 4:
                price = 5000;
                break;
            case 5:
                price = 5700;
                break;
            case 6:
                price = 3000;
                break;
            case 7:
                price = 6000;
                break;
            case 8:
                price = 8000;
                break;
            case 9:
                price = 9000;
                break;
            case 10:
                price = 15000;
                break;
            case 11:
                price = 20000;
                break;
            case 12:
                price = 25000;
                break;
            case 13:
                price = 1500;
                break;
            case 14:
                price = 1700;
                break;
            case 15:
                price = 2000;
                break;
            case 16:
                price = 5000;
                break;
            case 17:
                price = 3000;
                break;
            case 18:
                price = 1000;
                break;
            case 19:
                price = 1500;
                break;
            case 20:
                price = 3500;
                break;
            case 21:
                price = 20000;
                break;
            case 22:
                price = 25000;
                break;
            default:
                break;
        }

        if (itemPriceText != null)
        {
            itemPriceText.text = price.ToString();
        }
    }


    public void OnClickItemBox()
    {
        SimpleSound.Play("touch");
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

    public void OnClickPurchase()
    {
        if (ItemManager.gold >= price)
        {
            
            ItemManager.Instance.AddItem(itemData.itemID, 1);
            ItemManager.AddGold(price*(-1));
            OverlaySetup();
        }
        else
        {
            //구매불가 팝업
            PopupManager.Show(nameof(ErrorPopup));
        }
    }
    
    public void OverlaySetup()
    {
        // 현재 Scene에서 OverlaySetup 컴포넌트가 있는 모든 GameObject 찾기
        OverlaySetup[] overlaySetups = FindObjectsOfType<OverlaySetup>();

        // 각 OverlaySetup에서 SetOverlay 함수 실행
        foreach (OverlaySetup overlaySetup in overlaySetups)
        {
            overlaySetup.SetOverlay();
        }
    }

}
