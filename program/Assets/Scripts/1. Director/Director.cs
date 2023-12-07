using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Progress = UnityEditor.Progress;
using String = System.String;

public class Director : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        APIClient.Initialize();
        QuestManager.Instance.QuestDataInitialize();
        ItemManager.Instance.ItemDataInitialize();
        PageManager.ChangeImmediate("MainPage");
        

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ItemManager.Instance.AddItem(0, 1);
        }
    }
    
    
    

    
}
