using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class MainSceneController : MonoSingleton<MainSceneController>
{
    [Header("Scene")]
    [SerializeField] private GameObject mainScene;
    [SerializeField] private GameObject playScene;
    [SerializeField] private GameObject albumScene;
    [SerializeField] private GameObject alarmUIPrefabs;
    [SerializeField] private GameObject rewardUIPrefabs;
    [SerializeField] private GameObject adUIPrefabs;
    [SerializeField] private GameObject appReviewUIPrefabs;
    [SerializeField] private GameObject screenShotCanvas;
    [SerializeField] private Transform popUpCanvas;
    [SerializeField] private AttendanceUI attendanceUI;
    [SerializeField] private CharacterScrollManager characterScrollManager;
    //public int startCharacterIndex;

    public void Start()
    {
        //하루치 보상을 받았으면 더이상 자동출력 X
        if (PlayerDataManager.Instance.GetUserInfo().attendanceData.isGet)
        {
            attendanceUI.notification.SetActive(false);
        }
        else if (!PlayerDataManager.Instance.GetUserInfo().attendanceData.isGet)
        {
            attendanceUI.notification.SetActive(true);
        }

        LoadingPanel.Instance.Hide();
    }

    public void ShowMainScene()
    {
        SoundManager.Instance.OnClickSoundEffect();

        if(playScene.activeSelf)
        {
            playScene.SetActive(false);
        }
        else if(albumScene.activeSelf)
        {
            albumScene.SetActive(false);
        }
        mainScene.SetActive(true);
        characterScrollManager.ContentUpdate();
        characterScrollManager.CharacterRePosition(Statics.selectedCharacter);
    }

    public void ShowPlayScene()
    {
        SoundManager.Instance.OnClickSoundEffect();

        //검은캐릭터 클릭시 스크롤 이동후 씬으로 이동
        if (Statics.selectedCharacter == Statics.clearCharacterCount + 1 )
        {
            StartCoroutine(ClickedEndCharacter());
        }
        else
        {
            ShowLoading();
            mainScene.SetActive(false);
            playScene.SetActive(true);
            PlayManager.Instance.Init();
        }
    }

    public void ShowAlbumScene()
    {
        SoundManager.Instance.OnClickSoundEffect();
        mainScene.SetActive(false);
        albumScene.SetActive(true);
        //playScene.SetActive(false);
        albumScene.GetComponent<AlbumSceneManager>().Init();
        ShowScreenShotCanvas(true);
    }

    public void BuyAdRemove()
    {
        bool buyAdRemove = PlayerDataManager.Instance.s.userinfo.buyAdRemove;
        if(buyAdRemove)
        {
            if (PlayerDataManager.Instance.language == "korean")
            {
                ShowAlarmUI("구매완료");
            }
            else if (PlayerDataManager.Instance.language == "english")
            {
                ShowAlarmUI("Purchase Completed");
            }
        }
        else if(!buyAdRemove)
        { 
            List<ClosetData> closetDatas = DataManager.Instance.ClosetDatas;
            List<BackgroundData> backgroundDatas = DataManager.Instance.BackgroundDatas;
            foreach (ClosetData closet in closetDatas)
            {
                if(closet.type == ClosetType.Ad)
                {
                    closet.type = ClosetType.Default;
                    PlayerDataManager.Instance.AddRewardCloset(closet);
                }
            }

            foreach (BackgroundData background in backgroundDatas)
            {
                if (background.type == BackgroundType.Ad)
                {
                    background.type = BackgroundType.Default;
                    PlayerDataManager.Instance.AddRewardBackGround(background);
                }
            }
            PlayerDataManager.Instance.s.userinfo.buyAdRemove = true;
            PlayerDataManager.Instance.SaveData();
            if (PlayerDataManager.Instance.language == "korean")
            {
                ShowAlarmUI("구매성공");
            }
            else if (PlayerDataManager.Instance.language == "english")
            {
                ShowAlarmUI("Purchase Sucess");
            }
        }
    }

    public void ShowAlarmUI(string text)
    {
        GameObject obj;
        obj = Instantiate(alarmUIPrefabs, popUpCanvas);
        obj.GetComponent<AlarmUI>().Init(text);
    }

    public void ShowRewardUI(Sprite itemsprite)
    {
        GameObject obj;
        obj = Instantiate(rewardUIPrefabs, popUpCanvas);
        obj.GetComponent<RewardUI>().Init(itemsprite);
    }

    public void ShowAdUI(Action callback)
    {
        GameObject obj;
        obj = Instantiate(adUIPrefabs, popUpCanvas);
        obj.GetComponent<AdUI>().Init(callback);
    }

    public void ShowAppReviewUI()
    {
        if (!AppReviewManager.Instance.CheckAppReview())
        {
            return;
        }

        Instantiate(appReviewUIPrefabs, popUpCanvas);
    }

    public void ShowScreenShotCanvas(bool show)
    {
        screenShotCanvas.SetActive(show);
    }

    private IEnumerator ClickedEndCharacter()
    {
        characterScrollManager.EndCharacterRePosition();
        yield return new WaitForSeconds(0.7f);
        ShowLoading();
        mainScene.SetActive(false);
        playScene.SetActive(true);
        PlayManager.Instance.Init();
    }

    private void ShowLoading()
    {
        LoadingPanel.Instance.Show(() =>
        {
            LoadingPanel.Instance.Hide();
        });
        LoadingPanel.Instance.End();
    }
}
