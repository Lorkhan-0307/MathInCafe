using System;
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

    [Header("Texts On Question")] 
    [SerializeField] private TMP_Text textQuestionDescription;
    [SerializeField] private TEXDraw texQuestion;
    [SerializeField] private Button[] btnAns = new Button[4];
    private TEXDraw[] textAnsr;
    
    [Header("Texts On ChooseDiff")] 
    [SerializeField] private TMP_Text textDiffDescription;
    
    [Header("For ChooseDiff")] [SerializeField]
    private Button[] btnDiff = new Button[4];
    
    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;
    
    [SerializeField] Button             getLearningButton;

    
    public CurrentStatus currentStatus { get; set; } = CurrentStatus.WAITING;

    
    // nQuestion 이 0이면 firstRun으로 할까?
    private int nQuestion;
    int difficultyLevel = 0;
    
    
    
    
    // 일단 테스트를 위해서 API로 문제를 가져오는 부분만 생각해보자.
    
    public override void OnWillEnter(object param)
    {
        textAnsr = new TEXDraw[btnAns.Length];
        for (int i = 0; i < btnAns.Length; ++i)
        {
            textAnsr[i] = btnAns[i].GetComponentInChildren<TEXDraw>();
        }

        if (param is LevelData levelData)
        {
            nQuestion = levelData.nQuestion;

            switch (currentStatus)
            {
                case CurrentStatus.WAITING:
                    panel_diag_chooseDiff.SetActive(true);
                    break;
                default:
                    break;
            }
            
            Debug.Log("APIClient check");
            if (APIClient.Instance != null)
            {
                Debug.Log("APIClient check2");

                APIClient.Instance.onGetDiagnosis.AddListener(() => GetDiagnosis());
                APIClient.Instance.onGetLearning.AddListener(() => GetLearning(0));
            }
            

        }
        else
        {
            Debug.LogError("PlayPage :: OnWillEnter : param not supported!");
        }
    }

    
    
    public void SelectAnswer(int _idx)
    {
        bool isCorrect;
        string ansrCwYn = "N";

        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                isCorrect   = String.Compare(textAnsr[_idx].text, APIClient.Instance.cDiagnotics.data.qstCransr, StringComparison.Ordinal) == 0 ? true : false;
                ansrCwYn    = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;

                APIClient.Instance.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                //wj_displayText.SetState("진단평가 중", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " 초");

                panel_question.SetActive(false);
                questionSolveTime = 0;
                break;

            case CurrentStatus.LEARNING:
                isCorrect   = String.Compare(textAnsr[_idx].text, APIClient.Instance.cLearnSet.data.qsts[currentQuestionIndex].qstCransr, StringComparison.Ordinal) == 0 ? true : false;
                ansrCwYn    = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;
                currentQuestionIndex++;

                APIClient.Instance.Learning_SelectAnswer(currentQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000)).Forget();

                //wj_displayText.SetState("문제풀이 중", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " 초");

                if (currentQuestionIndex >= 8) 
                {
                    panel_question.SetActive(false);
                    //wj_displayText.SetState("문제풀이 완료", "", "", "");
                }
                else GetLearning(currentQuestionIndex);

                questionSolveTime = 0;
                break;
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
    
    /// <summary>
    /// 받아온 데이터를 가지고 문제를 표시
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);

        string      correctAnswer;
        string[]    wrongAnswers;

        textQuestionDescription.text = textCn;
        Debug.Log(qstCn);
        texQuestion.text = qstCn;

        correctAnswer = qstCransr;
        wrongAnswers    = qstWransr.Split(',');

        int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;

        for(int i=0; i<btnAns.Length; i++)
        {
            if (i < ansrCount)
                btnAns[i].gameObject.SetActive(true);
            else
                btnAns[i].gameObject.SetActive(false);
        }

        int ansrIndex = UnityEngine.Random.Range(0, ansrCount);

        for(int i = 0, q = 0; i < ansrCount; ++i, ++q)
        {
            if (i == ansrIndex)
            {
                textAnsr[i].text = correctAnswer;
                --q;
            }
            else
                textAnsr[i].text = wrongAnswers[q];
        }
        isSolvingQuestion = true;
    }
    
    /// <summary>
    ///  n 번째 학습 문제 받아오기
    /// </summary>
    private void GetLearning(int _index)
    {
        if (_index == 0) currentQuestionIndex = 0;

        MakeQuestion(APIClient.Instance.cLearnSet.data.qsts[_index].textCn,
            APIClient.Instance.cLearnSet.data.qsts[_index].qstCn,
            APIClient.Instance.cLearnSet.data.qsts[_index].qstCransr,
            APIClient.Instance.cLearnSet.data.qsts[_index].qstWransr);
    }

    private void GetDiagnosis()
    {
        switch (APIClient.Instance.cDiagnotics.data.prgsCd)
        {
            case "W":
                MakeQuestion(APIClient.Instance.cDiagnotics.data.textCn, 
                    APIClient.Instance.cDiagnotics.data.qstCn, 
                    APIClient.Instance.cDiagnotics.data.qstCransr, 
                    APIClient.Instance.cDiagnotics.data.qstWransr);
                //wj_displayText.SetState("진단평가 중", "", "", "");
                break;
            case "E":
                Debug.Log("진단평가 끝! 학습 단계로 넘어갑니다.");
                //wj_displayText.SetState("진단평가 완료", "", "", "");
                currentStatus = CurrentStatus.LEARNING;
                getLearningButton.interactable = true;
                break;
        }
    }


    public void OnClickDiff(int idx)
    {
        currentStatus = CurrentStatus.DIAGNOSIS;
        APIClient.Instance.FirstRun_Diagnosis(idx).Forget();
    }
    
    
    public void OnClickGetLearning()
    {
        APIClient.Instance.Learning_GetQuestion();
        //wj_displayText.SetState("문제풀이 중", "-", "-", "-");
    }
}
