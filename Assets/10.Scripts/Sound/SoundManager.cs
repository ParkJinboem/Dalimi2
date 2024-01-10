using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    [SerializeField] AudioMixer bgmMixer;
    [SerializeField] AudioMixer sfxMixer;
    private AudioSource source;  
    public AudioSource bgmSource;
    public GameObject bgmPrefabs;

    public AudioClip btnSound;
    public AudioClip effectSound;
    float bgmTime;

    [SerializeField] List<AudioClip> effects;
    UserInfo userInfo;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
    }

    void Update()
    {
        bgmTime = bgmSource.time;
    }

    private void OnDestroy()
    {
        AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
    }

    void OnAudioConfigurationChanged(bool deviceWasChanged)
    {
        Debug.Log(deviceWasChanged ? "Device was changed" : "Reset was called");
        if (deviceWasChanged)
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.dspBufferSize = 64;
            AudioSettings.Reset(config);
        }
        bgmSource.time = bgmTime;
        bgmSource.Play();
    }

    public void Init()
    {
        source = GetComponent<AudioSource>();
        bgmSource = Instantiate(bgmPrefabs).GetComponent<AudioSource>();
        DontDestroyOnLoad(bgmSource);
        userInfo = PlayerDataManager.Instance.GetUserInfo();
        SoundInit();
    }

    public void SoundInit()
    {
        //배경음 초기설정
        if (userInfo.optionData.bgm)
        {
            bgmSource.mute = false;
        }
        else if (!userInfo.optionData.bgm)
        {
            bgmSource.mute = true;
        }
        //효과음 초기설정
        if (userInfo.optionData.sound)
        {
            source.mute = false;
            sfxMixer.SetFloat("sfx", 0);
        }
        else if (!userInfo.optionData.sound)
        {
            source.mute = true;
            sfxMixer.SetFloat("sfx", -80f);
        }
    }
            
    public void BgmOnOff()
    {
        if(userInfo.optionData.bgm)
        {
            bgmSource.mute = false;
        }
        else if(!userInfo.optionData.bgm)
        {
            bgmSource.mute = true;
        }
    }

    public void EffectSoundOnOff()
    {
        
        if (userInfo.optionData.sound)
        {
            source.mute = false;
            sfxMixer.SetFloat("sfx", 0);
        }
        else if (!userInfo.optionData.sound)
        {
            source.mute = true;
            sfxMixer.SetFloat("sfx", -80f);
        }
    }

    public void OnClickSoundEffect()
    {
        
        if (!userInfo.optionData.sound)
        {
            return;
        }
        else if (userInfo.optionData.sound)
        {
            source.PlayOneShot(btnSound);
        }
    }

    public void OnPlayOneShot(string effectName)
    {
        AudioClip audioClip = effects.Find(x => x.name == effectName);
        if (audioClip != null)
        {
            source.PlayOneShot(audioClip);
        }
    }
}
