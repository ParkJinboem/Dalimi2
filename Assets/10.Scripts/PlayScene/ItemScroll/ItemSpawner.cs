using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Wash wash;
    [SerializeField] private ItemPool itemPool;
    public List<ItemConverter> itemConverters;
    public List<ItemConverter> ItemConverters
    {
        get { return itemConverters; }
    }

    public void ClosetInit(Transform itemParent, List<ClosetData> itemInfoDatas)
    {
        itemConverters = new List<ItemConverter>();
        CreateClosetItem(itemParent, itemInfoDatas);
    }

    public void CreateClosetItem(Transform itemParent, List<ClosetData> itemInfoDatas)
    {
        for (int i = 0; i < itemInfoDatas.Count; i++)
        {
            ClosetItemSpawn(itemParent, itemInfoDatas[i]);
        }

        //의상푸쉬버튼 설정
        string[] clostImageName = PlayManager.Instance.ClosetItemBtnImageUpdate(itemConverters).Split('(');
        if (clostImageName != null)
        {
            for (int i = 0; i < itemConverters.Count; i++)
            {
                if (itemConverters[i].itemName == clostImageName[0])
                {
                    itemConverters[i].scrollItemAnim.SetBool("ItemSelect", true);
                }
                else
                {
                    itemConverters[i].scrollItemAnim.SetBool("ItemSelect", false);
                }
            }
        }
    }

    public void ClosetItemSpawn(Transform itemParent, ClosetData closetInfoData)
    {
        ItemConverter itemConverter = itemPool.Pop(itemParent.position).instance.GetComponent<ItemConverter>();
        itemConverter.ClosetInit(closetInfoData, wash);
        itemConverters.Add(itemConverter);
    }

    public void BackGroundInit(Transform itemParent, List<BackgroundData> itemInfoDatas)
    {
        itemConverters = new List<ItemConverter>();
        CreateBackGroundItem(itemParent, itemInfoDatas);
    }

    public void CreateBackGroundItem(Transform itemParent, List<BackgroundData> itemInfoDatas)
    {
        for (int i = 0; i < itemInfoDatas.Count; i++)
        {
            BackGroundItemSpawn(itemParent, itemInfoDatas[i]);
        }

        string backgroundImageName = PlayManager.Instance.BackGroundItemBtnImageUpdate().Split('(')[0];

        //배경 푸쉬버튼 설정
        if (backgroundImageName != null)
        {
            for (int i = 0; i < itemConverters.Count; i++)
            {
                if (itemConverters[i].itemName == backgroundImageName)
                {
                    itemConverters[i].btnImage.sprite = itemConverters[i].bgPushBtn;
                }
            }
        }
    }

    public void BackGroundItemSpawn(Transform itemParent, BackgroundData backgroundInfoData)
    {
        ItemConverter itemConverter = itemPool.Pop(itemParent.position).instance.GetComponent<ItemConverter>();
        itemConverter.BackGroundInit(backgroundInfoData);
        itemConverters.Add(itemConverter);
    }
}
