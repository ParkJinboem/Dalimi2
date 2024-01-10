using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using I2.Loc;

[System.Serializable]
public class OptionData
{
    public bool sound;
    public bool bgm;
    public bool vibration;
    public bool korean;
}

public class OptionUI : MonoBehaviour
{
    private Dictionary<Sprite, bool> soundDictionary = new Dictionary<Sprite, bool>();
    private Dictionary<Sprite, bool> bgmDictionary = new Dictionary<Sprite, bool>();
    private Dictionary<Sprite, bool> vibrationDictionary = new Dictionary<Sprite, bool>();
    private Dictionary<Sprite, bool> languageDictionary = new Dictionary<Sprite, bool>();

    [SerializeField] private Transform optionRoot;
    [SerializeField] private Sprite[] soundBtnSprite;
    [SerializeField] private Sprite[] bgmBtnSprite;
    [SerializeField] private Sprite[] vibrationBtnSprite;
    [SerializeField] private Sprite[] languageBtnSprite;

    [SerializeField] private Button sound;
    [SerializeField] private Button bgm;
    [SerializeField] private Button vibration;
    [SerializeField] private Button language;
    private UserInfo userInfo;

    public void Init()
    {
        soundDictionary.Add(soundBtnSprite[0], true);
        soundDictionary.Add(soundBtnSprite[1], false);
        bgmDictionary.Add(bgmBtnSprite[0], true);
        bgmDictionary.Add(bgmBtnSprite[1], false);
        vibrationDictionary.Add(vibrationBtnSprite[0], true);
        vibrationDictionary.Add(vibrationBtnSprite[1], false);
        languageDictionary.Add(languageBtnSprite[0], true);
        languageDictionary.Add(languageBtnSprite[1], false);

        userInfo = PlayerDataManager.Instance.GetUserInfo();
        sound.GetComponent<Image>().sprite = soundDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.sound).Key;
        bgm.GetComponent<Image>().sprite = bgmDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.bgm).Key;
        vibration.GetComponent<Image>().sprite = vibrationDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.vibration).Key;
        language.GetComponent<Image>().sprite = languageDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.korean).Key;
    }

    public void ClickedOption(int number)
    {
        SoundManager.Instance.OnClickSoundEffect();

        Option selectedOption = (Option)number;
        switch (selectedOption)
        {
            case Option.Sound:
                if (userInfo.optionData.sound)
                {
                    userInfo.optionData.sound = false;
                }
                else if (!userInfo.optionData.sound)
                {
                    SoundManager.Instance.OnClickSoundEffect();
                    userInfo.optionData.sound = true;
                }
                SoundManager.Instance.EffectSoundOnOff();
                sound.GetComponent<Image>().sprite = soundDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.sound).Key;
                break;
            case Option.Bgm:
                if (userInfo.optionData.bgm)
                {
                    userInfo.optionData.bgm = false;
                }
                else if (!userInfo.optionData.bgm)
                {
                    userInfo.optionData.bgm = true;
                }
                SoundManager.Instance.BgmOnOff();
                bgm.GetComponent<Image>().sprite = bgmDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.bgm).Key;
                break;
            //진동 기능 삭제_230823 박진범
            //case Option.Vibration:
            //    if (userInfo.optionData.vibration)
            //    {
            //        userInfo.optionData.vibration = false;
            //    }
            //    else if (!userInfo.optionData.vibration)
            //    {
            //        userInfo.optionData.vibration = true;

            //        #if UNITY_ANDROID || UNITY_IOS
            //        Handheld.Vibrate();
            //        #endif
            //    }
            //    vibration.GetComponent<Image>().sprite = vibrationDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.vibration).Key;
            //    break;
        }
        PlayerDataManager.Instance.SaveData();
    }

    public void ClickedLanguage()
    {
        SoundManager.Instance.OnClickSoundEffect();
        if (userInfo.optionData.korean)
        {
            userInfo.optionData.korean = false;
            PlayerDataManager.Instance.language = LocalizationManager.GetAllLanguages()[0];
        }
        else if(!userInfo.optionData.korean)
        {
            userInfo.optionData.korean = true;
            PlayerDataManager.Instance.language = LocalizationManager.GetAllLanguages()[1];
        }
        language.GetComponent<Image>().sprite = languageDictionary.FirstOrDefault(x => x.Value == userInfo.optionData.korean).Key;
    }

    public void ExitBtn()
    {
        SoundManager.Instance.OnClickSoundEffect();
        Destroy(gameObject);
    }
}
