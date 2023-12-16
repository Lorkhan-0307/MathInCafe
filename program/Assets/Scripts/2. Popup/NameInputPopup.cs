using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;

public class NameInputPopup : CanvasPopupHandler, IPopupAnimation
{
    public TMP_InputField nameInputField;
    public GameObject popupPanel;
    
    public override IPopupAnimation GetAnimation()
    {
        return this;
    }
    public override string GetName()
    {
        return this.name;
    }

    public UniTask AnimationIn()
    {
        return UniTask.CompletedTask;
    }

    public UniTask AnimationOut()
    {
        return UniTask.CompletedTask;
    }
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        //Setup();
        
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string playerName = PlayerPrefs.GetString("PlayerName");
            nameInputField.text = playerName;
        }
    }
    
    public void SaveName()
    {
        string playerName = nameInputField.text;

        // 입력된 이름을 PlayerPrefs에 저장합니다.
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        // 팝업을 숨깁니다.
        PopupManager.Close();
        PopupManager.Show(nameof(TutorialDialoguePopup));
    }
}
