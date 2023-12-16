using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item Data")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> inventoryItems = new List<ItemData>();
}