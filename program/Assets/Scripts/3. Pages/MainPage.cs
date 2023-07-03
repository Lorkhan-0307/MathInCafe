using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPage : PageHandler
{
    [Header("BUTTONS")]
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button PlayButton;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        SettingButton.onClick.AddListener(OnClickSetting);
        PlayButton.onClick.AddListener(OnClickPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnClickSetting()
    {
        PopupManager.Show(nameof(SettingPopup));
    }

    private void OnClickPlay()
    {
        //SceneManager.LoadScene("PlayScene");
        PageManager.Change("LevelPage");
    }
}
