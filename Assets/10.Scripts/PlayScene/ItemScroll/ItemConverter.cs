using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using I2.Loc;

public class ItemConverter : MonoBehaviour
{
    [SerializeField] private Item item;
    private ClosetData closetData;
    private BackgroundData backgroundData;

    public ItemList itemList; 
    public Image btnImage;
    public GameObject sticker;
    public Sprite closetNomalBtn;
    public Sprite clostPushBtn;
    public Sprite bgNomalBtn;
    public Sprite bgPushBtn;

    [Header("ItemInfo")]
    public Image itemImage;
    public int itemId;
    public string itemName;
    public ClosetKind closetItemKind;
    public ClosetType closetItemType;
    public BackgroundKind backgroundItemKind;
    public BackgroundType backgroundItemType;
    public int itemlinkId;
    public Animator scrollItemAnim;

    private Wash wash;

    /// <summary>
    /// 의상아이템 스크롤리스트 초기화
    /// </summary>
    public void ClosetInit(ClosetData itemInfodata, Wash wash)
    {
        closetData = itemInfodata;
        this.wash = wash;
        itemList = ItemList.Clost;
        itemId = itemInfodata.id;
        itemName = itemInfodata.name;
        closetItemKind = itemInfodata.kind;
        closetItemType = itemInfodata.type;
        backgroundItemKind = BackgroundKind.None;
        backgroundItemType = BackgroundType.None;

        List<ClosetData> rewardItems = PlayerDataManager.Instance.sl.rewardCloset;
        ClosetData rewardItem = rewardItems.Find(x => x.id == itemId);
        if(rewardItem != null ||
            AdsManager.Instance.HasRemoveAds)
        {
            closetItemType = ClosetType.Default;
        }

        if(closetItemType == ClosetType.Ad)
        {
            sticker.SetActive(true);
        }
        else
        {
            sticker.SetActive(false);
        }

        itemlinkId = itemInfodata.linkId;
        itemImage.sprite = DataManager.Instance.GetCharacterPartUISprite(itemName);
        itemImage.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        itemImage.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        btnImage.sprite = closetNomalBtn;
        ItemEvent.itemDelectedHandler += ItemDeSelected;
        AdsEvent.OnAdsRemoved += AdsRemoved;
    }

    /// <summary>
    /// 배경아이템 스크롤리스트 초기화
    /// </summary>
    public void BackGroundInit(BackgroundData itemInfodata)
    {
        backgroundData = itemInfodata;
        itemList = ItemList.Bg;
        itemId = itemInfodata.id;
        itemName = itemInfodata.name;
        backgroundItemKind = itemInfodata.kind;
        backgroundItemType = itemInfodata.type;
        closetItemKind = ClosetKind.None;
        closetItemType = ClosetType.None;

        List<BackgroundData> rewardItems = PlayerDataManager.Instance.sl.rewardBackground;
        BackgroundData rewardItem = rewardItems.Find(x => x.id == itemId);
        if (rewardItem != null ||
            AdsManager.Instance.HasRemoveAds)
        {
            backgroundItemType = BackgroundType.Default;
        }

        if (backgroundItemType == BackgroundType.Ad)
        {
            sticker.SetActive(true);
        }
        else
        {
            sticker.SetActive(false);
        }

        itemImage.sprite = DataManager.Instance.GetBackGroundUISprite(itemName);
        itemImage.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(42, 42);
        itemImage.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-11, -11);
        btnImage.sprite = bgNomalBtn;
        ItemEvent.itemDelectedHandler += ItemDeSelected;
        AdsEvent.OnAdsRemoved += AdsRemoved;
    }

    //아이탬 버튼 클릭시 실행되는 함수
    public void SetUpItem()
    {
        if(!Statics.itemBtn)
        {
            return;
        }

        ItemEvent.UpdateItemDeSelected();

        if(closetItemKind == ClosetKind.Cheek)
        {
            wash.cheekNum = itemId - DataManager.Instance.GetItemIdWithName("Cheek_001");
            if (wash.firstCheek)
            {
                wash.ChangeTex(wash.cheeks[wash.cheekNum]);
                wash.clearParticle.Play();
                if(PlayManager.Instance.clearStep == GameStep.Cheek)
                {
                    PlayManager.Instance.clearStep = GameStep.Eyes;
                }
                PlayManager.Instance.UserInfoUpdate();
                PlayManager.Instance.ShowNextButton(true);
            }
            else
            {
                SoundManager.Instance.OnPlayOneShot("se_01");
                wash.ShowBeautyTool();
            }
        }
        else if(backgroundItemKind == BackgroundKind.Bg)
        {
            if (backgroundItemType == BackgroundType.Ad)
            {
                MainSceneController.Instance.ShowAdUI(() =>
                {
                    AdsManager.Instance.WatchVideoWithClothItem(acs =>
                    {
                        switch (acs)
                        {
                            case AdsManager.AdsCallbackState.Success:
                                MainSceneController.Instance.ShowRewardUI(itemImage.sprite);
                                backgroundItemType = BackgroundType.Default;
                                sticker.SetActive(false);
                                PlayerDataManager.Instance.AddRewardBackGround(backgroundData);
                                break;
                            case AdsManager.AdsCallbackState.Loading:
                                MainSceneController.Instance.ShowAlarmUI(LocalizationManager.GetTermTranslation("LoadAd"));
                                break;
                        }
                    });
                });
            }
            else
            {
                SoundManager.Instance.OnPlayOneShot("se_01");
                PlayManager.Instance.ChangeBackGroundItem(itemId, true);
            }
        }
        else
        {
            if(closetItemType == ClosetType.Ad)
            {
                MainSceneController.Instance.ShowAdUI(() =>
                {
                    AdsManager.Instance.WatchVideoWithClothItem(acs =>
                    {
                        switch (acs)
                        {
                            case AdsManager.AdsCallbackState.Success:
                                MainSceneController.Instance.ShowRewardUI(itemImage.sprite);
                                closetItemType = ClosetType.Default;
                                sticker.SetActive(false);
                                PlayerDataManager.Instance.AddRewardCloset(closetData);
                                break;
                            case AdsManager.AdsCallbackState.Loading:
                                MainSceneController.Instance.ShowAlarmUI(LocalizationManager.GetTermTranslation("LoadAd"));
                                break;
                        }
                    });
                });
            }
            else
            {
                SoundManager.Instance.OnPlayOneShot("se_01");
                PlayManager.Instance.ChangeBeautyItem(itemId);
            }
        }

        if(itemList == ItemList.Clost)
        {
            this.GetComponent<Animator>().SetBool("ItemSelect", true);
        }
        else if(itemList == ItemList.Bg)
        {
            btnImage.sprite  = bgPushBtn;
        }
    }

    public void ItemDeSelected()
    {
        if (itemList == ItemList.Clost)
        {
            this.GetComponent<Animator>().SetBool("ItemSelect", false);
        }
        else if (itemList == ItemList.Bg)
        {
            btnImage.sprite = bgNomalBtn;
        }
    }

    private void AdsRemoved()
    {
        if (itemList == ItemList.Clost)
        {
            closetItemType = ClosetType.Default;
        }
        else if (itemList == ItemList.Bg)
        {
            backgroundItemType = BackgroundType.Default;
        }
        sticker.SetActive(false);
    }

    public void Hide()
    {
        ItemEvent.itemDelectedHandler -= ItemDeSelected;
        AdsEvent.OnAdsRemoved -= AdsRemoved;
        item.Hide();
    }
}
