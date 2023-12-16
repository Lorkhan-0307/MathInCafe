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
<<<<<<< HEAD
        SimpleSound.Play("touch");
=======
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
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
<<<<<<< HEAD

=======
    
    public void OnClickAll()
    {
        DestroyChildren(itemBoxContent.transform);
        
        for (int i = 0; i < 5; i++)
        {
            if (i == 4)
            {
                tabButton[i].SetActive(true);
            }
            else tabButton[i].SetActive(false);

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
    
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
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
<<<<<<< HEAD
        SimpleSound.Play("touch");
=======
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
        PageManager.ChangeImmediate(nameof(MainPage));
    }
    
}
