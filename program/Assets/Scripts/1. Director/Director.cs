using System;
using UnityEngine;

public class Director : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("LocalizationType"))
        {
            PlayerPrefs.DeleteAll();

            SystemLanguage systemLanguage = Application.systemLanguage;
            if (systemLanguage == SystemLanguage.Korean)
            {
                PlayerPrefs.SetInt("LocalizationType", 1);
            }
            else
            {
                PlayerPrefs.SetInt("LocalizationType", 0);
            }
        }
        
        APIClient.Initialize();
        QuestManager.Instance.QuestDataInitialize();
        ItemManager.Instance.ItemDataInitialize();
        PageManager.ChangeImmediate("MainPage");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ItemManager.AddGold(99999);
        }
    }
}
