using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScroll : MonoBehaviour
{
    [SerializeField] private ItemSpawner itemSpawner;
    private List<ItemConverter> itemConverters;
    public List<ItemConverter> ItemeConverters
    {
        get { return itemConverters; }
        set { itemConverters = value; }
    }
    private List<ClosetData> closetInfoDatas;
    private List<BackgroundData> backGroundInfoDatas;

    [SerializeField] private GameObject ReturnParent;
    private List<GameObject> returnObjects;

    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject itemPrefabs;

    public Animator itemScrollAnim;
    string playingCharacter;

    private GridLayoutGroup itemParentGridLayoutGroup;
    
    public void ClosetInit(ClosetKind closetKind)
    {
        Resets();
        itemParentGridLayoutGroup = itemParent.GetComponent<GridLayoutGroup>();
        itemParentGridLayoutGroup.cellSize = new Vector2(220, 216);
        itemParentGridLayoutGroup.spacing = new Vector2(4,18);
        /*
         * UnSafeArea로 대체
         * 
        itemParentGridLayoutGroup.padding.top = 50;
        itemParentGridLayoutGroup.padding.bottom = 50;
        */
        itemParentGridLayoutGroup.constraintCount = 4;

        playingCharacter = FindPlayingCharacterName();
        closetInfoDatas = new List<ClosetData>();

        if(closetKind == ClosetKind.Shirts)
        {
            closetInfoDatas = DataManager.Instance.GetClosetDataWithKind(closetKind);
            closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithKind(ClosetKind.Dress));
            closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithKind(ClosetKind.Costume));
            ClosetData defaultShirts = closetInfoDatas.Find(x => x.name == "Shirts_000");
            closetInfoDatas.Remove(defaultShirts);
        }
        else if (closetKind == ClosetKind.Pants)
        {
            closetInfoDatas = DataManager.Instance.GetClosetDataWithKind(closetKind);
            ClosetData defaultPants = closetInfoDatas.Find(x => x.name == "Pants_000");
            closetInfoDatas.Remove(defaultPants);
        }
        //모자와 안경 분리_230823 박진범
        //else if (closetKind == ClosetKind.HeadDress)
        //{
        //    closetInfoDatas = DataManager.Instance.GetClosetDataWithKind(closetKind);
        //    closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithKind(ClosetKind.Glasses));
        //}

        //캐릭터 전용파트 사용할 경우 활성화
        //else if (closetKind == ClosetKind.BackHair)
        //{
        //    closetInfoDatas = DataManager.Instance.GetClosetDataWithCharacterName(closetKind, "Common");
        //    closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithCharacterName(closetKind, playingCharacter));
        //}
        //else if (closetKind == ClosetKind.Eyes)
        //{
        //    closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithCharacterName(closetKind, playingCharacter));
        //}
        //else if (closetKind == ClosetKind.Eyesbrow)
        //{
        //    closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithCharacterName(closetKind, playingCharacter));
        //}
        //else if (closetKind == ClosetKind.Mouth)
        //{
        //    closetInfoDatas.AddRange(DataManager.Instance.GetClosetDataWithCharacterName(closetKind, playingCharacter));
        //}
        //캐릭터 전용파트 사용할 경우 활성화
        else
        { 
            closetInfoDatas = DataManager.Instance.GetClosetDataWithKind(closetKind);
        }

        //기본 파트일 경우 아이템을 리스트에 보여주지않음
        List<ClosetData> basicItems = closetInfoDatas.FindAll(x => x.type == ClosetType.Basic);
        foreach (ClosetData basicItem in basicItems)
        {
            closetInfoDatas.Remove(basicItem);
        }

        //일일보상 아이템은 보여주지않음
        List<ClosetData> dailyItems = closetInfoDatas.FindAll(x => x.type == ClosetType.Daily);
        foreach (ClosetData dailyItem in dailyItems)
        {
            closetInfoDatas.Remove(dailyItem);
        }

        //출석보상 받은 아이템을 리스트에 넣어줌
        List<ClosetData> rewardDailyItems = PlayerDataManager.Instance.sl.rewardCloset.FindAll(x => x.type == ClosetType.Daily);
        foreach (ClosetData rewardDailyItem in rewardDailyItems)
        {
            if(rewardDailyItem.kind == ClosetKind.Dress || rewardDailyItem.kind == ClosetKind.Costume)
            {
                rewardDailyItem.kind = ClosetKind.Shirts;
            }
            if (rewardDailyItem.kind == closetKind)
            {
                closetInfoDatas.Add(rewardDailyItem);
            }
        }

        itemConverters = new List<ItemConverter>();
        itemSpawner.ClosetInit(itemParent, closetInfoDatas);
        itemConverters = itemSpawner.ItemConverters;
        ItemReturn();
    }

    public void BackGroundInit(BackgroundKind bgKind)
    {
        Resets();
        itemParentGridLayoutGroup = itemParent.GetComponent<GridLayoutGroup>();
        itemParentGridLayoutGroup.cellSize = new Vector2(361, 459);
        itemParentGridLayoutGroup.spacing = new Vector2(70, 20);
        /*
         * UnSafeArea로 대체
         * 
        itemParentGridLayoutGroup.padding.top = 50;
        itemParentGridLayoutGroup.padding.bottom = 50;
        */
        itemParentGridLayoutGroup.constraintCount = 2;
        
        backGroundInfoDatas = new List<BackgroundData>();
        backGroundInfoDatas = DataManager.Instance.GetBackGroundDataWithKind(bgKind);

        itemConverters = new List<ItemConverter>();
        itemSpawner.BackGroundInit(itemParent, backGroundInfoDatas);
        itemConverters = itemSpawner.ItemConverters;

        ItemReturn();
    }

    public void ItemReturn()
    {
        returnObjects = new List<GameObject>();

        int index = itemParent.childCount;
        for (int i = 0; i < index; i++)
        {
            if (itemParent.GetChild(i).gameObject.activeSelf == false)
            {
                returnObjects.Add(itemParent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < returnObjects.Count; i++)
        {
            returnObjects[i].transform.SetParent(ReturnParent.transform);
        }

        //아이템을 밑에서 위로 올라오는 효과
        itemParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10000);
    }

    public void Resets()
    {
        if (itemConverters != null)
        {
            for (int i = 0; i < itemConverters.Count; i++)
            {
                itemConverters[i].Hide();
            }

            itemConverters = new List<ItemConverter>();
            closetInfoDatas = new List<ClosetData>();
        }

        if(returnObjects != null)
        {
            for (int i = 0; i < returnObjects.Count; i++)
            {
                returnObjects[i].transform.SetParent(itemParent);
            }
        }

        returnObjects = new List<GameObject>();
    }

    public string FindPlayingCharacterName()
    {
        int characterId = PlayerDataManager.Instance.GetUserInfo().playingCharacter;
        if(characterId == 1 || characterId == 14 || characterId == 19)
        {
            return "Dalimi";
        }
        else if(characterId == 2 || characterId == 10 || characterId == 26)
        {
            return "FriendA";
        }
        else if (characterId == 3 || characterId == 15 || characterId == 22)
        {
            return "FriendB";
        }
        else if (characterId == 4 || characterId == 12 || characterId == 25)
        {
            return "FriendC";
        }
        else if (characterId == 5 || characterId == 11 || characterId == 20)
        {
            return "FriendD";
        }
        else if (characterId == 6 || characterId == 13 || characterId == 24)
        {
            return "FriendE";
        }
        else if (characterId == 7 || characterId == 16 || characterId == 21)
        {
            return "FriendF";
        }
        else if (characterId == 8 || characterId == 18 || characterId == 27)
        {
            return "FriendG";
        }
        else if (characterId == 9 || characterId == 17 || characterId == 23)
        {
            return "Suny";
        }
        else
        {
            return "Common";
        }
    }
}
