using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayManager : MonoSingleton<PlayManager>
{
    public void ContinueGame()
    {
        clearStep = playerUserInfo.clearStep;
        gameStep = clearStep;
        PlayerDataManager.Instance.SetUserInfoGameStep(gameStep);
        SetUpGameStage();

        SaveCharacter saveCharacter = PlayerDataManager.Instance.GetSaveCharacterWithId(playerUserInfo.playingSaveId);
        if (saveCharacter != null)
        {
            CreateContinueCharacter(saveCharacter);
        }

        //스테이지에 맞는 배경 변경
        bgStage.SetUpDefaultBg(gameStage);
        wash.ChangeTex(wash.clearTex);
        switch (gameStage)
        {
            case GameStage.Wash:
                gameStage = GameStage.Wash;
                character.ContinueCharacterPos(42.0f, 466f, 0.0f, 1.3f, 1.3f, 1.0f);
                PlayWash(gameStep);
                break;

            case GameStage.Beauty:
                gameStage = GameStage.Beauty;
                character.ContinueCharacterPos(42.0f, 531f, 0.0f, 1.3f, 1.3f, 1.0f);
                PlayBeauty(gameStep);
                break;

            case GameStage.Closet:
                gameStage = GameStage.Closet;
                character.ContinueCharacterPos(42.0f, 1002f, 0.0f, 0.45f, 0.45f, 1.0f);
                PlayCloset(gameStep);
                break;

            case GameStage.Accessory:
                gameStage = GameStage.Accessory;
                PlayAccessroy(gameStep);
                break;

            case GameStage.Bg:
                gameStage = GameStage.Bg;
                character.ContinueCharacterPos(42.0f, 1002f, 0.0f, 0.45f, 0.45f, 1.0f);
                PlayBg(gameStep);
                break;
        }

        stepIcon.UpdateStatIcon(gameStep);

        if (gameStage != GameStage.Wash)
        {
            if (gameStage != GameStage.Bg)
            {
                itemScrollShow(true);
            }
            wash.WashToolHide();
        }
    }

    public void PlayWash(GameStep gameStep)
    {
        wash.WashToolHide();
        wash.toolParticle.SetActive(true);
        switch (gameStep)
        {
            case GameStep.Soap:
                this.gameStep = GameStep.Soap;
                wash.ChangeTex(wash.dirtyMask);
                wash.washToolObj[0].SetActive(true);
                wash.guideHandanim.gameObject.SetActive(true);
                wash.guideHandanim.SetBool("Wash", true);
                break;
            case GameStep.Shower:
                this.gameStep = GameStep.Shower;
                wash.ChangeTex(wash.bubbleMask);
                wash.washToolObj[1].SetActive(true);
                break;
            case GameStep.Towel:
                this.gameStep = GameStep.Towel;
                wash.ChangeTex(wash.dropMask);
                wash.washToolObj[2].SetActive(true);
                break;
            case GameStep.MassagePack:
                this.gameStep = GameStep.MassagePack;
                wash.ChangeTex(wash.clearTex);
                wash.washToolObj[3].SetActive(true);
                break;
            case GameStep.Mask:
                this.gameStep = GameStep.Mask;
                wash.ChangeTex(wash.clearTex);
                wash.washToolObj[4].SetActive(true);
                wash.MaskInit();
                break;
            case GameStep.Cucumber:
                this.gameStep = GameStep.Cucumber;
                wash.ChangeTex(wash.clearTex);
                wash.washToolObj[5].SetActive(true);
                wash.CucumberInit();
                break;
            case GameStep.Cream:
                this.gameStep = GameStep.Cream;
                wash.DestroyCucumber();
                wash.ChangeTex(wash.clearTex);
                wash.washToolObj[6].SetActive(true);
                break;
            case GameStep.Cheek:
                playerUserInfo.playingStage = GameStage.Beauty;
                ContinueGame();
                break;
        }
    }

    public void PlayBeauty(GameStep gameStep)
    {
        switch (gameStep)
        {
            case GameStep.Cheek:
                this.gameStep = GameStep.Cheek;
                wash.beautyTool.gameObject.SetActive(false);
                break;
            case GameStep.Eyes:
                this.gameStep = GameStep.Eyes;
                break;
            case GameStep.Eyesbrow:
                this.gameStep = GameStep.Eyesbrow;
                break;
            case GameStep.Mouth:
                this.gameStep = GameStep.Mouth;
                break;
        }
        //캐릭터 텍스쳐변경
        ChangeCheekTexture2D(saveCharacter.cheekName);
        closet.ChangeScrollItem(gameStep);
    }

    public void PlayCloset(GameStep gameStep)
    {
        switch (gameStep)
        {
            case GameStep.BackHair:
                this.gameStep = GameStep.BackHair;
                break;
            case GameStep.Shirts:
                this.gameStep = GameStep.Shirts;
                break;
            case GameStep.Pants:
                this.gameStep = GameStep.Pants;
                break;
            case GameStep.Socks:
                this.gameStep = GameStep.Socks;
                break;
            case GameStep.Shoes:
                this.gameStep = GameStep.Shoes;
                break;
            case GameStep.Glasses:
                this.gameStep = GameStep.Glasses;
                break;
            case GameStep.HeadDress:
                this.gameStep = GameStep.HeadDress;
                break;
            case GameStep.Earring:
                gameStage = GameStage.Accessory;
                break;
        }
        //캐릭터 텍스쳐변경
        ChangeCheekTexture2D(saveCharacter.cheekName);
        closet.ChangeScrollItem(gameStep);
    }

    public void PlayAccessroy(GameStep gameStep)
    {
        switch (gameStep)
        {
            case GameStep.Earring:
                this.gameStep = GameStep.Earring;
                character.ContinueCharacterPos(42.0f, 695f, 0.0f, 1.0f, 1.0f, 1.0f);
                break;
            case GameStep.Neckless:
                this.gameStep = GameStep.Neckless;
                character.ContinueCharacterPos(42.0f, 695f, 0.0f, 1.0f, 1.0f, 1.0f);
                break;
            case GameStep.FaceAc:
                this.gameStep = GameStep.FaceAc;
                character.ContinueCharacterPos(42.0f, 695f, 0.0f, 1.0f, 1.0f, 1.0f);
                break;
            case GameStep.Bracelet:
                this.gameStep = GameStep.Bracelet;
                character.ContinueCharacterPos(42.0f, 970f, 0.0f, 0.8f, 0.8f, 1.0f);
                break;
            case GameStep.Pet:
                this.gameStep = GameStep.Pet;
                character.ContinueCharacterPos(42.0f, 1002f, 0.0f, 0.45f, 0.45f, 1.0f);
                break;
            case GameStep.Bag:
                this.gameStep = GameStep.Bag;
                character.ContinueCharacterPos(42.0f, 1002f, 0.0f, 0.45f, 0.45f, 1.0f);
                break;
        }
        //캐릭터 텍스쳐변경
        ChangeCheekTexture2D(saveCharacter.cheekName);
        closet.ChangeScrollItem(gameStep);
    }

    public void PlayBg(GameStep gameStep)
    {
        switch (gameStep)
        {
            case GameStep.Bg:
                this.gameStep = GameStep.Bg;
                bgStage.Init();
                break;
            case GameStep.Photo:
                this.gameStep = GameStep.Photo;
                cameraScreen.SetActive(true);
                DotweenManager.Instance.DoLocalMoveY(212, 1, screenShotBtnPos);
                break;
        }
        ChangeCheekTexture2D(saveCharacter.cheekName);
    }

    public void CreateContinueCharacter(SaveCharacter saveCharacter)
    {
        this.saveCharacter = saveCharacter;
        List<string> partName = new List<string>();
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.backHairId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.bodyId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.eyesbrowId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.eyesId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.mouthId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.shirtsId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.pantsId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.socksId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.shoesId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.necklaceId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.headDressId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.earringId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.glassesId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.braceletId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.petId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.faceAcId).name);
        partName.Add(DataManager.Instance.GetClosetDataWithId(saveCharacter.bagId).name);
        
        //캐릭터 의상 변경
        foreach (string item in partName)
        {
            ClosetData closetData = DataManager.Instance.GetClosetDataWithName(item);
            if(item == "NullImage")
            {
                continue;
            }
            character.ChangeCloset(closetData.id);
        }
    }

    private void SetUpGameStage()
    {
        if(gameStep == GameStep.Soap || gameStep == GameStep.Shower || gameStep == GameStep.Towel || gameStep == GameStep.MassagePack ||
            gameStep == GameStep.Mask || gameStep == GameStep.Cucumber || gameStep == GameStep.Cream)
        {
            gameStage = GameStage.Wash;
        }
        else if(gameStep == GameStep.Cheek || gameStep == GameStep.Eyes || gameStep == GameStep.Eyesbrow || gameStep == GameStep.Mouth)
        {
            gameStage = GameStage.Beauty;
        }
        else if (gameStep == GameStep.BackHair || gameStep == GameStep.Shirts || gameStep == GameStep.Pants ||
            gameStep == GameStep.Socks || gameStep == GameStep.Shoes || gameStep == GameStep.Glasses || gameStep == GameStep.HeadDress)
        {
            gameStage = GameStage.Closet;
        }
        else if (gameStep == GameStep.Earring || gameStep == GameStep.Neckless || gameStep == GameStep.Bracelet ||
           gameStep == GameStep.FaceAc || gameStep == GameStep.Pet || gameStep == GameStep.Bag)
        {
            gameStage = GameStage.Accessory;
        }
        else if (gameStep == GameStep.Bg || gameStep == GameStep.Photo)
        {
            gameStage = GameStage.Bg;
        }
        else
        {
            Debug.Log("해당 스테이지 없음 에러");
        }
    }
}


