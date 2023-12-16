using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpecialGuestData
{
    public int customerType;
}

public class SpecialGuestPage : PageHandler
{
    public class QuestionDialogue
    {
        public string speaker;
        public string[] text;
    }

    public class QuestionDialogueContainer
    {
        public List<QuestionDialogue> dialogues;
    }
    
    [Header("Panels")]
    [SerializeField] private GameObject dialogueParent;
    [SerializeField] private GameObject question;
    
    [Header("Dialogues")]
    public TMP_Text speaker;
    public TMP_Text dialogueText;
    public Image speakerImage;
    [SerializeField] private Sprite[] speakerSprite;
    private string customerType;

    [Header("Question")] 
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text[] answerTexts;
    public Image backSpeakerImage;


    private int currentIndex = 0;
    private QuestionDialogueContainer dialogueContainer = new QuestionDialogueContainer();
    
    private string[] menuNames = new[] { "Mathpresso", "Amathricano", "Mathmoothie", "Mathcaron" };
    private int[] menuPrices = new[] { 200, 500, 800, 1200};

    private bool[] isCorrect = new bool[7];
    private int currentQuestionIndex = 0;

    private int nCustomerType;
    
    
    
    
    
    
    //문제에 필요한 변수들

    private int aDrinkType;
    private int aDrinkCount;
    private int bDrinkType;
    private int bDrinkCount;
    private int cDrinkType;
    private int cDrinkCount;
    private int rawAnswer;

    private int answerButton;
    private int discount;
    private string percent = "%";


    private int gainGold = 0;




    public override void OnWillEnter(object param)
    {
        
        /*
         * param에 필요한 것들
         *
         * 1. 누가 신청한건지.
         * 2. 문제에 필요한 수식 작성하기
         * 3. 답이 맞은지 틀린지 확인하기.
         * 
         */

        switch (param)
        {
            case 0:
                customerType = "cat";
                nCustomerType = 0;
                break;
            case 1:
                customerType = "tiger";
                nCustomerType = 1;
                break;
            case 2:
                customerType = "sheep";
                nCustomerType = 2;
                break;
            
            
        }
        backSpeakerImage.sprite = speakerSprite[nCustomerType];

        Setup();
        
        
    }

    private void Setup()
    {
        // 다이얼로그 출력
        
        dialogueParent.SetActive(true);
        
        currentIndex = 0;
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogues/FinalMath");
        
        dialogueContainer = JsonConvert.DeserializeObject<QuestionDialogueContainer>(jsonFile.text);
        
        if (jsonFile == null)
        {
            Debug.LogError("json not found!");
            return;
        }
        
        
        // index 0
        
        string rawText =  dialogueContainer.dialogues[currentIndex].text[PlayerPrefs.GetInt("LocalizationType")];
        string resultText = "";

        speaker.text = dialogueContainer.dialogues[currentIndex].speaker == "customer"
            ? LocalizationManager.Instance.Translate(customerType, "Button_Trans")
            : LocalizationManager.Instance.Translate(dialogueContainer.dialogues[currentIndex].speaker, "Button_Trans");

        speakerImage.sprite = dialogueContainer.dialogues[currentIndex].speaker == "customer"
            ? speakerSprite[nCustomerType]
            : speakerSprite[3];
        
        // 현재 text를 Question Text에 작성한다.
        
        
        // currentIndex에 맞춰서 문제와 답을 생성...
        switch (currentIndex)
        {
            case 0:
                aDrinkCount = Random.Range(2, 50);
                aDrinkType = Random.Range(0, 4);
                string aName = LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans");

                rawAnswer = aDrinkCount * menuPrices[aDrinkType];
                resultText = string.Format(rawText, aName, aDrinkCount);
                Debug.Log(resultText);
                break;
            default:
                break;
        }

        dialogueText.text = resultText;
        questionText.text = resultText;

        // 문제 생성 및 문제 표기(정답 저장)
        
        
    }

    public void OnClickNext()
    {
        SimpleSound.Play("touch");
        /*
         *  0/1 1번 문제
         *  2/3 2번
         *  4/5 3번
         *  6/78 4번
         *  9/10 11 5번
         *  12/13 14 6번
         *  15/16 17 7번
         */
        currentIndex += 1;

        switch (currentIndex)
        {
            case 0: 
            case 2: 
            case 4: 
            case 6: 
            case 9: 
            case 12: 
            case 15:
                SetupToQuestion();
                break;
            
            case 1:
            case 3:
            case 5:
            case 7:

            case 10:
            case 13:
            case 16:
                GoToQuestion();
                break;
            
            case 8:
            case 11:
            case 14:
            case 17:
                ToNextDialogue();
                break;
            case 18:
                //끝남. 정산(영수증?)하고 끝내면 됨.
                dialogueParent.SetActive(false);
                List<SpecialGuestMenuData> data = new List<SpecialGuestMenuData>();

                for (int i = 0; i < 7; i++)
                {
                    data.Add(new SpecialGuestMenuData());
                    data[i].number = i + 1;
                    data[i].isSuccess = isCorrect[i];
                }
                PopupManager.Show(nameof(SpecialGuestReceiptPopup), data);
                break;
            default:
                break;
        }
    }

    private void ToNextDialogue()
    {
        //이전 dialogue와 연결됨.
        
        string rawText =  dialogueContainer.dialogues[currentIndex].text[PlayerPrefs.GetInt("LocalizationType")];
        string resultText = "";
        
        // 현재 text를 Question Text에 작성한다.
        
        
        // currentIndex에 맞춰서 문제와 답을 생성...
        switch (currentIndex)
        {
            case 8:
                resultText = string.Format(rawText, discount,percent, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), menuPrices[aDrinkType], 
                    menuPrices[aDrinkType] * discount / 100, menuPrices[aDrinkType] * (100 - discount) / 100);
                break;
            case 11:
                resultText = string.Format(rawText, discount, percent, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), menuPrices[aDrinkType],
                    menuPrices[aDrinkType] * discount / 100);
                break;
            case 14:
                Debug.Log($"{rawText}");
                resultText = string.Format(rawText, aDrinkCount, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), bDrinkCount,
                    LocalizationManager.Instance.Translate(menuNames[bDrinkType], "Button_Trans"),
                    discount, percent, menuPrices[aDrinkType], menuPrices[bDrinkType],
                    (aDrinkCount * menuPrices[aDrinkType] + bDrinkCount * menuPrices[bDrinkType]) * discount / 100, rawAnswer);
                break;
            case 17:
                resultText = string.Format(rawText, discount, percent, aDrinkCount, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"),
                    menuPrices[aDrinkType], menuPrices[aDrinkType] * aDrinkCount * discount / 100, rawAnswer);
                break;
            default:
                break;
        }

        dialogueText.text = resultText;
        
    }

    private void SetupToQuestion()
    {
        string rawText =  dialogueContainer.dialogues[currentIndex].text[PlayerPrefs.GetInt("LocalizationType")];
        string resultText = "";
        
        speaker.text = dialogueContainer.dialogues[currentIndex].speaker == "customer"
            ? LocalizationManager.Instance.Translate(customerType, "Button_Trans")
            : LocalizationManager.Instance.Translate(dialogueContainer.dialogues[currentIndex].speaker, "Button_Trans");

        speakerImage.sprite = dialogueContainer.dialogues[currentIndex].speaker == "customer"
            ? speakerSprite[nCustomerType]
            : speakerSprite[3];
        
        // 현재 text를 Question Text에 작성한다.
        
        
        // currentIndex에 맞춰서 문제와 답을 생성...
        switch (currentIndex)
        {
            case 0:
                aDrinkCount = Random.Range(2, 50);
                aDrinkType = Random.Range(0, 4);

                rawAnswer = aDrinkCount * menuPrices[aDrinkType];
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), aDrinkCount);
                break;
                
            case 2: 
                aDrinkCount = Random.Range(2, 50);
                aDrinkType = Random.Range(0, 4);
                
                bDrinkCount = Random.Range(2, 50);
                while(bDrinkType != aDrinkType) bDrinkType = Random.Range(0, 4);
                
                
                rawAnswer = aDrinkCount * menuPrices[aDrinkType] + bDrinkCount * menuPrices[bDrinkType];
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), aDrinkCount,LocalizationManager.Instance.Translate(menuNames[bDrinkType], "Button_Trans"), bDrinkCount);
                break;
                
            case 4: 
                aDrinkCount = Random.Range(2, 50);
                aDrinkType = Random.Range(0, 4);
                
                bDrinkCount = Random.Range(2, 50);
                while(bDrinkType != aDrinkType) bDrinkType = Random.Range(0, 4);
                
                cDrinkCount = Random.Range(2, 50);
                while(cDrinkType != aDrinkType && cDrinkType != bDrinkType) cDrinkType = Random.Range(0, 4);
                
                rawAnswer = aDrinkCount * menuPrices[aDrinkType] + bDrinkCount * menuPrices[bDrinkType] + cDrinkCount * menuPrices[cDrinkType];
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), aDrinkCount,LocalizationManager.Instance.Translate(menuNames[bDrinkType], "Button_Trans"), bDrinkCount,LocalizationManager.Instance.Translate(menuNames[cDrinkType], "Button_Trans"), cDrinkCount);
                break;
                
            case 6: 
                aDrinkType = Random.Range(0, 4);
                discount = Random.Range(1, 10) * 10;
                rawAnswer = menuPrices[aDrinkType] * (100 - discount) / 100;
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), discount, percent);
                break;
                
            case 9:
                aDrinkType = Random.Range(0, 4);
                discount = Random.Range(1, 10) * 10;
                rawAnswer = menuPrices[aDrinkType] * discount / 100;
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), discount, percent);

                break;
                
            case 12:
                discount = Random.Range(1, 10) * 10;
                aDrinkCount = Random.Range(2, 50);
                aDrinkType = Random.Range(0, 4);
                
                bDrinkCount = Random.Range(2, 50);
                while(bDrinkType != aDrinkType) bDrinkType = Random.Range(0, 4);
                rawAnswer = (aDrinkCount * menuPrices[aDrinkType] + bDrinkCount * menuPrices[bDrinkType]) *
                    (100 - discount) / 100;
                resultText = string.Format(rawText,discount, percent, aDrinkCount, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), bDrinkCount, LocalizationManager.Instance.Translate(menuNames[bDrinkType], "Button_Trans"));

                break;
            case 15:
                discount = Random.Range(1, 10) * 10;
                aDrinkCount = Random.Range(2, 50);
                aDrinkType = Random.Range(0, 4);

                rawAnswer = (aDrinkCount * menuPrices[aDrinkType]) * (100-discount) / 100;
                resultText = string.Format(rawText, discount, percent, aDrinkCount, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"));
                break;
            default:
                break;
        }

        dialogueText.text = resultText;
        questionText.text = resultText;
        
    }

    private void GoToQuestion()
    {
        // 답은 이미 나와있고, 텍스트도 다 받은 상태.
        // 유저의 입력을 기다린다.
        
        // dialogue 끄기
        // button Setup
        
        dialogueParent.SetActive(false);
        question.SetActive(true);


        answerButton = Random.Range(0, 4);
        // 3
        // 0 3차이 얘는 600만큼 감소
        int difference = Random.Range(1, 8) * 100;

        for (int i = 0; i < 4; i++)
        {
            answerTexts[i].text = (rawAnswer + ((i - answerButton) * difference)).ToString();
        }
    }



    public void SelectAnswer(int answer)
    {
        
        //답이 1, 2, 3, 4번중 어떤건지 미리 저장해둬야 한다.
        //답 버튼이 눌려서, 이미 저장된 다이얼로그가 등장해야 한다.
        dialogueParent.SetActive(true);
        question.SetActive(false);

        if (answer == answerButton)
        {
            isCorrect[currentQuestionIndex] = true;
        }
        else
        {
            isCorrect[currentQuestionIndex] = false;
        }
        
        //답이 옳은지 그른지는 isCorrect[currentQuestionIndex]에 저장. 끝나면 인덱스 +1 해야함.
        currentQuestionIndex += 1;
        
        speaker.text = dialogueContainer.dialogues[currentIndex].speaker == "customer"
            ? LocalizationManager.Instance.Translate(customerType, "Button_Trans")
            : LocalizationManager.Instance.Translate(dialogueContainer.dialogues[currentIndex].speaker, "Button_Trans");

        speakerImage.sprite = dialogueContainer.dialogues[currentIndex].speaker == "customer"
            ? speakerSprite[nCustomerType]
            : speakerSprite[3];
        
        
        //다이얼로그 보여주기.
        
        string rawText =  dialogueContainer.dialogues[currentIndex].text[PlayerPrefs.GetInt("LocalizationType")];
        string resultText = "";
        
        // 현재 text를 Question Text에 작성한다.
        
        
        // currentIndex에 맞춰서 문제와 답을 생성...
        switch (currentIndex)
        {
            case 1:
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"),menuPrices[aDrinkType], aDrinkCount, rawAnswer);
                break;
                
            case 3:
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), menuPrices[aDrinkType], aDrinkCount,
                    LocalizationManager.Instance.Translate(menuNames[bDrinkType], "Button_Trans"), menuPrices[bDrinkType], bDrinkCount, rawAnswer);
                break;
            case 5:
                resultText = string.Format(rawText, LocalizationManager.Instance.Translate(menuNames[aDrinkType], "Button_Trans"), menuPrices[aDrinkType], aDrinkCount,
                    LocalizationManager.Instance.Translate(menuNames[bDrinkType], "Button_Trans"), menuPrices[bDrinkType], bDrinkCount, LocalizationManager.Instance.Translate(menuNames[cDrinkType], "Button_Trans"), menuPrices[cDrinkType], cDrinkCount,rawAnswer);
                break;
            case 7:
                resultText = string.Format(rawText, discount, percent);
                break;

            case 10:
                resultText = string.Format(rawText, discount, percent);
                break;
            case 13:
                resultText = string.Format(rawText, discount, percent);
                break;
            case 16:
                resultText = string.Format(rawText, discount, percent);
                break;
            default:
                break;
        }

        dialogueText.text = resultText;

    }

    private void OnDialogueFinish()
    {
        //문제 표기
    }
    
    
    

}
