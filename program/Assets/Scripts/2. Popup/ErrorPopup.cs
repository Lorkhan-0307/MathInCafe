using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ErrorPopup :  CanvasPopupHandler, IPopupAnimation
{
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        StartCoroutine(CloseSelfAfterDelay(3f));
    }
    
    private void OnClickQuit()
    {
        PopupManager.Close();
    }
    
    
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
        return UniTask.CompletedTask;
    }

    public UniTask AnimationOut()
    {
        return UniTask.CompletedTask;
    }
    
    IEnumerator CloseSelfAfterDelay(float delay)
    {
        // 지정된 시간(3초) 동안 대기
        yield return new WaitForSeconds(delay);

        // 자신을 파괴
        OnClickQuit();
    }
}
