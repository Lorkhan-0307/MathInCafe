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
    [SerializeField] private Button QuestButton;
    [SerializeField] private Button SpecialGuestButton;
    [SerializeField] private Button InventoryButton;
    [SerializeField] private Button ShopButton;



    [Header("CAFE")]
    [SerializeField] private Image cafeSkin;
    [SerializeField] private Sprite[] cafeSkinList;
    [SerializeField] private GameObject cafeObjectParent;

    

    private GameObject[] cafeObjectList;
    
    public override void OnDidEnter(object param)
    {
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        SettingButton.onClick.AddListener(OnClickSetting);
        PlayButton.onClick.AddListener(OnClickPlay);
        QuestButton.onClick.AddListener(OnClickQuest);
        SpecialGuestButton.onClick.AddListener(OnClickSpecialGuest);
        InventoryButton.onClick.AddListener(OnClickInventory);
        ShopButton.onClick.AddListener(OnClickShop);
    }

    private void CafeSetup()
    {
        if (PlayerPrefs.HasKey("SkinInfo"))
        {
            // 저장된 스킨이 있는 경우 해당 스킨 적용
            // 스킨 적용을 누르면, 해당 스킨에 맞는 int형이 저장됨.
            
            // 스킨의 경우, 다음과 같다.
            /*
             * Default == 0
             * Retro == 1
             * Halloween == 2
             * Garden == 3
             * 
             */
            
            
            cafeSkin.sprite = cafeSkinList[PlayerPrefs.GetInt("SkinInfo")];
        }
        else
        {
            // 없는 경우 기본스킨 적용
            cafeSkin.sprite = cafeSkinList[0];
        }
        
        // 저장된 책상, 아이템 등을 불러와야 한다.
        
        
        
    }

    private void OnClickSetting()
    {
        PopupManager.Show(nameof(SettingPopup));
    }

    private static async void OnClickPlay()
    {
        PopupManager.Show(nameof(LevelSelectionPopup));
    }

    private void OnClickQuest()
    {
        PopupManager.Show(nameof(QuestPopup));
    }

    private void OnClickSpecialGuest()
    {
        PopupManager.Show(nameof(SpecialGuestPopup));
    }

    private void OnClickInventory()
    {
        PageManager.ChangeImmediate(nameof(InventoryPage));
    }

    private void OnClickShop()
    {
        PageManager.ChangeImmediate(nameof(ShopPage));
    }

    public static async void GoToLevel()
    {
        //Maybe a ScreenLock

        if (PageManager.CurrentPage == null || PageManager.CurrentPage.GetName() == "LevelPage")
        {
            SwitchSceneManager.Instance.SwitchScene("LevelToPlay", "PlayScene", () => { GoToLevelCallback(); });
        }
        else if (PageManager.CurrentPage.GetName() == "PlayPage")
        {
            GoToLevelCallback();
        }
        else
        {
            Debug.LogError($"{PageManager.CurrentPage.GetName()} is not valid !!");
        }
    }


    private static void GoToLevelCallback() {
        PageManager.ChangeImmediate("PlayPage");
    }
}

[System.Serializable]
public class CafeData
{
    public int cafeSkinID;
    public int objectNumber;
    public Vector2 objectXYPos;
    

}
