using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;

public struct LevelData
{
    // 몇개의 문제를 가져와야 하는지
    public int nQuestion;
}

/*
 *PlayPage에서 해야 할 것들
 *
 *
 * 1. 문제 수를 받으면 이를 통해서, 문제의 갯수를 저장함.
 *
 * 현재 문제의 맞춘 수, 틀린 수, 연속하여 맞춘 수를 토대로 계산을 추가함
 *
 * 문제를 보여줌
 * 
 */

public class PlayPage : PageHandler
{
    [Header("Panels")]
    [SerializeField] private GameObject panel_diag_chooseDiff; // For FirstRun
    [SerializeField] private GameObject panel_question; //For Levels

    [Header("Texts")] 
    [SerializeField] private TMP_Text textDescription;
    [SerializeField] private TEXDraw texQuestion;

    [Header("For ChooseDiff")] [SerializeField]
    private Button[] btAnsr = new Button[4];
    
    public CurrentStatus currentStatus { get; set; } = CurrentStatus.WAITING;

    
    // nQuestion 이 0이면 firstRun으로 할까?
    private int nQuestion;
    int difficultyLevel = 0;
    
    
    
    
    // 일단 테스트를 위해서 API로 문제를 가져오는 부분만 생각해보자.
    
    public override void OnWillEnter(object param)
    {
        if (param is LevelData levelData)
        {
            nQuestion = levelData.nQuestion;

            switch (currentStatus)
            {
                case CurrentStatus.WAITING:
                    break;
                default:
                    break;
            }
            

        }
        else
        {
            Debug.LogError("PlayPage :: OnWillEnter : param not supported!");
        }
    }

    
    
    
    
    async void GetFirstDiagnosisQuestion()
    {
        await APIClient.Instance.FirstRun_Diagnosis(difficultyLevel);
    
        // 문제를 받아왔을 때 수행할 작업
        if (APIClient.Instance.cDiagnotics != null)
        {
            // 받아온 진단평가 문제 처리
            // APIClient.Instance.cDiagnotics를 사용하여 문제 정보에 접근할 수 있음
        }
    }


    public void OnClickDiff(int _idx)
    {
        currentStatus = CurrentStatus.DIAGNOSIS;
        APIClient.Instance.FirstRun_Diagnosis(_idx).Forget();
    }
    
}
