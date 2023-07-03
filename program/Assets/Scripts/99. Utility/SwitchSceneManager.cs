using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SwitchSceneManager : SingletonMonoBehaviour<SwitchSceneManager> {
    
    private Action onComplete;
    [SerializeField] GameObject loadingPageRoot;
    [SerializeField] GameObject[] tutorials;
    [SerializeField] GameObject weekendEventRoot;

    private const float MinLoadingTime = 1f;

    private void Start() {
        Debug.Log("SwitchSceneManager.Start");

#if UNITY_EDITOR
        SwitchScene("Title", "MainScene", () => {
            PageManager.ChangeImmediate("MainPage");
        });
#else
        if (PlatformContext.Instance.isFirstRun) {
            LevelPage.GoToLevel(1);
        } else {
            SwitchScene("Title", "LevelScene", () => {
                PageManager.ChangeImmediate("LevelPage");
            });
        }
#endif
    }

    public async void SwitchScene(string loadingPageName, string mainSceneName, Action action) {
        bool minTimeOk = false;
        DOVirtual.DelayedCall(MinLoadingTime, () => minTimeOk = true);
        
        onComplete = action;
        loadingPageRoot.SetActive(true);
        foreach(var t in tutorials) t.SetActive(false);

        tutorials[Random.Range(0, tutorials.Length)].SetActive(true);
        
        //PlayerPrefsHelper.ClearCache();
        PlayerPrefs.Save();
        PageManager.Clear();
        //PopupUtility.OnSceneChange();
        //EscapeButtonHandler.Instance.ClearListeners();
        
        System.GC.Collect();
        await Resources.UnloadUnusedAssets();
        
        await ChangeScene(mainSceneName);
        await UniTask.WaitUntil(() => minTimeOk);
        
        OnChangeSceneComplete();
        
        // 바뀐 씬이 초기화될 때까지 1프레임 대기 후 로딩화면을 끈다. (1프레임 후 팝업이 나오기 시작하여 화면이 깜빡이는 것처럼 보임)
        await UniTask.Yield();
        loadingPageRoot.SetActive(false);
    }

    public void OnChangeSceneUpdate(float progress) {
        // Ignored...
        // 업데이트 함수가 필요할 경우 SwitchScene 함수가 UnitySceneManager.Change을 사용하도록 변경할 것
    }

    public void OnChangeSceneComplete() {
        //PopupUtility.OnSceneChangeCompleted();
        onComplete?.Invoke();
    }

    async UniTask ChangeScene(string sceneName) {
        await SceneManager.LoadSceneAsync(sceneName);
        await UniTask.Yield();
    }
}
