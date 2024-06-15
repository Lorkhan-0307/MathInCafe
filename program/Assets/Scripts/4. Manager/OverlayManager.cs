using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverlayManager : SingletonMonoBehaviour<OverlayManager>
{
    public TMP_Text gold;
    public TMP_Text heart;
    
    // Start is called before the first frame update
    void Start()
    {
        gold.text = ItemManager.gold.ToString();
        heart.text = ItemManager.heart.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
