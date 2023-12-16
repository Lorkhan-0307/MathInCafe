using System;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;
using TMPro;
using UnityEngine.Serialization;

public class QuestPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject MovableComponent;
    
    // 0 = daily, 1 = level
    [SerializeField] private GameObject[] QuestList;
    [SerializeField] private GameObject[] QuestButtonList;

    [SerializeField] private Image dailyQuestImage;
    [SerializeField] private Image levelQuestImage;
    [SerializeField] private Sprite[] dailyQuestImageSprite;
    [SerializeField] private Sprite[] levelQuestImageSprite;

    [SerializeField] private TMP_Text dailyQuestText;
    [SerializeField] private TMP_Text levelQuestText;

    [SerializeField] private GameObject bigQuest;
    [SerializeField] private GameObject smallQuest;

    private Color onColor;
    private Color offColor;
    


#region OverrideSetup
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
        quitButton.onClick.AddListener(OnClickQuit);
        
        Setup();
    }

    private void OnClickQuit()
    {
        PopupManager.Close();
    }
    
#endregion


    private void Setup()
    {
        onColor = HexToColor("A67E43");
        offColor = HexToColor("CBB996");
        
        // 시작했을때, 퀘스트들을 초기화한다.
        // 먼저, 언제나 Daily Quest 항목들을 먼저 불러온다.
        QuestButtonList[0].GetComponent<Button>().onClick.Invoke();
        SetDailyQuest();
    }

    public void OnClickQuestButton(int index = 0)
    {
        QuestButtonList[index].GetComponent<Button>().image.sprite =
            QuestButtonList[index].GetComponent<Button>().spriteState.selectedSprite;
        QuestButtonList[1-index].GetComponent<Button>().image.sprite =
            QuestButtonList[1-index].GetComponent<Button>().spriteState.disabledSprite;
        QuestList[index].SetActive(true);
        QuestList[1 - index].SetActive(false);

        //DestroyChildren(QuestList[index].transform);
        

        if (index == 0)
        {
            
            dailyQuestImage.sprite = dailyQuestImageSprite[0];
            dailyQuestText.color = onColor;

            levelQuestImage.sprite = levelQuestImageSprite[1];
            levelQuestText.color = offColor;
            
            SetDailyQuest();
        }

        else
        {
            dailyQuestImage.sprite = dailyQuestImageSprite[1];
            dailyQuestText.color = offColor;

            levelQuestImage.sprite = levelQuestImageSprite[0];
            levelQuestText.color = onColor;
            
            SetLevelQuest();
        }
        
        
        // 선택된 퀘스트의 리스트를 뽑아와서 생성하는 항목이 필요하다.
        
    }

    private void SetDailyQuest()
    {
        DestroyChildren(QuestList[0].transform);

        foreach (QuestData data in QuestManager.dailyQuestList)
        {
<<<<<<< HEAD
=======
            Debug.Log($"Building {data.questContent}");
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
            if (data.heartReward > 0)
            {
                // Big Quest
                GameObject newQuest = Instantiate(bigQuest, QuestList[0].transform);
                newQuest.GetComponent<Quest>().Setup(data);
            }
            else
            {
                // Small Quest
                GameObject newQuest = Instantiate(smallQuest, QuestList[0].transform);
                newQuest.GetComponent<Quest>().Setup(data);
            }
        }
    }

    private void SetLevelQuest()
    {
        
<<<<<<< HEAD
=======
        Debug.Log($"Quest Count : {QuestManager.currentLevelQuestList.Count}");
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
        DestroyChildren(QuestList[1].transform);
        
        foreach (QuestData data in QuestManager.currentLevelQuestList)
        {
            if (data.heartReward > 0)
            {
                // Big Quest
                GameObject newQuest = Instantiate(bigQuest, QuestList[1].transform);
                newQuest.GetComponent<Quest>().Setup(data);
            }
            else
            {
                // Small Quest
                GameObject newQuest = Instantiate(smallQuest, QuestList[1].transform);
                newQuest.GetComponent<Quest>().Setup(data);
            }
        }
    }


    private Color HexToColor(string hex)
    {
        Color color = new Color();

        // 색상 코드를 RGB 값으로 변환합니다.
        UnityEngine.ColorUtility.TryParseHtmlString("#" + hex, out color);

        return color;
    }

    public void DestroyChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }
}
