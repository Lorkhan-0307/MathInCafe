using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;


[System.Serializable]
public class QuestData
{
    public int questID;
    public int requiredLevel;
    public string questName;
    public string questContent;
    public int goal;
    public bool isCompleted;
    public int progress;
    public int coinReward;
    public int heartReward;
}



