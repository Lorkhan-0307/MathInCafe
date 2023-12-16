using System;
using UnityEngine;
<<<<<<< HEAD
=======
using Progress = UnityEditor.Progress;
using String = System.String;
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac

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

<<<<<<< HEAD
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ItemManager.AddGold(99999);
=======

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ItemManager.Instance.AddItem(0, 1);
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
        }
    }
    
    
    

    
}
