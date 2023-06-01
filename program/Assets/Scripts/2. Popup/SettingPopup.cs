using System;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingPopup : BasePopup
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button hapticButton;
    [SerializeField] private GameObject MovableComponent;
    public override void OnEnterPopup()
    {
        AnimationActive();

        quitButton.onClick.AddListener(OnClickQuit);
        bgmButton.onClick.AddListener(OnClickBGM);
        sfxButton.onClick.AddListener(OnClickSFX);
        hapticButton.onClick.AddListener(OnClickHaptic);
        
        // Todo : 현재 상태를 확인 후 각 버튼의 Select, disSelect를 진행함?
        // 아래에서는 버튼 스프라이트를 보고 결정하는데, 이를 현 상태를 기준으로 진행해야 할 듯
    }

    private void AnimationActive()
    {
        // 초기 알파 값 설정
        float initialAlpha = 0f;
        CanvasGroup canvasGroup = MovableComponent.GetComponent<CanvasGroup>();
        canvasGroup.alpha = initialAlpha;

        // 목표 알파 값 설정
        float targetAlpha = 1f;

        // 초기 위치 설정
        Vector2 initialPosition = new Vector2(-500f, 500f);
        RectTransform rectTransform = MovableComponent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = initialPosition;

        // 목표 위치 설정
        Vector2 targetPosition = Vector2.zero;

        // 애니메이션 시간 설정
        float animationDuration = 0.5f;

        // Dotween을 사용하여 애니메이션 적용
        canvasGroup.DOFade(targetAlpha, animationDuration)
            .SetEase(Ease.InQuad)
            .From(initialAlpha)
            .OnStart(() =>
            {
                // 애니메이션이 시작되기 전에 실행할 코드 작성
                // 예: 팝업을 활성화하는 등의 작업
                gameObject.SetActive(true);
            });

        rectTransform.DOAnchorPos(targetPosition, animationDuration)
            .SetEase(Ease.InQuad)
            .From(initialPosition);
    }

    private void AnimationDisactive()
    {
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
                ClosePopup();
            });

        rectTransform.DOAnchorPos(targetPosition, animationDuration)
            .SetEase(Ease.OutQuad)
            .From(initialPosition);
    }

    public override void OnClosePopup()
    {
        throw new NotImplementedException();
    }

    private void OnClickBGM()
    {
        if (bgmButton.spriteState.selectedSprite)
        {
            Debug.Log("Disactivate BGM");
        }
        else
        {
            Debug.Log("Activate BGM");
        }
    }

    private void OnClickSFX()
    {
        if (bgmButton.spriteState.selectedSprite)
        {
            Debug.Log("Disactivate SFX");
        }
        else
        {
            Debug.Log("Activate SFX");
        }
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

    private void OnClickQuit()
    {
        AnimationDisactive();
    }
    
}