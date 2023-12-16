using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;

public class LevelSelectionPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject MovableComponent;
    [SerializeField] public GameObject OrderContent;
    [SerializeField] public Transform Panel;

    private int playableLevels = 0;
    DateTime lastCheckTime;
    
    int[,] savedArrays = new int[,]
    {
        {0, 0, 0, 2},  //44 -0
        {0, 2, 0, 1},  //422 -1
        {1, 0, 1, 1},  //431 -2
        {0, 1, 2, 0},  //332 -3
        {1, 0, 2, 0},  //3311 -4
        {1, 2, 1, 0},  //3221 -5
        {3, 1, 1, 0},  //32111 -6
        {5, 0, 1, 0},  //311111 -7
        {0, 4, 0, 0},  //2222 -8
        {2, 3, 0, 0},  //22211 -9
        {4, 2, 0, 0},  //221111 -10
        {6, 1, 0, 0},  //2111111 -11
        {8, 0, 0, 0},  //11111111 -12
    };
    


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
        /*
        bool animationFinish = false;
        // 초기 알파 값 설정
        float initialAlpha = 0f;
        CanvasGroup canvasGroup = MovableComponent.GetComponent<CanvasGroup>();
        canvasGroup.alpha = initialAlpha;

        // 목표 알파 값 설정
        float targetAlpha = 1f;

        // 초기 위치 설정
        Vector2 initialPosition = Vector2.zero;
        RectTransform rectTransform = MovableComponent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = initialPosition;

        // 목표 위치 설정
        Vector2 targetPosition = Vector2.zero;

        // 애니메이션 시간 설정
        float animationDuration = 0.5f;

        // DOTween을 사용하여 애니메이션 적용
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(targetAlpha, animationDuration).SetEase(Ease.InQuad).From(initialAlpha));
        sequence.Join(rectTransform.DOAnchorPos(targetPosition, animationDuration).SetEase(Ease.InQuad).From(initialPosition));

        // 애니메이션이 시작되기 전에 실행할 코드 작성
        // 예: 팝업을 활성화하는 등의 작업
        gameObject.SetActive(true);
        */
        return UniTask.CompletedTask;
    }

    public UniTask AnimationOut()
    {
        /*
        bool animationFinish = false;
        // 초기 알파 값 설정
        float initialAlpha = 1f;
        CanvasGroup canvasGroup = MovableComponent.GetComponent<CanvasGroup>();
        canvasGroup.alpha = initialAlpha;

        // 목표 알파 값 설정
        float targetAlpha = 0f;

        // 초기 위치 설정
        Vector2 initialPosition = Vector2.zero;
        RectTransform rectTransform = MovableComponent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = initialPosition;

        // 목표 위치 설정
        Vector2 targetPosition = new Vector2(-500f, 500f);

        // 애니메이션 시간 설정
        float animationDuration = 0.5f;

        // Dotween을 사용하여 애니메이션 적용
        canvasGroup.DOFade(targetAlpha, animationDuration)
            .SetEase(Ease.OutQuad)
            .From(initialAlpha)
            .OnComplete(() =>
            {
                // 애니메이션이 완료된 후에 실행할 코드 작성
                // 예: 팝업을 비활성화하거나 삭제하는 등의 작업
                animationFinish = true;
            });

        rectTransform.DOAnchorPos(targetPosition, animationDuration)
            .SetEase(Ease.OutQuad)
            .From(initialPosition);
            
        
        return UniTask.WaitUntil(() => animationFinish);*/
        return UniTask.CompletedTask;
    }
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        quitButton.onClick.AddListener(OnClickQuit);
        
        
        // Setup 실행
        // Setup에서는 현재 시간에 대해 필요한 문제의 갯수를 출력한다.
        // 현재 시간에 대해 필요한 문제의 수는, 가장 마지막으로 문제를 해결한 시간을
        // PlayerPrefs에 저장한 뒤, 해당 시간을 확인하여
        // 현재와의 차이를 계산하여 확인하자.
        // 만약, 현재 시간의 차이가 6이상 발생하면 그냥 6으로 계산하도록 하자.
        
        Setup();

    }

    private void Setup()
    {
        if (PlayerPrefs.HasKey("PlayableLevels"))
        {
            playableLevels = PlayerPrefs.GetInt("PlayableLevels");
            if (DateTime.TryParse(PlayerPrefs.GetString("LastPlayableLevelCheckTime"), out lastCheckTime))
            {
                TimeSpan difference = DateTime.Now - lastCheckTime;
                int minutesDifference = (int)difference.TotalMinutes;
                playableLevels = Math.Min(minutesDifference + playableLevels, 6);
                PlayerPrefs.SetInt("PlayableLevels", playableLevels);

                //Todo : Remove Debug
                Debug.Log($"LevelSelectionPopup :: Setup : Current Available level : {playableLevels}");
            }
            else
            {
                Debug.LogError("LevelSelectionPopup :: Setup : Cannot Convert PP LastPlayableLevelCheckTime to DateTime!!");
            }
        }
        else
        {
            //한번도 플레이하지 않은 유저인 상황?
            playableLevels = 6;
            PlayerPrefs.SetInt("PlayableLevels", 6);
        }
        
        PlayerPrefs.SetString("LastPlayableLevelCheckTime", DateTime.Now.ToString());
        
        // 맞춰진 playable Levels 만큼 Level을 재생성한다.
        // 기존에 존재하던 레벨은 유지되어야 하나...?

        for (int i = 0; i < playableLevels; i++)
        {
            // Todo : DataSetup

            OrderData setupData = SetupOrderData();
            GameObject newOrder = Instantiate(OrderContent, Panel);
            
          
            newOrder.GetComponent<OrderContent>().Setup(setupData);
        }
    }

    private OrderData SetupOrderData()
    {
        OrderData setupData = new OrderData();

        // characterNum 설정
        setupData.characterNum = UnityEngine.Random.Range(0, 6);

        // orderValues 설정
        setupData.orderValues = new int[4];

        int indexOfQues = UnityEngine.Random.Range(0, 13);
        setupData.orderValues[0] = savedArrays[indexOfQues,0];
        setupData.orderValues[1] = savedArrays[indexOfQues,1];
        setupData.orderValues[2] = savedArrays[indexOfQues,2];
        setupData.orderValues[3] = savedArrays[indexOfQues,3];
        
        

        for (int i = 0; i < 4; i++)
        {
            /* Obsolete
             if (totalCount >= 12 - i - 1)
            {
                setupData.orderValues[i] = 0;
            }
            else
            {
                setupData.orderValues[i] = UnityEngine.Random.Range(0, 12 - i - 1 - totalCount);
                if (setupData.orderValues[i] < i + 1) setupData.orderValues[i] += i + 1;
            }

            totalCount += setupData.orderValues[i];*/
            
            


        }
        return setupData;
    }


    private void OnClickQuit()
    {
        PopupManager.Close();
    }
    
}
