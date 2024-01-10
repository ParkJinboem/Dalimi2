using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

public partial class PlayerDataManager : MonoBehaviour
{
    #region SaveCharacter
    public SaveCharacter GetSaveCharacterWithId(int savecharacterId)
    {
        SaveCharacter savecharacter = sl.saveCharacters.Find(x => x.saveId == savecharacterId);
        return savecharacter;
    }

    public void SaveCharacterData(SaveCharacter saveCharacter)
    {
        int index = sl.saveCharacters.FindIndex(x => x.saveId == saveCharacter.saveId);
        if (index == -1)
        {
            sl.saveCharacters.Add(saveCharacter);
        }
        else
        {
            sl.saveCharacters[index] = saveCharacter;
        }

        SaveData();
    }

    public void DeleteCharacterData(int saveCharacterId)
    {
        SaveCharacter saveCharacter = sl.saveCharacters.Find(x => x.saveId == saveCharacterId);
        sl.saveCharacters.Remove(saveCharacter);
        for (int i = 0; i < sl.saveCharacters.Count; i++)
        {
            sl.saveCharacters[i].saveId = i + 1;
        }
        SaveData();
    }

    public bool IsHaveSaveCharacter(int saveCharacterId)
    {
        SaveCharacter saveCharacter = GetSaveCharacterWithId(saveCharacterId);
        if (saveCharacter == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddSaveCharacter(SaveCharacter savecharacterData)
    {
        SaveCharacterData(savecharacterData);
    }

    public List<SaveCharacter> GetSaveCharacters()
    {
        return sl.saveCharacters;
    }

    public List<SaveCharacter> AlbumSaveCharacters()
    {
        List<SaveCharacter> alubmSaveCharacters = new List<SaveCharacter>();
        List<SaveCharacter> saveCharacters = GetSaveCharacters();
        foreach (var item in saveCharacters)
        {
            if (item.trueSave)
            {
                alubmSaveCharacters.Add(item);
            }
        }
        return alubmSaveCharacters;
    }
    #endregion SaveCharacter

    #region UserInfo
    public UserInfo GetUserInfo()
    {
        return s.userinfo;
    }

    public void SetUserInfoGameStep(GameStep gameStep)
    {
        s.userinfo.playingStep = gameStep;
    }
    #endregion UserInfo

    #region Attendance
    public void DailyCheck()
    {
        if (string.IsNullOrEmpty(s.userinfo.attendanceData.date))
        {
            s.userinfo.attendanceData.date = Scheduler.GetNowToString();
        }

        Scheduler.S schedulerS = new Scheduler.S(Scheduler.SchedulerType.Daily, s.userinfo.attendanceData.date);
        if (schedulerS.IsAfter)
        {
            // 일일 리셋
            ResetDailyAttendance();
            SaveData();
        }
    }

    public void Attendance()
    {
        if (s.userinfo.attendanceData.isGet)
        {
            return;
        }

        s.userinfo.attendanceData.isGet = true;
    }

    public void ResetDailyAttendance()
    {
        if (s.userinfo.attendanceData.isGet)
        {
            int nextDay = s.userinfo.attendanceData.day + 1;
            s.userinfo.attendanceData.day = nextDay;
        }
        s.userinfo.attendanceData.date = Scheduler.GetNowToString();
        s.userinfo.attendanceData.isGet = false;
    }

    public int GetRewardDailyItemCount()
    {
        List<ClosetData> rewardCloset = sl.rewardCloset;
        List<ClosetData> dailyRewardCloset = new List<ClosetData>();
        foreach (var item in rewardCloset)
        {
            if(item.type == ClosetType.Daily)
            {
                dailyRewardCloset.Add(item);
            }
        }
        return dailyRewardCloset.Count;
    }

    public int AttendanceDay { get { return s.userinfo.attendanceData.day; } }
    #endregion Attendance

    #region Reward
    //출석체크 보상 및 광고의상 유저데이터에 저장
    public void AddRewardCloset(ClosetData closetData)
    {
        sl.rewardCloset.Add(closetData);
    }

    public void AddRewardBackGround(BackgroundData backgroundData)
    {
        sl.rewardBackground.Add(backgroundData);
    }
    #endregion Reward


    public string language
    {
        set { s.lanuage = value; SaveData(); LocalizationManager.CurrentLanguage = value; }
        get { return s.lanuage; }
    }
}
