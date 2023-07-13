using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class MainPage : PageHandler
{
    [Header("BUTTONS")]
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button PlayButton;
    
    public override void OnDidEnter(object param)
    {
        
    }
    
    
    
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

    private static async void OnClickPlay()
    {
        PopupManager.Show(nameof(LevelSelectionPopup));
    }
    
    public static async void GoToLevel() {
        //Maybe a ScreenLock

        if (PageManager.CurrentPage == null || PageManager.CurrentPage.GetName() == "LevelPage") {
            SwitchSceneManager.Instance.SwitchScene("LevelToPlay", "PlayScene", () => {
                GoToLevelCallback();
            });
        } else if (PageManager.CurrentPage.GetName() == "PlayPage") {
            GoToLevelCallback();
        } else {
            Debug.LogError($"{PageManager.CurrentPage.GetName()} is not valid !!");
        }
    }
    
    
    private static void GoToLevelCallback() {
        PageManager.ChangeImmediate("PlayPage");
    }
}
