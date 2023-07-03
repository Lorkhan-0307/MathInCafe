using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using String = System.String;

public class Director : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        APIClient.Initialize();
        PageManager.ChangeImmediate("MainPage");
    }

    // Update is called once per frame
    async void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            await APIClient.Instance.FirstRun_Diagnosis();
        }
        #endif
    }
}
