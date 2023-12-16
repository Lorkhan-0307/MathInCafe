using System;
using Cysharp.Threading.Tasks;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;
using TMPro;
using UnityEngine.Serialization;


public class ShopPage : PageHandler
{
    
    [SerializeField] private GameObject itemBoxContent;
    [SerializeField] private GameObject itemBox;

    [SerializeField] private GameObject[] tabButton;
    [SerializeField] private Sprite[] buttonSprite;
    
    [SerializeField] private Button quitButton;
    
    
    public override void OnWillEnter(object param)
    {
        base.OnWillEnter(param);
        quitButton.onClick.AddListener(OnClickQuit);

    }
    
    public override void OnDidEnter(object param)
    {
        Setup();
    }
    
    private void Setup()
    {
        OnClickItems(0);
        quitButton.onClick.AddListener(OnClickQuit);
    }
    
    public void OnClickItems(int itemType)
    {
        SimpleSound.Play("touch");
        DestroyChildren(itemBoxContent.transform);

        for (int i = 0; i < 5; i++)
        {
            if (i == itemType)
            {
                tabButton[i].GetComponent<Button>().image.sprite = buttonSprite[1];
                Vector2 newSizeDelta = tabButton[i].GetComponent<RectTransform>().sizeDelta;
                newSizeDelta.y = 120;
                tabButton[i].GetComponent<RectTransform>().sizeDelta = newSizeDelta;
            }
            else
            {
                tabButton[i].GetComponent<Button>().image.sprite = buttonSprite[0];
                Vector2 newSizeDelta = tabButton[i].GetComponent<RectTransform>().sizeDelta;
                newSizeDelta.y = 100;
                tabButton[i].GetComponent<RectTransform>().sizeDelta = newSizeDelta;
            }
        }
        
        foreach (ItemData data in ItemManager.itemDataList)
        {
            if (data.itemType == itemType)
            {
                GameObject newItemBox = Instantiate(itemBox, itemBoxContent.transform);
                newItemBox.GetComponent<ItemBox>().Setup(data);
            }
        }
    }

    public void DestroyChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }

    private void OnClickQuit()
    {
        SimpleSound.Play("touch");
        PageManager.ChangeImmediate(nameof(MainPage));
    }
    
}
