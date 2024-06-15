using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public static class SoundHelper {
        static bool enableSFX;
        static bool enableBGM;
        static bool enableHaptic;
        static float volumeMaster;
        static float volumeSFX;
        static float volumeBGM;

        static SoundHelper() {
            enableSFX = (PlayerPrefs.GetInt("Sound.enableSFX", 1) == 1);
            enableBGM = (PlayerPrefs.GetInt("Sound.enableBGM", 1) == 1);
            enableHaptic = (PlayerPrefs.GetInt("Sound.enableHaptic", 1) == 1);
            volumeMaster = PlayerPrefs.GetFloat("Sound.volumeMaster", 1F);
            volumeSFX = PlayerPrefs.GetFloat("Sound.volumeSFX", 1F);
            volumeBGM = PlayerPrefs.GetFloat("Sound.volumeBGM", 1F);
            Sound.Instance.Initialize();
            //Vibration.Instance.Initialize();
            Sound.Instance.SetVolumeSFX(volumeSFX);
            Sound.Instance.SetVolumeBGM(volumeBGM);
            AudioListener.volume = volumeMaster;
        }

        public static bool EnableSFX {
            get => enableSFX;
            set {
                if (enableSFX == value) return;

                enableSFX = value;
                PlayerPrefs.SetInt("Sound.enableSFX", enableSFX ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public static bool EnableBGM {
            get => enableBGM;
            set {
                if (enableBGM == value) return;

                enableBGM = value;
                PlayerPrefs.SetInt("Sound.enableBGM", enableBGM ? 1 : 0);
                PlayerPrefs.Save();

                if (enableBGM == false) StopBGMAll();
            }
        }

        public static bool EnableHaptic {
            get => enableHaptic;
            set {
                if (enableHaptic == value) return;

                enableHaptic = value;
                PlayerPrefs.SetInt("Sound.enableHaptic", enableHaptic ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public static float VolumeMaster {
            get => volumeMaster;
            set {
                if (Math.Abs(volumeMaster - value) <= 0) return;

                volumeMaster = value;
                PlayerPrefs.SetFloat("Sound.volumeMaster", volumeMaster);
                PlayerPrefs.Save();
                AudioListener.volume = volumeMaster;
            }
        }

        public static float VolumeSFX {
            get => volumeSFX;
            set {
                if (Math.Abs(volumeSFX - value) <= 0) return;

                volumeSFX = value;
                PlayerPrefs.SetFloat("Sound.volumeSFX", volumeSFX);
                PlayerPrefs.Save();
                Sound.Instance.SetVolumeSFX(volumeSFX);
            }
        }

        public static float VolumeBGM {
            get => volumeBGM;
            set {
                if (Math.Abs(volumeBGM - value) <= 0) return;

                volumeBGM = value;
                PlayerPrefs.SetFloat("Sound.masterVolume", volumeBGM);
                PlayerPrefs.Save();
                Sound.Instance.SetVolumeBGM(volumeBGM);
            }
        }

        public static void PlayBGM(string clipName, int channel = 0, bool loop = true, bool useFade = true) {
            if (enableBGM) Sound.Instance.PlayBGM(clipName, VolumeBGM, channel, loop, useFade);
        }

        public static void StopBGM(int channel = 0, bool useFade = true, float fadeTime = 0.5f) {
            Sound.Instance.StopBGM(channel, useFade, fadeTime);
        }

        public static void StopBGMAll(bool useFade = true) {
            Sound.Instance.StopBGMAll(useFade);
        }

        public static void Play(string clipName, bool loop = false, float pitch = 1f) {
            if (enableSFX) Sound.Instance.Play(clipName, VolumeSFX, loop, pitch);
        }

        // volume을 파라미터로 넘길 수 없어서 함수 추가. overloading 함수의 파라미터가 헷갈릴 수 있으므로 정리가 필요함.
        public static void Play(string clipName, float volume, bool loop = false, float pitch = 1f) {
            if (enableSFX) Sound.Instance.Play(clipName, volume * VolumeSFX, loop, pitch);
        }

        /// <summary>
        /// 사운드 파일을 재생한다. 현재 재생중일 경우에는 다시 재생하지 않는다.
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="volume"></param>
        /// <param name="loop"></param>
        /// <param name="pitch"></param>
        public static void PlayOnce(string clipName, float volume = 1f, bool loop = false, float pitch = 1f) {
            if (enableSFX == false) return;
            if (IsPlaying(clipName)) return;
            Sound.Instance.Play(clipName, volume * VolumeSFX, loop, pitch);
        }

        public static void Stop(string clipName) {
            Sound.Instance.Stop(clipName);
        }

        public static void StopAll() {
            Sound.Instance.StopAll();
        }

        public static bool IsPlaying(string clipName) {
            return Sound.Instance.IsPlaying(clipName);
        }

        public static void SetMaxChannelForClip(string clipName, int maxChn) {
            Sound.Instance.SetMaxChannelForClip(clipName, maxChn);
        }

        // Todo...
        /// <summary>
        /// Vibrate need to check Vibration.Instance.IsHapticSupported
        /// </summary>
        /// <param name="androidMillisecs">vibrate time for android</param>
        /// <param name="iosVibrationType">haptic mode for ios</param>
        /*public static void Vibrate(long androidMillisecs, Vibration.IOSVibrationType iosVibrationType) {
        if (Vibration.Instance.IsHapticSupported && enableHaptic) {
			Vibration.Instance.Vibrate(androidMillisecs, iosVibrationType);
        }*/
    }

    public class Sound : ScriptableObject {
        const string ThemeSettingsAssetName = "SoundSettings";
        const string ThemeSettingsPath = "Resources";
        const string ThemeSettingsAssetExtension = ".asset";

        static Sound instance;

        [NonSerialized] public static Transform SoundParent;

        class ClipInfo {
            public int MaxCnt;
            public List<AudioSource> Sources = new List<AudioSource>();
            public AudioClip Clip;

            public ClipInfo(AudioClip clip, int max = 0) {
                Clip = clip;
                SetMaxChannel(max);
            }

            public bool IsPlaying {
                get
                {
                    return Sources.Any(t => t.volume > 0 && t.isPlaying);
                }
            }

            public void SetMaxChannel(int max) {
                MaxCnt = max;

                if (MaxCnt <= 0) return;
                for (int i = Sources.Count; i < max; ++i) Sources.Add(CreateAudioSource(Clip.name + "_" + Sources.Count));
            }

            public AudioSource GetPlayer(int idx = 0) {
                if (Sources.Count > idx) return Sources[idx];
                for (int i = Sources.Count; i < (idx + 1); ++i) Sources.Add(CreateAudioSource(Clip.name + "_" + Sources.Count));

                return Sources[idx];
            }

            public void SetVolume(float vol)
            {
                foreach (var t in Sources) t.volume = vol;
            }

            void PlayClip(AudioSource s, float vol, bool loop, float pitch = 1f) {
                s.clip = Clip;
                s.loop = loop;
                s.volume = vol;
                s.pitch = pitch;
                s.Play();
            }

            public bool Play(float volume, bool loop, float pitch = 1f) {
                AudioSource slot = Sources.FirstOrDefault(t => !t.isPlaying);

                if (slot == null) {
                    if (MaxCnt <= 0) {
                        slot = CreateAudioSource(Clip.name + "_" + Sources.Count);
                        Sources.Add(slot);
                    } 
                    else 
                    {
                        if (instance.warnOnSkip)
                            Debug.LogWarning("SoundHelper : ChannelCount(" + MaxCnt + ") for clip[" + Clip.name +
                                             "] exceeded. skipping..");

                        return false;
                    }
                }

                PlayClip(slot, volume, loop, pitch);
                return true;
            }

            public void Stop()
            {
                foreach (var t in Sources.Where(t => t.isPlaying))
                {
                    t.volume = 0;
                    t.Stop();
                }
            }

            public void SetPitch(float amount)
            {
                foreach (var t in Sources) t.pitch = amount;
            }

            AudioSource CreateAudioSource(string name = null) {
                GameObject soundPlayer = new GameObject(name);
                AudioSource audioSource = soundPlayer.AddComponent<AudioSource>() as AudioSource;
                audioSource.volume = 0;
                soundPlayer.transform.parent = Sound.SoundParent;
                return audioSource;
            }

        }

        [NonSerialized] Dictionary<string, ClipInfo> clipDic = new Dictionary<string, ClipInfo>();
        List<ClipInfo> clipsForBGM = new List<ClipInfo>();

        public AudioClip[] audioClips;
        public bool warnOnSkip = true;

        public static Sound Instance {
            get {
                if (instance != null) return instance;
                instance = Resources.Load(ThemeSettingsAssetName) as Sound;

                if (instance != null) return instance;
                instance = CreateInstance<Sound>();
#if UNITY_EDITOR
                string properPath = Path.Combine(Application.dataPath, ThemeSettingsPath);

                if (!Directory.Exists(properPath)) {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                string fullPath = Path.Combine(Path.Combine("Assets", ThemeSettingsPath),
                    ThemeSettingsAssetName + ThemeSettingsAssetExtension
                );
                AssetDatabase.CreateAsset(instance, fullPath);
#endif

                return instance;
            }
        }

        public float VolumeOfBGM {
            get => clipsForBGM[0].GetPlayer().volume;
            set
            {
                foreach (var t in clipsForBGM)
                {
                    t.GetPlayer().volume = value;
                }
            }
        }

#if UNITY_EDITOR
        [MenuItem("Utility/Sound Settings")]
        public static void Edit() {
            Selection.activeObject = Instance;
        }
#endif

        void OnEnable() {
            DontDestroyOnLoad(this);
        }

        public void Initialize() {
            if (audioClips == null) return;

            if (SoundParent == null) {
                SoundParent = new GameObject("+Sound").transform;
                DontDestroyOnLoad(SoundParent);
            }

            foreach (AudioClip audioClip in audioClips) {
                if (audioClip != null)
                    clipDic.Add(audioClip.name, new ClipInfo(audioClip));
            }
        }

        public void SetVolumeSFX(float volumeSFX)
        {
            foreach (var clp in clipDic.Values.Where(clp => clipsForBGM.IndexOf(clp) == -1)) clp.SetVolume(volumeSFX);
            
        }

        public void SetVolumeBGM(float volumeBGM)
        {
            foreach (var t in clipsForBGM)
            {
                if (t == null) continue;

                t.GetPlayer().volume = volumeBGM;
            }
        }

        public void PlayBGM(string clipName, float volume = 1F, int channel = 0, bool loop = true,
            bool useFade = true) {
            //Todo
            //if (NativeInterface.IsMusicPlaying()) return;
        
        
            while (clipsForBGM.Count <= channel)
                clipsForBGM.Add(null);

            ClipInfo prevClip = clipsForBGM[channel];
            ClipInfo curClip = FindClipInfoByName(clipName);

            if (curClip == null) {
                Debug.LogError("Sound.PlayBGM fail : clip not found " + clipName);
                return;
            }

            clipsForBGM[channel] = curClip;
            DOTween.Complete("BGM_Fade_" + channel);

            if (useFade) {
                Sequence seq = DOTween.Sequence().SetId("BGM_Fade_" + channel);

                if (prevClip != null && prevClip.IsPlaying) {
                    seq.Append(prevClip.GetPlayer().DOFade(0F, 0.5F));
                    seq.AppendInterval(0.3F);
                }

                seq.AppendCallback(() => { curClip.Play(volume, loop); });
                seq.Append(curClip.GetPlayer().DOFade(volume, 0.5F));

                if (prevClip != null && prevClip.IsPlaying) {
                    seq.AppendCallback(() => { prevClip.Stop(); });
                }

                seq.Play();
            } else {
                prevClip?.Stop();

                curClip.Play(volume, loop);
            }
        }

        public void StopBGM(int channel = 0, bool useFade = true, float fadeTime = 0.5f) {
            if (clipsForBGM.Count <= channel) return;

            ClipInfo clip = clipsForBGM[channel];
            Debug.Assert(clip != null);

            if (clip.IsPlaying == false) return;

            DOTween.Complete("BGM_Fade_" + channel);

            if (useFade) {
                Sequence seq = DOTween.Sequence().SetId("BGM_Fade_" + channel);
                seq.Append(clip.GetPlayer().DOFade(0F, fadeTime));
                seq.AppendCallback(() => { clip.Stop(); });
                seq.Play();
            } else {
                clip.Stop();
            }
        }

        public void StopBGMAll(bool useFade = true) {
            for (int i = 0; i < clipsForBGM.Count; i++) {
                StopBGM(i, useFade);
            }
        }

        public void Play(string clipName, float volume = 1F, bool loop = false, float pitch = 1f) {
            var clip = FindClipInfoByName(clipName);

            clip?.Play(volume, loop, pitch);
        }

        public void Stop(string clipName) {
            var clip = FindClipInfoByName(clipName);

            clip?.Stop();
        }

        public void StopAll() {
            foreach (var e in clipDic)
                e.Value.Stop();
        }

        public bool IsPlayingBGM(int channel = 0) {
            ClipInfo clip = clipsForBGM[channel];

            if (clip == null) return false;

            return clip.IsPlaying;
        }

        public bool IsPlaying(string clipName) {
            ClipInfo clip = FindClipInfoByName(clipName);

            if (clip == null) return false;

            return clip.IsPlaying;
        }

        public void SetMaxChannelForClip(string clipName, int maxChn) {
            ClipInfo clip = FindClipInfoByName(clipName);

            clip?.SetMaxChannel(maxChn);
        }

        public void SetPitch(string clipName, float amount, bool useFade = true) {
            ClipInfo clip = FindClipInfoByName(clipName);

            if (clip == null) return;

            DOTween.Complete("SetPitch_" + clipName);

            if (useFade) {
                Sequence seq = DOTween.Sequence().SetId("SetPitch_" + clipName);
                seq.Append(clip.GetPlayer().DOPitch(amount, 0.5F));
                seq.Play();
            } else {
                clip.SetPitch(amount);
            }
        }

        ClipInfo FindClipInfoByName(string clipName) {
            if (clipDic.ContainsKey(clipName))
                return clipDic[clipName];

            Debug.Assert(false, "Can't found audio clip - " + clipName);
            return null;
        }
    }
}