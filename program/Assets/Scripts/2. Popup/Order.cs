using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Sprite[] ordersSprites;
    [SerializeField] private Image _image;
    
    // Start is called before the first frame update
    public void Setup(string menuType, int count)
    {
        switch (menuType)
        {
            case "Mathpresso":
                _image.sprite = ordersSprites[0];
                break;
            case "Amathricano":
                _image.sprite = ordersSprites[1];
                break;
            case "Mathmoothie":
                _image.sprite = ordersSprites[2];
                break;
            case "Mathcaron":
                _image.sprite = ordersSprites[3];
                break;
            default:
                break;
            
        }
        text.text = LocalizationManager.Instance.Translate(menuType, "Button_Trans") + " " +count +
                    LocalizationManager.Instance.Translate("cup", "Button_Trans");
    }
}
