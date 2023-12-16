using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingPage : PageHandler
{
    [SerializeField] private TMP_Text loadingText;
    private void OnEnable()
    {
        loadingText.text = LocalizationManager.Instance.Translate("Cleaning cafe entrance...", "Button_Trans");
    }
}