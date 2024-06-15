using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;


[CreateAssetMenu(fileName = "QuestDatabase", menuName = "Quest Data")]
public class QuestDatabase : ScriptableObject
{
    public List<QuestData> quests = new List<QuestData>();
}