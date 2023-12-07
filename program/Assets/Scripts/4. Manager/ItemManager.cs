using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;


public class ItemManager : Singleton<ItemManager>
{
    public ItemDatabase itemDatabase;

    public static List<ItemData> itemDataList = new List<ItemData>();

    public void ItemDataInitialize()
    {
        // ItemDatabase 객체를 Resources 폴더에서 불러옵니다.
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");

        itemDataList = new List<ItemData>();

        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase not found! Make sure it exists in the Resources folder.");
            return;
        }

        for (int i = 0; i < itemDatabase.inventoryItems.Count; i++)
        {
            if (PlayerPrefs.HasKey($"ItemData_{itemDatabase.inventoryItems[i].itemID}"))
            {
                // 이전 아이템 기록이 존재함
                itemDatabase.inventoryItems[i] = LoadItemData(itemDatabase.inventoryItems[i].itemID);
                Debug.Log($"item is here! {itemDatabase.inventoryItems[i].itemID} {itemDatabase.inventoryItems[i].remain}");

            }
            else
            {
                // 이전 아이템 기록이 없음
                SaveItemData(itemDatabase.inventoryItems[i]);
            }
            itemDataList.Add(itemDatabase.inventoryItems[i]);
        }
        
    }

    public void SaveItemData(ItemData itemData)
    {
        string json = JsonUtility.ToJson(itemData);
        PlayerPrefs.SetString($"ItemData_{itemData.itemID}", json);
        PlayerPrefs.Save();
    }
    
    public ItemData LoadItemData(int itemID)
    {
        string json = PlayerPrefs.GetString($"ItemData_{itemID}", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<ItemData>(json);
        }
        return null;
    }

    public void AddItem(int itemID, int count)
    {
        foreach (ItemData data in itemDataList)
        {
            if (itemID == data.itemID)
            {
                data.remain += count;
            }
            SaveItemData(data);
        }
    }

    
}
