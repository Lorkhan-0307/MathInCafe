using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemData
{
    public int itemID;
    public string itemName;
    
    public int itemType;
    // item type의 경우, 각 아이템이 원두, 머신, 스킨, 벽장식 중 하나인지를 나타낸다.
    // 원두 : 0
    // 머신 : 1
    // 스킨 : 2
    // 벽장식 : 3
    
    public bool isUpgradable;
    
    public int itemLevel;
    // 업그레이드 가능 아이템의 경우 현재 몇레벨인지 확인한다.
    public bool isGained;

    public string itemDescription;
    
    public int remain;
    public int requireLevel;
    public bool isConsumable;

    public Sprite itemSprite;
}
