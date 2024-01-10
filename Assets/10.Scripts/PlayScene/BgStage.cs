using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgStage : MonoBehaviour
{
    public Sprite[] defaultBg;
    public Image backgroundImage;
    public string selectedBackGround;
    [SerializeField] private ItemScroll itemScroll;

    public void Init()
    {
        itemScroll.BackGroundInit(BackgroundKind.Bg);
        PlayManager.Instance.itemScrollShow(true);
    }

    public void ChangeBackGround(int itemId)
    {
        string backGroundName = DataManager.Instance.GetBackGroundDataWithId(itemId).name;
        backgroundImage.sprite = DataManager.Instance.GetBackGroundSprite(backGroundName);
        selectedBackGround = backgroundImage.sprite.name.Split('(')[0];
        PlayerDataManager.Instance.s.userinfo.backGroundName = selectedBackGround;
        PlayerDataManager.Instance.SaveData();
    }

    public void SetUpDefaultBg(GameStage gameStage)
    {
        switch (gameStage)
        {
            case GameStage.Wash:
                backgroundImage.sprite = defaultBg[0];
                break;
            case GameStage.Beauty:
                backgroundImage.sprite = defaultBg[1];
                break;
            case GameStage.Closet:
            case GameStage.Accessory:
                backgroundImage.sprite = defaultBg[2];
                break;
            case GameStage.Bg:
                if (selectedBackGround == "")
                {
                    if (PlayerDataManager.Instance.s.userinfo.backGroundName == "")
                    {
                        backgroundImage.sprite = defaultBg[2];
                    }
                    else
                    { 
                        backgroundImage.sprite = DataManager.Instance.GetBackGroundSprite(PlayerDataManager.Instance.s.userinfo.backGroundName);
                    }
                }
                else
                {
                    backgroundImage.sprite = DataManager.Instance.GetBackGroundSprite(selectedBackGround);
                }
                break;
        }
    }

    public string GetSelectedBackGround()
    {
        return selectedBackGround;
    }
}
