using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescriptionPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private TMP_Text itemName;
<<<<<<< HEAD
    [SerializeField] public Image itemImage;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Button purchaseButton;

    private int price;
    private ItemData itemData;
=======
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image itemImage;
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
<<<<<<< HEAD
        purchaseButton.onClick.AddListener(OnClickPurchase);
=======
        quitButton.onClick.AddListener(OnClickQuit);
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
        Setup(param as ItemData);
    }

    public void Setup(ItemData data)
    {
<<<<<<< HEAD
        itemData = data;
        itemName.text = LocalizationManager.Instance.Translate(data.itemName, "Shop_Trans");
        itemDescription.text = LocalizationManager.Instance.Translate(data.itemDescription, "Shop_Trans");
        itemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{data.itemID}");
        
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
    }

    public void OnClickQuit()
    {
        SimpleSound.Play("touch");
=======
        itemName.text = data.itemName;
        itemDescription.text = data.itemDescription;
        itemImage.sprite = data.itemSprite;
    }

    private void OnClickQuit()
    {
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
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
<<<<<<< HEAD
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
=======
        
    }
    
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
    
}
