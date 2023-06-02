using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UGUI Canvas를 사용하는 Popup을 관리한다.
/// gameObject를 활성화할때 UGUI Canvas의 Transform들이 업데이트되면서 시간을 잡아먹기 때문에,
/// canvas.enabled를 이용해서 처리하도록 변경했다.
/// 하위의 GameObject들이 비활성화되지 않기 때문에 백그라운드에서 동작하는 작업이 있다면 직접 멈춰야 한다.
/// </summary>
public abstract class CanvasPopupHandler : PopupHandler {
    protected Canvas[] canvases;

    protected override void Awake() {
        base.Awake();
        canvases = GetComponentsInChildren<Canvas>(includeInactive: true);
    }

    public override void Show() {
        // 비활성화되어 있을 경우에는 활성화한다.(unity 작업중에 꺼놓을 가능성 고려)
        if (gameObject.activeSelf == false) {
            gameObject.SetActive(true);
        }

        if (canvases != null) {
            foreach (var canvas in canvases) {
                canvas.enabled = true;
            }
        }
    }

    public override void Hide() {
        if (canvases == null) return;

        foreach (var canvas in canvases) {
            canvas.enabled = false;
        }
    }

    public override bool IsActivePopup() {
        if (isActiveAndEnabled == false) return false;
        if (canvases.Length == 0) return false;
        return canvases[0].enabled;
    }

}
