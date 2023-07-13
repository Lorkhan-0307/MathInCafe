using System.Collections;
using System.Collections.Generic;
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
    
    private string[] menuList;

    public void Setup(OrderData data)
    {
        menuList = new string[4];
        menuList[0] = "매쓰프레소";
        menuList[1] = "매쓰리카노";
        menuList[2] = "매쓰무디";
        menuList[3] = "매쓰카롱";

        string _orderText = "";
        int _coinText = 0;

        // Todo : Image를 들어온 characterNum에 따라 변경시킴
        
        // 각 메뉴들의 설명 작성
        for (int i = 0; i < 4; i++)
        {
            if (data.orderValues[i] != 0)
            {
                menuList[i] = menuList[i] + " " + data.orderValues[i].ToString() + "잔";
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
}
