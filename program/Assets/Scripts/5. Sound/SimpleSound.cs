using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "New SimpleSound", menuName = "ScriptableObject/SimpleSound")]
public class SimpleSound : ScriptableObject {
    public List<AudioClip> bgm;
    public List<AudioClip> sfx;
    Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    Dictionary<string, float> lastPlayTime = new Dictionary<string, float>();
    static SimpleSound instance;
    public static SimpleSound Instance {
        get {
            if (instance == null)
                Initialize();
            return instance;
        }
    }

    static GameObject BGMSource;
    static GameObject SFXSource;
    static List<AudioSource> bgmAudios { get; } = new List<AudioSource>();
    static List<AudioSource> sfxAudios { get; } = new List<AudioSource>();

    static void Initialize() {
        instance = Resources.Load(nameof(SimpleSound)) as SimpleSound;
        Debug.Log($"path is {nameof(SimpleSound)}");
        BGMSource = new GameObject("BGM Audio");
        SFXSource = new GameObject("SFX_Audio");
        foreach (var bgm in instance.bgm)
            instance.bgmDict[bgm.name] = bgm;
        foreach (var sfx in instance.sfx)
            instance.sfxDict[sfx.name] = sfx;
        foreach (var pair in instance.bgmDict)
            instance.lastPlayTime[pair.Key] = 0f;
        foreach (var pair in instance.sfxDict)
            instance.lastPlayTime[pair.Key] = 0f;

        DontDestroyOnLoad(BGMSource);
        DontDestroyOnLoad(SFXSource);
    }

    public static void Play(string name, bool loop = false, float min_interval = 0.02f, float volume = 1f)    //파일이름 / 반복여부 / 최소 재생 간격
    {
        if (Instance.sfxDict.TryGetValue(name, out var sfx)) {
            if (!Utility.SoundHelper.EnableSFX) return;   //세팅의    sfx가 꺼져있거나
            //if (Time.time - Instance.lastPlayTime[name] < min_interval) return;    //동일한 효과음이 주어진 최소 재생 간격 이내에 재생된 적이 있다면, 리턴

            var audio = sfxAudios.Find(a => !a.isPlaying);  //플레이 중이지 않은 오디오를 찾는다.
            if (audio == null) {    //남는 오디오가 없다면, 새로 추가
                audio = SFXSource.AddComponent<AudioSource>();
                sfxAudios.Add(audio);
            }
            Instance.lastPlayTime[name] = Time.time;    //마지막 재생 시간을 저장한다.
            audio.clip = sfx;
            audio.loop = loop;
            audio.volume = volume;
            audio.Play();
        }
        else {
            Debug.LogError($"해당 사운드를 찾을 수 없음: {name}");
        }
    }
    public static void Stop(string name) {
        if (Instance.bgmDict.TryGetValue(name, out var bgm)) {
            var audio = bgmAudios.Find(a => a.clip == bgm);
            if (audio != null)
                audio.Stop();
            else
                Debug.LogError($"해당 사운드가 재생 중이 아님: {name}");
        }
        else if (Instance.sfxDict.TryGetValue(name, out var sfx)) {
            var audio = sfxAudios.Find(a => a.clip == sfx);
            if (audio != null)
                audio.Stop();
            else
                Debug.LogError($"해당 사운드가 재생 중이 아님: {name}");
        }
        else {
            Debug.LogError($"해당 사운드를 찾을 수 없음: {name}");
        }
    }

    public static void PlayBGM(string name, bool loop = true, float volume = 1f) {
        if (Instance.bgmDict.TryGetValue(name, out var bgm)) {
            if (!SoundHelper.EnableBGM) return;
            foreach (var a in bgmAudios)    //이미 같은 bgm이 재생중이라면, 리턴
                if (a.clip == bgm && a.isPlaying) return;

            var audio = bgmAudios.Find(a => !a.isPlaying);  //플레이 중이지 않은 오디오를 찾는다.
            if (audio == null) {     //남는 오디오가 없다면, 새로 추가
                audio = BGMSource.AddComponent<AudioSource>();
                bgmAudios.Add(audio);
            }

            audio.clip = bgm;
            audio.loop = loop;
            audio.volume = volume;
            audio.Play();
        }
        else {
            Debug.LogError($"해당 사운드를 찾을 수 없음: {name}");
        }
    }

    public static void StopBGM(float fadeSec = 0f) {
        fadeSec = Mathf.Abs(fadeSec);
        foreach (var a in bgmAudios) {
            DOTween.Kill(a.GetInstanceID());
            if (a.isPlaying) {
                if (fadeSec > Mathf.Epsilon)
                    a.DOFade(0f, fadeSec).SetId(a.GetInstanceID()).OnComplete(a.Stop);
                else
                    a.Stop();
            }
        }
    }

    public static AudioClip GetClip(string name) {
        if (Instance.sfxDict.TryGetValue(name, out var sfx))
            return sfx;

        Debug.Log($"해당 클립을 찾을 수 없음: {name}");
        return null;
    }

    public static IEnumerator QueueBGM(string name1, string name2)
    {
        SimpleSound.PlayBGM(name1, loop: false);
        yield return new WaitForSecondsRealtime(SimpleSound.Instance.bgmDict[name1].length);
        SimpleSound.PlayBGM(name2);
    }
}
