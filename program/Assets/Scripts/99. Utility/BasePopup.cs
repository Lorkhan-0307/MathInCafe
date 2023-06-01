using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;



public abstract class BasePopup : MonoBehaviour
{
    protected PopupManager popupManager; // PopupManager 참조를 저장하기 위한 변수
    public string popupName;

    // Popup이 등장할 때 호출되는 함수
    public abstract void OnEnterPopup();
    public abstract void OnClosePopup();

    // Popup을 닫는 함수
    protected void ClosePopup()
    {
        if (popupManager != null)
        {
            popupManager.Close(this);
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    // PopupManager에게 자신을 등록하는 함수
    public void RegisterPopupManager(PopupManager manager)
    {
        popupManager = manager;
        popupName = this.gameObject.name;
    }
}