using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 팝업을 필요할때만 생성하고 파괴한다.
/// </summary>
public class PopupInstantiatePolicy : IPopupMemoryPolicy {
    private readonly GameObject popupParent;
    private readonly Dictionary<string, GameObject> popupInstances = new Dictionary<string, GameObject>();
    
    public PopupInstantiatePolicy() {
        popupParent = new GameObject("Popup Parent");
        UnityEngine.Object.DontDestroyOnLoad(popupParent);
    }
    
    public async UniTask<IPopupHandler> LoadPopup(string popupName) {
        if (popupInstances.TryGetValue(popupName, out var popupInstance) == false) {
            popupInstance = await LoadAsync(popupName, popupParent.transform);
            if (popupInstance == null) return null;
            popupInstances[popupName] = popupInstance;
        }
        return popupInstance.GetComponent<IPopupHandler>();
    }

    public bool DestroyPopup(string popupName) {
        if (popupInstances.TryGetValue(popupName, out var popupInstance)) {
            popupInstances.Remove(popupName);
            UnityEngine.Object.Destroy(popupInstance);
            Resources.UnloadUnusedAssets();
            return true;
        }

        return false;
    }

    private async UniTask<GameObject> LoadAsync(string popupName, Transform parent) {
        var req = Resources.LoadAsync(popupName);

        if (req.asset != null) {
            return InstantiateFromResourceRequest(req, parent);
        }

        // await req;	//에셋 로드를 기다리지 않는것같아서 아랫줄로 바꿈
        await UniTask.WaitUntil(() => req.asset);
		
        return InstantiateFromResourceRequest(req, parent);
    }
    

    private GameObject InstantiateFromResourceRequest(ResourceRequest req, Transform parent) {
        if (req.asset == null) {
            throw new NullReferenceException("req.asset == null");
        }

        var go = req.asset as GameObject;
        if (go == null) {
            throw new Exception($"{req.asset.name} is not GameObject ({req.asset.GetType().Name}");
        }

        var cloned = UnityEngine.Object.Instantiate(go, parent);
        cloned.name = req.asset.name;
        cloned.SetActive(false); //처음부터 켜져있으면 PopupHandler의 IsActivePopup()에 걸려서 팝업이 열리지 않기때문에, 꺼준다.
        return cloned;
    }        

}