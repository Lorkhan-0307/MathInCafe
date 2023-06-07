using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;

public class APIClient
{
    private static APIClient instance;
    private static HttpClient httpClient;

    private readonly string apiURL = "https://prd-brs-relay-model.mathpid.com/api/v1/contest/diagnosis/setting";
    private readonly string XAPIKEY = "6_hNzYA9mvSiPheucoxAdX-2W1-0lCmCr0GvPbpstNc";
    private readonly string gamecode = "E12";

    private string userId;
    private string userToken;

    private APIClient()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", XAPIKEY);
    }

    public static APIClient Instance
    {
        get
        {
            if (instance == null)
            {
                throw new Exception("APIClient 인스턴스가 초기화되지 않았습니다.");
            }
            return instance;
        }
    }

    public static void Initialize()
    {
        if (instance == null)
        {
            instance = new APIClient();
        }
    }

    public async Task<string> GetResponseFromAPI()
    {
        /*var requestData = new Dictionary<string, object>
        {
            { "gameCd", gamecode },
            { "mbrId", userId },
            { "gameVer", "1.0" },
            { "osScnCd", GetOsScnCd() },
            { "deviceNm", GetDeviceName() },
            { "langCd", GetLanguageCode() },
            { "timeZone", GetTimeZone() }
        };*/
        
        var requestData = new Dictionary<string, string>
        {
            { "x-api-key", XAPIKEY }
        };


        var content = new FormUrlEncodedContent(requestData);
        
        //Debug.Log(jsonContent);
        
        //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        Debug.Log(content);

        HttpResponseMessage response = await httpClient.PostAsync(apiURL, content);
        response.EnsureSuccessStatusCode();

        // 'authorization' 값을 가져와 사용자 토큰으로 저장
        if (response.Headers.TryGetValues("authorization", out var tokenValues))
        {
            userToken = tokenValues.FirstOrDefault();
            PlayerPrefs.SetString("USERTOKEN", userToken);
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    public async Task<List<string>> GetItemsFromAPI()
    {
        string responseBody = await GetResponseFromAPI();
        List<string> items = JsonConvert.DeserializeObject<List<string>>(responseBody);
        return items;
    }

    public string GenerateUserID()
    {
        if (PlayerPrefs.HasKey("USERID")&&!String.IsNullOrEmpty(PlayerPrefs.GetString("USERID")))
        {
            Debug.LogWarning("APICLIENT :: UserAlreadyHas ID!");
            return userId;
        }
        userId = gamecode + GenerateUniqueNumber(17);
        PlayerPrefs.SetString("USERID", userId);
        Debug.Log($"APICLIENT :: USER ID GENERATED AS {userId}");
        return userId;
    }

    private string GenerateUniqueNumber(int length)
    {
        string numbers = "1234567890";
        char[] chars = new char[length];
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            chars[i] = numbers[random.Next(numbers.Length)];
        }

        return new string(chars);
    }
    
    private string GetOsScnCd()
    {
        string osScnCd = string.Empty;

        // 플랫폼에 따라 OS 구분을 얻는 로직을 구현
#if UNITY_EDITOR
        osScnCd = "Editor";
#elif UNITY_IOS
    osScnCd = "iOS";
#elif UNITY_ANDROID
    osScnCd = "Android";
#elif UNITY_WEBGL
    osScnCd = "WebGL";
#else
    osScnCd = "Unknown";
#endif

        return osScnCd;
    }

    private string GetDeviceName()
    {
        // 플랫폼에 따라 디바이스 이름을 얻는 로직을 구현
        string deviceName = SystemInfo.deviceName;
        return deviceName;
    }

    private string GetLanguageCode()
    {
        // 플랫폼에 따라 학습 언어 코드를 얻는 로직을 구현
        string languageCode = string.Empty;

#if UNITY_EDITOR
        languageCode = "KO"; // 테스트를 위해 임시로 "KO"로 설정
#elif UNITY_IOS || UNITY_ANDROID
    // 실제 플랫폼 API를 사용하여 학습 언어 코드를 얻는 로직을 구현
    // 예: languageCode = MobilePlatformAPI.GetLanguageCode();
#else
    languageCode = "EN"; // 기본적으로 영어로 설정
#endif

        return languageCode;
    }

    private int GetTimeZone()
    {
        // 플랫폼에 따라 시간대를 얻는 로직을 구현
        int timeZone = 0;

#if UNITY_EDITOR
        timeZone = 9; // 테스트를 위해 임시로 GMT+9로 설정
#elif UNITY_IOS || UNITY_ANDROID
    // 실제 플랫폼 API를 사용하여 시간대를 얻는 로직을 구현
    // 예: timeZone = MobilePlatformAPI.GetTimeZone();
#else
    timeZone = -2; // 기본적으로 GMT+0로 설정
#endif

        return timeZone;
    }
}
