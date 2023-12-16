using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationImage : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image Image;
    
    // Start is called before the first frame update
    void Start()
    {
        Image.sprite = _sprites[PlayerPrefs.GetInt("LocalizationType", 0)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
