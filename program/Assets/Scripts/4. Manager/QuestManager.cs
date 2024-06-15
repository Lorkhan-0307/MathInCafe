using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;


public class QuestManager : Singleton<QuestManager>
{
    public QuestDatabase questDatabase;

    public static List<QuestData> dailyQuestList = new List<QuestData>();
    public static List<QuestData> currentLevelQuestList = new List<QuestData>();

    public void QuestDataInitialize()
    {
        // QuestDatabase 객체를 Resources 폴더에서 불러옵니다.
        questDatabase = Resources.Load<QuestDatabase>("QuestDatabase");

        dailyQuestList = new List<QuestData>();
        currentLevelQuestList = new List<QuestData>();

        if (questDatabase == null)
        {
            Debug.LogError("QuestDatabase not found! Make sure it exists in the Resources folder.");
            return;
        }

        for (int i = 0; i < questDatabase.quests.Count; i++)
        {
            if (questDatabase.quests[i].requiredLevel == 0)
            {
                dailyQuestList.Add(questDatabase.quests[i]);
            }

            else if (PlayerPrefs.HasKey("QuestLevel"))
            {
                if (questDatabase.quests[i].requiredLevel == PlayerPrefs.GetInt("QuestLevel"))
                {
                    currentLevelQuestList.Add(questDatabase.quests[i]);
                }
            }

            else
            {
                PlayerPrefs.SetInt("QuestLevel", 1);
                if (questDatabase.quests[i].requiredLevel == PlayerPrefs.GetInt("QuestLevel"))
                {
                    currentLevelQuestList.Add(questDatabase.quests[i]);
                }
            }

            if (PlayerPrefs.HasKey($"QuestData_{questDatabase.quests[i].questID}"))
            {
                // 이전 퀘스트 기록이 존재함
                questDatabase.quests[i] = LoadQuestData(questDatabase.quests[i].questID);
            }
            else
            {
                // 이전 퀘스트 기록이 없음
                SaveQuestData(questDatabase.quests[i]);
            }
        }
        
        Debug.Log($"Building {QuestManager.dailyQuestList.Count}");
        Debug.Log($"Building {QuestManager.currentLevelQuestList.Count}");

    }

    public void SaveQuestData(QuestData questData)
    {
        string json = JsonUtility.ToJson(questData);
        PlayerPrefs.SetString($"QuestData_{questData.questID}", json);
        PlayerPrefs.Save();
    }
    
    public QuestData LoadQuestData(int questID)
    {
        string json = PlayerPrefs.GetString($"QuestData_{questID}", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<QuestData>(json);
        }
        return null;
    }

    public void CheckDailyQuest()
    {
        DateTime currentDate = DateTime.Now;
        DateTime lastResetDate = default;
        
        
        string lastResetDateStr = PlayerPrefs.GetString("LastDailyQuestData");

        if (!string.IsNullOrEmpty(lastResetDateStr))
        {
            lastResetDate = DateTime.Parse(lastResetDateStr);
        }
        
        if (lastResetDate == default(DateTime) || lastResetDate.Date < currentDate.Date)
        {
            // 새로운 날이 시작되었으므로 Daily Quest 초기화
            ResetDailyQuests();
            lastResetDate = currentDate;
            PlayerPrefs.SetString("LastDailyQuestData", lastResetDate.ToString());
            PlayerPrefs.Save();
        }

    }

    private void ResetDailyQuests()
    {
        foreach (QuestData quest in dailyQuestList)
        {
            quest.progress = 0;
            quest.isCompleted = false;
            SaveQuestData(quest);
        }
    }
    
}
