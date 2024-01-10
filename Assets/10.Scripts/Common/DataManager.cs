using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public partial class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    [Header("Datas")]
    public TextAsset backgroundData;
    public TextAsset dailyBonusData;
    public TextAsset closetPresetData;
    public TextAsset closetData;

    [Header("ClosetInfo")]
    public List<ClosetPresetData> closetPresetInfoData = new List<ClosetPresetData>();
    public List<ClosetData> closetInfoDatas = new List<ClosetData>();

    [Header("BackGroundInfo")]
    public List<BackgroundData> backgroundInfoDatas = new List<BackgroundData>();

    [Header("AttendanceData")]
    public List<AttendanceData> attendanceInfoDatas = new List<AttendanceData>();

    [Header("SpriteAtlas")]
    public SpriteAtlas characterPartAtlas;
    public SpriteAtlas characterPartUIAtlas;
    public SpriteAtlas BackGroundAtlas;
    public SpriteAtlas BackGroundUIAtlas;
    public SpriteAtlas menuHolder;

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

    public void Init()
    {
        InitClosetPresetData();
        InitClosetData();
        InitBackGroundData();
        InitDailyBonusData();
    }

    #region Atlas
    public Sprite GetCharacterPartSprite(string spriteName)
    {
        return characterPartAtlas.GetSprite(spriteName);
    }

    public Sprite GetCharacterPartUISprite(string spriteName)
    {
        return characterPartUIAtlas.GetSprite(spriteName);
    }

    public Sprite GetBackGroundSprite(string spriteName)
    {
        return BackGroundAtlas.GetSprite(spriteName);
    }

    public Sprite GetBackGroundUISprite(string spriteName)
    {
        return BackGroundUIAtlas.GetSprite(spriteName);
    }

    public Sprite GetMenuHolderSprite(string spriteName)
    {
        return menuHolder.GetSprite(spriteName);
    }
    #endregion Atlas

    public List<ClosetPresetData> GetClosetPresetDataWithId(int id)
    {
        return closetPresetInfoData.FindAll(x => x.id == id);
    }

    public List<ClosetData> GetClosetDataWithKind(ClosetKind closetKind)
    {
        return closetInfoDatas.FindAll(x => x.kind == closetKind);
    }

    //캐릭터 전용파트 사용할 경우 사용
    //public List<ClosetData> GetClosetDataWithCharacterName(ClosetKind closetKind, string characterName)
    //{
    //    return closetInfoDatas.FindAll(x => x.kind == closetKind).FindAll(x => x.characterName == characterName);
    //}

    public ClosetData GetClosetDataWithId(int closetId)
    {
        return closetInfoDatas.Find(x => x.id == closetId);
    }

    public ClosetData GetClosetDataWithName(string closetName)
    {
        return closetInfoDatas.Find(x => x.name == closetName);
    }

    public int GetItemIdWithName(string closetName)
    {
        return closetInfoDatas.Find(x => x.name == closetName).id;
    }

    public List<BackgroundData> GetBackGroundDataWithKind(BackgroundKind backgroundKind)
    {
        return backgroundInfoDatas.FindAll(x => x.kind == backgroundKind);
    }

    public BackgroundData GetBackGroundDataWithId(int backgroundId)
    {
        return backgroundInfoDatas.Find(x => x.id == backgroundId);
    }

    public BackgroundData GetBackGroundDataWithName(string backgroundName)
    {
        return backgroundInfoDatas.Find(x => x.name == backgroundName);
    }
}
