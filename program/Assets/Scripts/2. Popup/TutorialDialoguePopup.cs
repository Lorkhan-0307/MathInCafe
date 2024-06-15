using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;



public class TutorialDialoguePopup : CanvasPopupHandler, IPopupAnimation
{
    [System.Serializable]
    public class Dialogue
    {
        public string[] text;
    }

    [System.Serializable]
    public class DialogueContainer
    {
        public List<List<string>> dialogues;
    }
    
    private int currentIndex;
    public TMP_Text speaker;
    public Image speakerImage;
    public TMP_Text text;
    [SerializeField] private Sprite[] speakerSprite;
    private DialogueContainer dialogueContainer = new DialogueContainer();

    
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
        Debug.Log("ENTERING Tutorial");
        base.OnWillEnter(param);
        
        
        Setup();
        
    }

    void Setup()
    {
        currentIndex = 0;

        string path = "";

        if (!PlayerPrefs.HasKey("TutorialBefore"))
        {
            path = "Tutorial_beforeTest";
        }
        else
        {
            path = "Tutorial_afterTest";
        }
        
        TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogues/{path}");
        
        
        if (jsonFile == null)
        {
            Debug.LogError("json not found!");
            return;
        }
        
        dialogueContainer = JsonConvert.DeserializeObject<DialogueContainer>(jsonFile.text);
        
        speaker.text = dialogueContainer.dialogues[currentIndex][2] == "0" ? LocalizationManager.Instance.Translate("woong", "Button_Trans") : LocalizationManager
            .Instance.Translate("jin", "Button_Trans");
        
        speakerImage.sprite = dialogueContainer.dialogues[currentIndex][2] == "0" ? speakerSprite[0] : speakerSprite[1];

        text.text = dialogueContainer.dialogues[0][PlayerPrefs.GetInt("LocalizationType")];
        currentIndex += 1;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClickNext(int answer = 0)
    {
        SimpleSound.Play("touch");
        if (currentIndex + 1 >= dialogueContainer.dialogues.Count)
        {
            if (!PlayerPrefs.HasKey("TutorialBefore"))
            {
                PlayerPrefs.SetInt("TutorialBefore", 1);
            }
            else
            {
                PlayerPrefs.SetInt("TutorialAfter", 1);
            }

            // 마지막 대화까지 진행한 경우 팝업을 닫습니다.
            PopupManager.Close();
            return;
        }

         speaker.text = dialogueContainer.dialogues[currentIndex][2] == "0" ? LocalizationManager.Instance.Translate("woong", "Button_Trans") : LocalizationManager
        .Instance.Translate("jin", "Button_Trans");
        
        speakerImage.sprite = dialogueContainer.dialogues[currentIndex][2] == "0" ? speakerSprite[0] : speakerSprite[1];
        
        text.text = dialogueContainer.dialogues[currentIndex][PlayerPrefs.GetInt("LocalizationType")];

        currentIndex += 1;
        
        
    }
    
    
}
