using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuData
{
    public int number;
    public string menuName;
    public int menuCount;
    public bool isSuccess;
    public int totalGaingold;
}

public class CompletedMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text number;
    [SerializeField] private TMP_Text menuName;
    [SerializeField] private TMP_Text menuCount;
    [SerializeField] private Image score;
    [SerializeField] private Sprite[] scoreSprite;

    public void Setup(MenuData data)
    {
        number.text = data.number.ToString();
        Debug.Log($"AT COMPLETE MEUN : {number.text}");
        menuName.text = LocalizationManager.Instance.Translate(data.menuName, "Button_Trans");
        //menuCount.text = data.menuCount.ToString();
        score.sprite = data.isSuccess ? scoreSprite[1] : scoreSprite[0];
        Debug.Log($"AT COMPLETE MEUN : {data.number}");
    }
}

