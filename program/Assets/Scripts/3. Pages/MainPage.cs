using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    [Header("BUTTONS")]
    [SerializeField] private Button SettingButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SettingButton.onClick.AddListener(OnClickSetting);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnClickSetting()
    {
        PopupManager.Show(nameof(SettingPopup));
    }
}
