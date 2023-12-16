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

<<<<<<< HEAD
    public static int gold;
    public static int heart;

    public void ItemDataInitialize()
    {
        //gold와 heart를 설정한다.
        if (PlayerPrefs.HasKey("gold"))
        {
            gold = PlayerPrefs.GetInt("gold");
        }
        else
        {
            PlayerPrefs.SetInt("gold", 0);
            gold = PlayerPrefs.GetInt("gold");
        }
        
        if (PlayerPrefs.HasKey("heart"))
        {
            heart = PlayerPrefs.GetInt("heart");
        }
        else
        {
            PlayerPrefs.SetInt("heart", 0);
            heart = PlayerPrefs.GetInt("heart");
        }

        
        
=======
    public void ItemDataInitialize()
    {
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
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
<<<<<<< HEAD
=======
                Debug.Log($"item is here! {itemDatabase.inventoryItems[i].itemID} {itemDatabase.inventoryItems[i].remain}");
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac

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

<<<<<<< HEAD
    public static void AddGold(int count = 0)
    {
        gold += count;
        if (gold < 0) gold = 0;
        PlayerPrefs.SetInt("gold", gold);
    }

    public static void AddHeart(int count = 0)
    {
        heart += count;
        if (heart < 0) heart = 0;
        PlayerPrefs.SetInt("heart", heart);
    }

=======
    
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
}
