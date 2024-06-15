using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class LocalizationText : MonoBehaviour
{
    private TMP_Text textComponent;
    private string key;


    private void Start()
    {
        Translate();
    }

    public void Translate()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Button_Trans");
        if (jsonFile == null)
        {
            Debug.LogError("Button_Trans.json not found!");
            return;
        }

        textComponent = GetComponent<TMP_Text>();
        key = textComponent.text;
        JObject translations = JObject.Parse(jsonFile.text);

        JProperty property = translations.Property(key);
        if (property != null)
        {
            JArray values = (JArray)property.Value;

            string localizedText = values[PlayerPrefs.GetInt("LocalizationType")].ToString();
            
            textComponent.text = localizedText;
        }
        else
        {
            Debug.LogWarning("Translation not found for key: " + key);
        }
    }
    
    string ExtractKey(string input)
    {
        // 정규식 패턴을 사용하여 숫자를 제외한 나머지 부분을 추출
        string pattern = @"(\d*\D+)"; // 숫자와 그 뒤에 오는 모든 문자에 대응합니다.
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            string key = match.Groups[1].Value;
            return key;
        }
        else
        {
            Debug.LogError("Failed to extract key from input: " + input);
            return "";
        }
    }
    
    string ExtractNumberFromKey(string input)
    {
        string pattern = @"\d+"; // \d는 숫자에 대응합니다.
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            return match.Value;
        }
        return "";
    }

    public void TranslateSelf()
    {
        Translate();
    }
}