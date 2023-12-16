using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct OrderData
{
    public int characterNum;
    public int[] orderValues;
}

public class OrderContent : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Sprite[] characterImageList;

    private int totalQuestion = 0;
    private string[] menuList;
    private int[] nMenuList;

    public void Setup(OrderData data)
    {
        nMenuList = data.orderValues;
        menuList = new string[4];
        // Localization 적용해야?
        menuList[0] = LocalizationManager.Instance.Translate("Mathpresso", "Button_Trans");
        menuList[1] = LocalizationManager.Instance.Translate("Amathricano", "Button_Trans");
        menuList[2] = LocalizationManager.Instance.Translate("Mathmoothie", "Button_Trans");
        menuList[3] = LocalizationManager.Instance.Translate("Mathcaron", "Button_Trans");

        string _orderText = "";
        int _coinText = 0;

        characterImage.sprite = characterImageList[Random.Range(0, 8)];
        


        // Todo : Image를 들어온 characterNum에 따라 변경시킴
        
        // 각 메뉴들의 설명 작성
        for (int i = 0; i < 4; i++)
        {
            totalQuestion += data.orderValues[i];
            if (data.orderValues[i] != 0)
            {
                menuList[i] = menuList[i] + " " + data.orderValues[i].ToString() + LocalizationManager.Instance.Translate("cup", "Button_Trans");;
            }
            else
            {
                menuList[i] = "";
            }

            _orderText += menuList[i];
            if(_orderText != "") _orderText += "  ";
            _coinText += (200 + 200 * (i + 1)) * data.orderValues[i];
        }

        orderText.text = _orderText;
        coinText.text = _coinText.ToString();
    }

    public void OnClickContent()
    {
<<<<<<< HEAD
        SimpleSound.Play("touch");
        // 문제 진입 방법
        LevelData data = new LevelData();
        data.nMenuList = nMenuList;
        

=======
        // 문제 진입 방법
        LevelData data = new LevelData();

        data.nQuestion = totalQuestion;
        
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
        PopupManager.Close();

        SwitchSceneManager.Instance.SwitchScene("Title", "PlayScene", () => {
            PageManager.ChangeImmediate("PlayPage", data);
        });
    }
}
