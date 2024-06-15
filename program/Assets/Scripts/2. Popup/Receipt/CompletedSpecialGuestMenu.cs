using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialGuestMenuData
{
    public int number;
    public bool isSuccess;
}

public class CompletedSpecialGuestMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text number;
    [SerializeField] private Image score;
    [SerializeField] private Sprite[] scoreSprite;

    public void Setup(SpecialGuestMenuData data)
    {
        number.text = data.number.ToString();
        score.sprite = data.isSuccess ? scoreSprite[0] : scoreSprite[1];
    }
}
