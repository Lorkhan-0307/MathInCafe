using System;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class LevelSelectionPopup : CanvasPopupHandler, IPopupAnimation
{
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject MovableComponent;
    
    
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

        return UniTask.CompletedTask;
    }

    public UniTask AnimationOut()
    {
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
        
        return UniTask.WaitUntil(() => animationFinish);
    }
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);

        quitButton.onClick.AddListener(OnClickQuit);

    }
    
    
    private void OnClickQuit()
    {
        PopupManager.Close();
    }
    
}
