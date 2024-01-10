using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public partial class PlayManager : MonoSingleton<PlayManager>
{
    [Header("Character")]
    [SerializeField] private Character character;
    [SerializeField] private Character[] preSetCharacter;

    [Header("ButtonObject")]
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private GameObject continueParticle;
    [SerializeField] private GameObject cameraScreen;
    [SerializeField] private Button screenShotBtn;
    [SerializeField] private Button playBtn;
    [SerializeField] private GameObject itemScroll;
    private RectTransform nextBtnPos;
    private RectTransform prevBtnPos;
    private RectTransform continuBtnPos;
    private RectTransform screenShotBtnPos;
    private RectTransform playBtnPos;
    private RectTransform itemScrollPos;

    [Header("GameTest")]
    private GameStep gameStep;
    private GameStep lastStep;
    private ClosetKind selectedCloset;
    public GameStep clearStep { get; set; }
    public GameStage gameStage { get; set; }

    [Header("Save")]
    [SerializeField] private SaveBehaviour saveBehaviour;
    [SerializeField] private SaveCharacter saveCharacter;
    [SerializeField] private List<SaveCharacter> userInfoSaveCharacter;
    [SerializeField] private UserInfo playerUserInfo;

    [Header("Prefabs")]
    [SerializeField] private GameObject optionPrefabs;
    [SerializeField] private GameObject exitUIPrefabs;

    [Header("UI")]
    [SerializeField] private StepIcon stepIcon;
    [SerializeField] private Wash wash;
    [SerializeField] private Closet closet;
    [SerializeField] private BgStage bgStage;

    [Header("TimeLine")]
    [SerializeField] private PlayableDirector clearPlayableDirector;
    [SerializeField] private TimelineAsset clearTimelineAsset;
    [SerializeField] private CleareParticle cleareParticle;

    public void Init()
    {
        clearPlayableDirector.Stop();
        cleareParticle.StopClearParticle();
        Statics.itemBtn = true;
        userInfoSaveCharacter = PlayerDataManager.Instance.GetSaveCharacters();
        playerUserInfo = PlayerDataManager.Instance.GetUserInfo();

        //두트윈 애니메이션 사용을 위한컴포넌트 세팅
        nextBtnPos = nextBtn.GetComponent<RectTransform>();
        prevBtnPos = prevBtn.GetComponent<RectTransform>();
        continuBtnPos = continueBtn.GetComponent<RectTransform>();
        screenShotBtnPos = screenShotBtn.GetComponent<RectTransform>();
        playBtnPos = playBtn.GetComponent<RectTransform>();
        itemScrollPos = itemScroll.GetComponent<RectTransform>();

        nextBtnPos.anchoredPosition = new Vector2(300, 584);
        prevBtnPos.anchoredPosition = new Vector2(-300, 584);
        continuBtnPos.anchoredPosition = new Vector2(0, -543);
        screenShotBtnPos.anchoredPosition = new Vector2(0, -300);
        playBtnPos.anchoredPosition = new Vector2(0, -300);
        itemScrollPos.anchoredPosition = new Vector2(0, -1000);
        preSetCharacter[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, 0, 0);
        preSetCharacter[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(1000, 0, 0);
        cameraScreen.SetActive(false);
        continueParticle.SetActive(false);

        //저장되어있는 유저데이터 세팅
        if (Statics.selectedCharacter > playerUserInfo.clearCharacterCount)
        {
            Statics.selectedCharacter = playerUserInfo.clearCharacterCount;
        }
        gameStage = playerUserInfo.playingStage;
        gameStep = playerUserInfo.clearStep;
        lastStep = gameStep;
        bgStage.selectedBackGround = PlayerDataManager.Instance.s.userinfo.backGroundName;
        //캐릭터 초기 설정
        character.Init();
        //현재선택한 캐릭터가 유저데이터에 저장되어있는 캐릭터가 동일하다면 이어서 진행
        if (playerUserInfo.playingCharacter == Statics.selectedCharacter && userInfoSaveCharacter.Last().trueSave == false)
        {
            wash.guideHandanim.gameObject.SetActive(false);
            character.saveId = playerUserInfo.playingSaveId;
            if (gameStage != GameStage.Wash && gameStep != GameStep.Cheek)
            {
                ShowPrevButton(true);
            }

            if(gameStep > GameStep.Cheek)
            {
                wash.firstCheek = true;
            }

            ContinueGame();
        }
        //현재선택한 캐릭터가 유저데이터에 저장되어있는 캐릭터가 동일하지않다면 처음부터 진행
        else
        {
            //SaveCharacter 리스트 정리
            List<int> removeSaveCharacterIndex = new List<int>();

            for (int i = 0; i < userInfoSaveCharacter.Count; i++)
            {
                if (!userInfoSaveCharacter[i].trueSave)
                {
                    removeSaveCharacterIndex.Add(userInfoSaveCharacter[i].saveId);
                }
            }
            for (int i = 0; i < removeSaveCharacterIndex.Count; i++)
            {
                SaveCharacter removeData = userInfoSaveCharacter.Find(x => x.saveId == removeSaveCharacterIndex[i]);
                userInfoSaveCharacter.Remove(removeData);
            }
            character.saveId = userInfoSaveCharacter.Count + 1;
            gameStage = GameStage.Wash;
            gameStep = GameStep.Soap;
            clearStep = GameStep.Soap;
            lastStep = GameStep.Soap;
            bgStage.selectedBackGround = "";
            bgStage.SetUpDefaultBg(gameStage);
            UserInfoUpdate();
            //Wash단계 초기화
            wash.Init();
            stepIcon.ShowStepIconObj(true);
            stepIcon.UpdateStatIcon(gameStep);
        }

        LikeChatCharacterInit();
    }

    public void LikeChatCharacterInit()
    {
        foreach (var item in preSetCharacter)
        {
            int randomCount = Random.Range(1, 20);
            item.CreateCharacter(randomCount);
            item.CreateLikeChatCharacter();
            item.gameObject.SetActive(false);
        }
    }

    public void UpdateGameStepInWash()
    {
        clearStep++;
        if (gameStep != GameStep.Cheek)
        {
            gameStep++;
        }
        UserInfoUpdate();
    }

    public void ChangeBeautyItem(int itemId)
    {
        selectedCloset = DataManager.Instance.GetClosetDataWithId(itemId).kind;
        character.RemoveSamePart(itemId);
        character.particlePlay = true;
        character.ChangeCloset(itemId);
        if (selectedCloset == ClosetKind.Shirts)
        {
            character.ShowDefaultPants();
        }

        UpdateGameStep();
        ShowNextButton(true);
    }

    public void ChangeBackGroundItem(int itemId, bool showNextBtn)
    {
        bgStage.ChangeBackGround(itemId);
        UpdateGameStep();
        ShowNextButton(showNextBtn);
    }

    public void UpdateGameStep()
    {
        //드레스 및 커스톰 셔츠는 클릭시 버튼 꼬이는현상 if문
        if (selectedCloset == ClosetKind.Dress || selectedCloset == ClosetKind.Costume || selectedCloset == ClosetKind.Shirts)
        {
            if(selectedCloset == ClosetKind.Costume)
            {
                gameStage = GameStage.Accessory;
                clearStep = lastStep >= clearStep ? lastStep : GameStep.Earring;
                if (clearStep == GameStep.Pants || clearStep == GameStep.Socks || clearStep == GameStep.Shoes || clearStep == GameStep.Glasses || clearStep == GameStep.HeadDress)
                {
                    clearStep = GameStep.Earring;
                }
            }
            else if (selectedCloset == ClosetKind.Dress)
            {
                gameStage = GameStage.Closet;
                if(clearStep == GameStep.Pants)
                {
                    clearStep = GameStep.Socks;
                }
                else if(clearStep == GameStep.Socks || clearStep == GameStep.Shoes || clearStep == GameStep.Glasses || clearStep == GameStep.HeadDress || clearStep == GameStep.Earring)
                {
                    //클리어 스탭 변화 없음
                }
                else
                {
                    clearStep = lastStep >= clearStep ? lastStep : GameStep.Socks;
                }
            }
            else if(selectedCloset == ClosetKind.Shirts)
            {             
                gameStage = GameStage.Closet;
                if (clearStep == GameStep.Shoes || clearStep == GameStep.Glasses || clearStep == GameStep.HeadDress || clearStep == GameStep.Earring)
                {
                    //클리어 스탭 변화 없음
                }
                else if(clearStep == GameStep.Socks && lastStep == GameStep.Pants)
                {
                    clearStep = GameStep.Socks;
                }
                else
                { 
                    clearStep = lastStep >= clearStep ? lastStep : GameStep.Pants;
                }
            }
            selectedCloset = ClosetKind.Shirts;
        }
        //모자와 안경 분리_230823 박진범
        //else if (selectedCloset == ClosetKind.Glasses)
        //{
        //    selectedCloset = ClosetKind.HeadDress;
        //}

        if(gameStage == GameStage.Bg)
        {
            selectedCloset = ClosetKind.Bg;
        }

        string selectClosetType = selectedCloset.ToString();
        GameStep nowStep = (GameStep)System.Enum.Parse(typeof(GameStep), selectClosetType);

        if (nowStep == clearStep)
        {
            clearStep++;
            if(character.GetShirtsName() == "Dress" && selectedCloset == ClosetKind.Shirts)
            {
                clearStep++;
            }
            if (character.GetShirtsName() == "Costume" && selectedCloset == ClosetKind.Shirts)
            {
                clearStep = GameStep.Earring;
            }
        }
        if ((int)gameStep >= 25)
        {
            gameStep = GameStep.Photo;
            clearStep = GameStep.Photo;
        }
        UserInfoUpdate();
    }

    public void UserInfoUpdate()
    {
        playerUserInfo.clearCharacterCount = Statics.clearCharacterCount;
        playerUserInfo.playingCharacter = Statics.selectedCharacter;
        playerUserInfo.playingStage = gameStage;
        playerUserInfo.playingStep = gameStep;
        playerUserInfo.clearStep = clearStep;
        playerUserInfo.playingSaveId = character.saveId;
        playerUserInfo.backGroundName = bgStage.selectedBackGround;
        saveBehaviour.CharacterSave(character, wash.paint, bgStage, false);
    }

    public void ChangeHair()
    {
        character.ChangeHair(Statics.selectedCharacter);
    }

    public string ClosetItemBtnImageUpdate(List<ItemConverter> itemConverters)
    {
        ClosetKind closetKind = itemConverters[0].closetItemKind;
        if (itemConverters[0].closetItemKind == ClosetKind.Dress || itemConverters[0].closetItemKind == ClosetKind.Costume)
        {
            closetKind = ClosetKind.Shirts;
        }

        if (closetKind == ClosetKind.Cheek)
        {
            return wash.paint.pattenTexture.name;
        }
        GameObject closetPartObj;
        for (int i = 0; i < character.transform.GetChild(1).childCount; i++)
        {
            if (character.transform.GetChild(1).GetChild(i).gameObject.name == closetKind.ToString())
            {
                closetPartObj = character.transform.GetChild(1).GetChild(i).gameObject;
                return closetPartObj.GetComponent<Image>().sprite.name;
            }
        }
        return null;
    }

    public string BackGroundItemBtnImageUpdate()
    {
        string selectedBackGround = bgStage.GetSelectedBackGround();
        if(selectedBackGround == "")
        {
            return bgStage.defaultBg[2].name;
        }
        else
        {
            return selectedBackGround;
        }
    }

    public void ChangeCheekTexture2D(string cheekName)
    {
        Texture2D cheekTex = wash.cheeks.Find(x => x.name == cheekName);
        wash.ChangeTex(cheekTex);
    }

    public void RemoveMask()
    {
        StartCoroutine(wash.RemoveMask());
    }

    public void RemoveCucumber()
    {
        StartCoroutine(wash.RemoveCucumber());
    }

    public void ClickedOption()
    {
        SoundManager.Instance.OnClickSoundEffect();
        GameObject obj;
        obj = Instantiate(optionPrefabs, this.transform);
        obj.GetComponent<OptionUI>().Init();
    }

    public void ClicekdHome()
    {
        SoundManager.Instance.OnClickSoundEffect();
        exitUIPrefabs.SetActive(true);
    }

    public void itemScrollShow(bool show)
    {
        if(show)
        {
            DotweenManager.Instance.DoLocalMoveY(-160, 1, itemScrollPos);
        }
        else if(!show)
        {
            DotweenManager.Instance.DoLocalMoveY(-1000, 1, itemScrollPos);
        }
    }
}