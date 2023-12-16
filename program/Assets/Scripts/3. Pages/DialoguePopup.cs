using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;



public class DialoguePopup : CanvasPopupHandler, IPopupAnimation
{
    [System.Serializable]
    public class Option
    {
        public List<string> text;
    }
    
    [System.Serializable]
    public class Dialogue
    {
        public string speaker;
        public string[] text;
        public List<Option> options;
        public List<List<string>> answer;
    }

    [System.Serializable]
    public class DialogueContainer
    {
        public List<Dialogue> dialogues;
    }
    
    private int currentIndex;
    public TMP_Text speaker;
    public TMP_Text text;
    public Image speakerImage;
    [SerializeField] private Sprite[] speakerSprite;
    [SerializeField] private TMP_Text[] selectAnswers;
    [SerializeField] private GameObject optionsParent;
    private bool isOptional;
    private DialogueContainer dialogueContainer = new DialogueContainer();

    private int speakerType;
    
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
        Debug.Log("ENTERING RECEIPT");
        base.OnWillEnter(param);
        
        
        Setup(param as string);
        
    }

    void Setup(string speakerName)
    {
        // Start에 있는걸 전부 여기로 가져와야 함.
        
        string path = speakerName + Random.Range(1, 7).ToString() + "_dialogue";

        switch (speakerName)
        {
            case "cat":
                speakerImage.sprite = speakerSprite[0];
                speakerType = 0;
                break;
            case "sheep":
                speakerImage.sprite = speakerSprite[2];
                speakerType = 2;
                break;
            case "tiger":
                speakerImage.sprite = speakerSprite[1];
                speakerType = 1;
                break;
            default:
                break;
        }

        
        
        currentIndex = 0;
        TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogues/{path}");
        dialogueContainer = JsonConvert.DeserializeObject<DialogueContainer>(jsonFile.text);
        
        if (jsonFile == null)
        {
            Debug.LogError("json not found!");
            return;
        }

        speaker.text = LocalizationManager.Instance.Translate(dialogueContainer.dialogues[0].speaker, "Button_Trans");

        text.text = dialogueContainer.dialogues[0].text[PlayerPrefs.GetInt("LocalizationType")];

        if (dialogueContainer.dialogues[0].options != null)
        {
            isOptional = true;
            foreach (Option option in dialogueContainer.dialogues[0].options)
            {
                Debug.Log("Option: " + option.text);
            }
        }

        currentIndex += 1;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClickNext(int answer = 0)
    {
        SimpleSound.Play("touch");
        if (isOptional)
        {
            optionsParent.SetActive(false);
            isOptional = false;
            speaker.text = LocalizationManager.Instance.Translate(dialogueContainer.dialogues[currentIndex].speaker, "Button_Trans");
            //text.text = dialogueContainer.dialogues[currentIndex].answer.ToString();
            
            if (dialogueContainer.dialogues[currentIndex].answer != null)
            {
                List<string> answerList = dialogueContainer.dialogues[currentIndex].answer[answer];
                text.text = answerList[PlayerPrefs.GetInt("LocalizationType")];
            }

            currentIndex += 1;
        }
        else
        
        {
            if (currentIndex + 1 >= dialogueContainer.dialogues.Count)
            {
                // 마지막 대화까지 진행한 경우 팝업을 닫습니다.
                PopupManager.Close();
                SwitchSceneManager.Instance.SwitchScene("Title", "PlayScene", () => {
                    PageManager.ChangeImmediate("SpecialGuestPage", speakerType);
                });
                return;
            }
            
            speaker.text = LocalizationManager.Instance.Translate(dialogueContainer.dialogues[currentIndex].speaker, "Button_Trans");
            text.text = dialogueContainer.dialogues[currentIndex].text[PlayerPrefs.GetInt("LocalizationType")];

            if (dialogueContainer.dialogues[currentIndex].options != null)
            {
                isOptional = true;
                
                optionsParent.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    Option option = dialogueContainer.dialogues[currentIndex].options[i];
                    selectAnswers[i].text = option.text[PlayerPrefs.GetInt("LocalizationType")];
                }
            }

            currentIndex += 1;
        }
        
    }
    
    
}
