using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPage : PageHandler
{
    
    [SerializeField] private GameObject itemBoxContent;
    [SerializeField] private GameObject itemBox;

    [SerializeField] private GameObject[] selectedImages;
    [SerializeField] private Button quitButton;
    
    
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
                selectedImages[i].SetActive(true);
            }
            else selectedImages[i].SetActive(false);
        }
        
        foreach (ItemData data in ItemManager.itemDataList)
        {
            if (data.remain > 0 && data.itemType == itemType)
            {
                GameObject newItemBox = Instantiate(itemBox, itemBoxContent.transform);
                newItemBox.GetComponent<ItemBox>().Setup(data);
            }
        }
    }

    public void OnClickAll()
    {
        SimpleSound.Play("touch");
        DestroyChildren(itemBoxContent.transform);
        
        for (int i = 0; i < 5; i++)
        {
            if (i == 4)
            {
                selectedImages[i].SetActive(true);
            }
            else selectedImages[i].SetActive(false);

        }

        foreach (ItemData data in ItemManager.itemDataList)
        {
            if (data.remain > 0)
            {
                GameObject newItemBox = Instantiate(itemBox, itemBoxContent.transform);
                newItemBox.GetComponent<ItemBox>().Setup(data);
            }
        }
    }

    private void OnClickQuit()
    {
        SimpleSound.Play("touch");
        PageManager.ChangeImmediate(nameof(MainPage));
    }
    
    public void DestroyChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }
    
}