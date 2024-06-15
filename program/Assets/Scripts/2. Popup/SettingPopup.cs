using System;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utility;
using Sequence = DG.Tweening.Sequence;

public class SettingPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button hapticButton;
    [SerializeField] private GameObject MovableComponent;
    [SerializeField] private Toggle englishSelect;
    [SerializeField] private Toggle koreanSelect;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button restartTutorial;

    public override IPopupAnimation GetAnimation()
    {
        return this;
    }

    public override string GetName()
    {
        return this.name;
    }

    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        
        //AnimationActive();

        quitButton.onClick.AddListener(OnClickQuit);
        hapticButton.onClick.AddListener(OnClickHaptic);
        restartTutorial.onClick.AddListener(RestartTutorial);
        
        englishSelect.onValueChanged.AddListener(OnEnglishValueChanged);
        koreanSelect.onValueChanged.AddListener(OnKoreanValueChanged);

        if (PlayerPrefs.HasKey("LocalizationType"))
        {
            if (PlayerPrefs.GetInt("LocalizationType") == 0)
            {
                englishSelect.isOn = true;
                koreanSelect.isOn = false;
            }
            else
            {
                englishSelect.isOn = false;
                koreanSelect.isOn = true;
            }
        }
        
        // Todo : 현재 상태를 확인 후 각 버튼의 Select, disSelect를 진행함?
        // 아래에서는 버튼 스프라이트를 보고 결정하는데, 이를 현 상태를 기준으로 진행해야 할 듯
    }

    public override void OnWillLeave()
    {
        base.OnWillLeave();
    }

    private void OnClickHaptic()
    {
        if (bgmButton.spriteState.selectedSprite)
        {
            Debug.Log("Disactivate Haptic");
        }
        else
        {
            Debug.Log("Activate Haptic");
        }
    }

    private void OnEnglishValueChanged(bool isOn)
    {
        SimpleSound.Play("touch");
        if (isOn)
        {
            PlayerPrefs.SetInt("LocalizationType", 0);
            koreanSelect.isOn = false;
            englishSelect.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("LocalizationType", 1);
            englishSelect.isOn = false;
            koreanSelect.isOn = true;
        }
        
        LocalizationText[] localizationTexts = FindObjectsOfType<LocalizationText>();
        foreach (LocalizationText localizationText in localizationTexts)
        {
            localizationText.TranslateSelf();
        }
        
    }

    private void OnKoreanValueChanged(bool isOn)
    {
        SimpleSound.Play("touch");
        if (isOn)
        {
            PlayerPrefs.SetInt("LocalizationType", 1);
            englishSelect.isOn = false;
            koreanSelect.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("LocalizationType", 0);
            koreanSelect.isOn = false;
            englishSelect.isOn = true;
        }
        
        LocalizationText[] localizationTexts = FindObjectsOfType<LocalizationText>();
        foreach (LocalizationText localizationText in localizationTexts)
        {
            localizationText.TranslateSelf();
        }
    }

    private void OnClickQuit()
    {
        SimpleSound.Play("touch");
        PopupManager.Close();
    }

    private void RestartTutorial()
    {
        SimpleSound.Play("touch");
        PlayerPrefs.DeleteKey("TutorialBefore");
        PlayerPrefs.DeleteKey("TutorialAfter");
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        SoundHelper.VolumeMaster = volume;
    }


    public UniTask AnimationIn()
    {
        /*bool animationFinish = false;
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
        gameObject.SetActive(true);*/

        return UniTask.CompletedTask;
    }

    public UniTask AnimationOut()
    {
        /*bool animationFinish = false;
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
            .From(initialPosition);*/
        
        return UniTask.CompletedTask;
    }
}