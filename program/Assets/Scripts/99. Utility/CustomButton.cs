using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler
{
    public Image buttonImage;
    public Text buttonText;
    public Button.ButtonClickedEvent onClick;

    private void Awake()
    {
        // 필요한 초기화 작업 수행
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<Text>();

        // 클릭 이벤트 핸들러 등록
        Button buttonComponent = GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(InvokeClickEvent);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭 이벤트 처리
        InvokeClickEvent();
    }

    private void InvokeClickEvent()
    {
        // 등록된 클릭 이벤트 호출
        onClick?.Invoke();
    }

    // 기타 필요한 메서드 구현
}