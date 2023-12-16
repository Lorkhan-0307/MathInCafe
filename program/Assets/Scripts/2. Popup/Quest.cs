using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Quest : MonoBehaviour
{
    [SerializeField] private Button acceptButton;
    [SerializeField] private GameObject receivedImage;
    
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text heartText;

    public void Setup(QuestData data)
    {
        questNameText.text = LocalizationManager.Instance.Translate(data.questName, "Button_Trans");
        questDescriptionText.text = LocalizationManager.Instance.Translate(data.questContent, "Button_Trans");
        Debug.Log(data.questName);
        coinText.text = data.coinReward.ToString();
        
        if(data.heartReward != 0)
            heartText.text = data.heartReward.ToString();

        if (data.isCompleted == true)
        {
            acceptButton.gameObject.SetActive(false);
            receivedImage.SetActive(true);
        }
        else
        {
            acceptButton.gameObject.SetActive(true);
            receivedImage.SetActive(false);
        }

        if (data.goal > 1)
        {
            questDescriptionText.text += $" {data.progress} / {data.goal}";
        }
        
    }
    
}
