using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IPopupMemoryPolicy {
    /// <summary>
    /// 팝업을 로드한다.
    /// </summary>
    /// <param name="popupName"></param>
    /// <returns>로드한 팝업을 리턴한다. 로드할 수 없을 경우 null을 리턴한다.</returns>
    UniTask<IPopupHandler> LoadPopup(string popupName);
        
    /// <summary>
    /// 팝업을 파괴한다.
    /// </summary>
    /// <param name="popupName">팝업의 이름</param>
    /// <returns>파과되었을때 true를 반환한다.</returns>
    bool DestroyPopup(string popupName);
}
