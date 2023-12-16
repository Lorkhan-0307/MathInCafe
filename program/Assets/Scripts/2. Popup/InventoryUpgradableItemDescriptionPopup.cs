using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
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
=======
using UnityEngine;

public class InventoryUpgradableItemDescriptionPopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
    }
}
