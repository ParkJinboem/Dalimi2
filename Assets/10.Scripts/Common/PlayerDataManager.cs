using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public partial class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance = null;

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

    [Serializable]
    public class S
    {
        public UserInfo userinfo;
        public string lanuage;
    }
    
    [Serializable]
    public class SL
    {
        public List<ClosetData> rewardCloset;
        public List<BackgroundData> rewardBackground;
        public List<SaveCharacter> saveCharacters;
    }

    public S s; // 자료형
    public SL sl; // 리스트

    public void Init()
    {
        LoadData();
        SaveData();
        DailyCheck();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("s", JsonConvert.SerializeObject(s));
        PlayerPrefs.SetString("sl", JsonConvert.SerializeObject(sl));
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("s"))
        {
            s = JsonConvert.DeserializeObject<S>(PlayerPrefs.GetString("s"));
            sl = JsonConvert.DeserializeObject<SL>(PlayerPrefs.GetString("sl"));
        }

        // S 초기화
        if (s.userinfo.clearCharacterCount == 0)
        {
            s.userinfo = new UserInfo
            {
                //최종 캐릭터 27, 최초 설정 clearCharacterCount는 1로 수정
                clearCharacterCount = 1,
                playingCharacter = 0,
                playingSaveId = 0,
                playingStage = GameStage.Wash,
                playingStep = GameStep.Soap,
                clearStep = GameStep.Soap,
                backGroundName = "",
                buyAdRemove = false
            };
        }
        if (s.userinfo.attendanceData == null) s.userinfo.attendanceData = new AttendanceData();
        if (s.userinfo.optionData == null)
        {
            s.userinfo.optionData = new OptionData
            {
                sound = true,
                bgm = true,
                //vibration = false, //진동 기능 삭제_230823 박진범
                korean = true
            };
        }
        if (string.IsNullOrEmpty(s.lanuage)) s.lanuage = I2.Loc.LocalizationManager.GetAllLanguages()[1];
        I2.Loc.LocalizationManager.CurrentLanguage = s.lanuage;

        // SL 초기화
        if (sl.saveCharacters == null) sl.saveCharacters = new List<SaveCharacter>();
        if (sl.rewardCloset == null) sl.rewardCloset = new List<ClosetData>();
        if (sl.rewardBackground == null) sl.rewardBackground = new List<BackgroundData>();
    }
}
