using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

public partial class PlayManager : MonoSingleton<PlayManager>
{
    public void NextBtn()
    {
        selectedCloset = ClosetKind.None;
        SoundManager.Instance.OnClickSoundEffect();

        if (lastStep < clearStep)
        {
            lastStep = clearStep;
        }

        switch (gameStage)
        {
            case GameStage.Wash:
                if (gameStep == GameStep.Mask)
                {
                    wash.ChangeTex(wash.clearTex);
                    wash.ShowMaskGuide();
                }
                else if (gameStep == GameStep.Cucumber)
                {
                    RemoveMask();
                    wash.CucumberInit();
                }
                else if (gameStep == GameStep.Cream)
                {
                    RemoveCucumber();
                }
                else if (gameStep == GameStep.Cheek)
                {
                    DotweenManager.Instance.DoSizeImage(1.3f, 0.45f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(1002f, 1, character.GetComponent<RectTransform>());
                    DotweenManager.Instance.DoLocalMoveY(100, 1, continuBtnPos);
                    continueParticle.SetActive(true);
                    continueBtn.interactable = true;
                    foreach (var item in wash.washToolObj)
                    {
                        item.SetActive(false);
                    }
                }
                if(gameStep != GameStep.Cheek)
                {
                    wash.ShowWashTools(gameStep);
                }
                break;
            case GameStage.Beauty:
                if (gameStep == GameStep.Cheek)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_05");
                    ShowPrevButton(true);
                    closet.ChangeScrollItem(gameStep + 1);
                }
                else if (gameStep == GameStep.Eyes)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_03");
                    closet.ChangeScrollItem(gameStep + 1);
                }
                else if (gameStep == GameStep.Eyesbrow)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_07");
                    closet.ChangeScrollItem(gameStep + 1);
                }
                else if (gameStep == GameStep.Mouth)
                {
                    Statics.itemBtn = false;
                    DotweenManager.Instance.DoSizeImage(1.3f, 0.45f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(1002f, 1, character.GetComponent<RectTransform>());
                    itemScrollShow(false);
                    DotweenManager.Instance.DoLocalMoveY(100, 1, continuBtnPos);
                    continueParticle.SetActive(true);
                    continueBtn.interactable = true;
                    ShowPrevButton(false);
                    ShowNextButton(false);
                }
                break;

            case GameStage.Closet:
                if (gameStep == GameStep.Shirts)
                {
                    if(character.GetShirtsName() == "Dress")
                    {
                        gameStep = GameStep.Socks;
                        clearStep = lastStep >= clearStep ? lastStep : GameStep.Socks;
                        stepIcon.UpdateStatIcon(gameStep);
                        closet.ChangeScrollItem(gameStep);
                        UserInfoUpdate();
                        if (clearStep > gameStep)
                        {
                            ShowNextButton(true);
                        }
                        else
                        {
                            ShowNextButton(false);
                        }
                        return;
                    }
                    else if (character.GetShirtsName() == "Costume")
                    {
                        Statics.itemBtn = false;
                        gameStep = GameStep.Earring;
                        clearStep = lastStep >= clearStep ? lastStep : GameStep.Earring;
                        gameStage = GameStage.Closet;
                        stepIcon.UpdateStatIcon(gameStep);
                        itemScrollShow(false);
                        DotweenManager.Instance.DoLocalMoveY(100, 1, continuBtnPos);
                        continueParticle.SetActive(true);
                        continueBtn.interactable = true;
                        ShowPrevButton(false);
                        ShowNextButton(false);
                        UserInfoUpdate();
                        return;
                    }   
                }
                if (gameStep == GameStep.HeadDress)
                {
                    Statics.itemBtn = false;
                    itemScrollShow(false);
                    DotweenManager.Instance.DoLocalMoveY(100, 1, continuBtnPos);
                    continueParticle.SetActive(true);
                    continueBtn.interactable = true;
                    ShowPrevButton(false);
                    ShowNextButton(false);
                }
                else
                {
                    closet.ChangeScrollItem(gameStep + 1);
                }
                break;
            case GameStage.Accessory:
                if (gameStep == GameStep.Bag)
                {
                    Statics.itemBtn = false;
                    itemScrollShow(false);
                    DotweenManager.Instance.DoLocalMoveY(100, 1, continuBtnPos);
                    continueParticle.SetActive(true);
                    continueBtn.interactable = true;
                    ShowPrevButton(false);
                    ShowNextButton(false);
                }
                else if(gameStep == GameStep.Shirts)
                {
                    if (character.GetShirtsName() == "Costume")
                    {
                        Statics.itemBtn = false;
                        gameStep = GameStep.Earring;
                        clearStep = lastStep >= clearStep ? lastStep : GameStep.Earring;
                        gameStage = GameStage.Closet;
                        stepIcon.UpdateStatIcon(gameStep);
                        itemScrollShow(false);
                        DotweenManager.Instance.DoLocalMoveY(100, 1, continuBtnPos);
                        continueParticle.SetActive(true);
                        continueBtn.interactable = true;
                        ShowPrevButton(false);
                        ShowNextButton(false);
                        UserInfoUpdate();
                        return;
                    }
                }
                else if(gameStep == GameStep.FaceAc)
                {
                    DotweenManager.Instance.DoSizeImage(1.0f, 0.8f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(970.0f, 1, character.GetComponent<RectTransform>());
                    closet.ChangeScrollItem(gameStep + 1);
                }
                else if(gameStep == GameStep.Bracelet)
                {
                    DotweenManager.Instance.DoSizeImage(0.8f, 0.45f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(1002, 1, character.GetComponent<RectTransform>());
                    closet.ChangeScrollItem(gameStep + 1);
                }
                else
                {
                    closet.ChangeScrollItem(gameStep + 1);
                }
                break;
            case GameStage.Bg:
                cameraScreen.SetActive(true);
                DotweenManager.Instance.DoLocalMoveY(212, 1, screenShotBtnPos);
                itemScrollShow(false);
                break;
        }

        if(clearStep > gameStep)
        {
            gameStep++;
        }
        if(gameStage != GameStage.Wash)
        {
            UpdateGameStep();
        }
        stepIcon.UpdateStatIcon(gameStep);

        if (clearStep > gameStep && gameStep != GameStep.BackHair && gameStep != GameStep.Earring && gameStep != GameStep.Bg)
        {
            ShowNextButton(true);
        }
        else
        {
            ShowNextButton(false);
        }
    }

    public void PrevBtn()
    {
        selectedCloset = ClosetKind.None;
        SoundManager.Instance.OnClickSoundEffect();
        switch (gameStage)
        {
            case GameStage.Wash:
                break;
            case GameStage.Beauty:
                if (gameStep == GameStep.Eyes)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_04");
                    ShowPrevButton(false);
                }
                if (gameStep == GameStep.Eyesbrow)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_05");
                }
                if (gameStep == GameStep.Mouth)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_03");
                }
                break;
            case GameStage.Closet:

                if(gameStep == GameStep.Socks)
                {
                    if(character.GetShirtsName() == "Dress")
                    {
                        gameStep = GameStep.Shirts;
                        UserInfoUpdate();
                        stepIcon.UpdateStatIcon(gameStep);
                        closet.ChangeScrollItem(gameStep);
                        return;
                    }
                }
                if (gameStep == GameStep.BackHair)
                {
                    SoundManager.Instance.OnPlayOneShot("ve_07");
                    gameStage = GameStage.Beauty;
                    bgStage.SetUpDefaultBg(gameStage);
                    DotweenManager.Instance.DoSizeImage(0.45f, 1.3f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(466f, 1, character.GetComponent<RectTransform>());
                }
                break;
            case GameStage.Accessory:

                if (gameStep == GameStep.Earring)
                {
                    gameStage = GameStage.Closet;
                    DotweenManager.Instance.DoSizeImage(1.0f, 0.45f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(1002f, 1, character.GetComponent<RectTransform>());
                    if (character.GetShirtsName() == "Costume")
                    {
                        gameStep = GameStep.Shirts;
                        bgStage.SetUpDefaultBg(gameStage);
                        UserInfoUpdate();
                        stepIcon.UpdateStatIcon(gameStep);
                        closet.ChangeScrollItem(gameStep);
                        ShowNextButton(true);
                        return;
                    }
                }
                else if(gameStep == GameStep.Bracelet)
                {
                    DotweenManager.Instance.DoSizeImage(0.8f, 1.0f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(695, 1, character.GetComponent<RectTransform>());
                }
                else if (gameStep == GameStep.Pet)
                {
                    DotweenManager.Instance.DoSizeImage(0.45f, 0.8f, 1, character.gameObject);
                    DotweenManager.Instance.DoLocalMoveY(970f, 1, character.GetComponent<RectTransform>());
                }

                break;
            case GameStage.Bg:
                if(gameStep == GameStep.Bg)
                { 
                    gameStage = GameStage.Accessory;
                    bgStage.SetUpDefaultBg(gameStage);
                    stepIcon.ShowStepIconObj(true);
                }
                else if(gameStep == GameStep.Photo)
                {
                    bgStage.Init();
                    cameraScreen.SetActive(false);
                    DotweenManager.Instance.DoLocalMoveY(-300, 1, screenShotBtnPos);
                    gameStep--;
                    ShowNextButton(true);
                    UserInfoUpdate();
                    return;
                }
                break;
        }

        gameStep--;
        UserInfoUpdate();
        stepIcon.UpdateStatIcon(gameStep);
        closet.ChangeScrollItem(gameStep);
        ShowNextButton(true);
    }

    bool enableBtnTime = false;
    public void ShowNextButton(bool showBtn)
    {
        if (showBtn)
        {
            if(!enableBtnTime)
            {
                DotweenManager.Instance.DoLocalMoveX(-36, 1, nextBtnPos);
                enableBtnTime = true;
                StartCoroutine(EnableBtnWatiTime());
            }
            nextBtn.interactable = true;
        }
        else if (!showBtn)
        {
            DotweenManager.Instance.DoLocalMoveX(300, 1, nextBtnPos);
            nextBtn.interactable = false;
        }
    }

    IEnumerator EnableBtnWatiTime()
    {
        yield return new WaitForSeconds(1.0f);
        enableBtnTime = false;
    }

    public void ShowPrevButton(bool showBtn)
    {
        if (showBtn)
        {
            DotweenManager.Instance.DoLocalMoveX(36, 1, prevBtnPos);
            prevBtn.interactable = true;
        }
        else if (!showBtn)
        {
            DotweenManager.Instance.DoLocalMoveX(-300, 1, prevBtnPos);
            prevBtn.interactable = false;
        }
    }

    public void ContinueBtn()
    {
        Statics.itemBtn = true;
        SoundManager.Instance.OnClickSoundEffect();
        gameStage++;
        if (gameStage == GameStage.Beauty)
        {
            SoundManager.Instance.OnPlayOneShot("ve_04");
            DotweenManager.Instance.DoSizeImage(0.45f, 1.3f, 1, character.gameObject);
            DotweenManager.Instance.DoLocalMoveY(531f, 1, character.GetComponent<RectTransform>());
            closet.BeautyInit();
        }
        else if (gameStage == GameStage.Closet)
        {
            ShowPrevButton(true);
            closet.ClosetInit();
        }
        else if (gameStage == GameStage.Accessory)
        {
            ShowPrevButton(true);
            DotweenManager.Instance.DoSizeImage(0.45f, 1.0f, 1, character.gameObject);
            DotweenManager.Instance.DoLocalMoveY(695, 1, character.GetComponent<RectTransform>());
            closet.AccessoryInit();
        }
        else if (gameStage == GameStage.Bg)
        {
            ShowPrevButton(true);
            stepIcon.ShowStepIconObj(false);
            bgStage.Init();   
        }

        if(playerUserInfo.playingStep < playerUserInfo.clearStep)
        {
            ShowNextButton(true);
        }

        UserInfoUpdate();
        stepIcon.UpdateStatIcon(gameStep);
        bgStage.SetUpDefaultBg(gameStage);
        DotweenManager.Instance.DoLocalMoveY(-543, 1, continuBtnPos);
        continueParticle.SetActive(false);
        continueBtn.interactable = false;
    }

    //저장하기 버튼
    public void ScreenShotBtn()
    {
        preSetCharacter[0].gameObject.SetActive(true);
        preSetCharacter[1].gameObject.SetActive(true);
        SoundManager.Instance.OnPlayOneShot("ve_08");
        //진동 기능 삭제_230823 박진범
        //if (playerUserInfo.optionData.vibration)
        //{
        //    #if UNITY_ANDROID || UNITY_IOS
        //    Handheld.Vibrate();
        //    #endif
        //}
        ShowPrevButton(false);
        cameraScreen.SetActive(false);
        DotweenManager.Instance.DoLocalMoveY(100, 1, playBtnPos);
        DotweenManager.Instance.DoLocalMoveY(-300, 1, screenShotBtnPos);
        saveBehaviour.CharacterSave(character, wash.paint, bgStage, true);
        
        ClearStage();
    }

    public void ClearStage()
    {
        character.likeChat.SetActive(true);
        if (playerUserInfo.clearCharacterCount == playerUserInfo.playingCharacter)
        {
            Statics.clearCharacterCount++;
            playerUserInfo.clearCharacterCount++;
            if (Statics.clearCharacterCount >= Statics.maxCharacterCount)
            {
                Statics.clearCharacterCount = Statics.maxCharacterCount;
                playerUserInfo.clearCharacterCount = Statics.maxCharacterCount;
            }
        }
        clearPlayableDirector.Play();

        MainSceneController.Instance.ShowAppReviewUI();
    }

    //캐릭터 꾸미기 완료후 플레이버_다음캐릭터로 게임 진행
    public void PlayBtn()
    {
        SoundManager.Instance.OnClickSoundEffect();
        //27개의 캐릭터가 최대 생성 가능
        if (playerUserInfo.playingCharacter >= Statics.maxCharacterCount)
        {
            Statics.clearCharacterCount = Statics.maxCharacterCount;
            playerUserInfo.clearCharacterCount = Statics.maxCharacterCount;
            MainSceneController.Instance.ShowAlarmUI(LocalizationManager.GetTermTranslation("WaitNextVersionMessage"));
        }
        else
        { 
            Statics.selectedCharacter++;
            DotweenManager.Instance.DoLocalMoveY(-300, 1, playBtnPos);
            LoadingPanel.Instance.Show(() =>
            {
                Init();
                LoadingPanel.Instance.Hide();
            });
            LoadingPanel.Instance.End();
        }
    }
}
