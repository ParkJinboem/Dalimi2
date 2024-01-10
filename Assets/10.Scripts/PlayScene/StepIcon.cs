using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepIcon : MonoBehaviour
{
    [Header("StatIcon")]
    [SerializeField] private GameObject stepIconObj;
    [SerializeField] private List<Image> stepIcon;
    [SerializeField] private Sprite[] stepSprite;

    public void ShowStepIconObj(bool show)
    {
        stepIconObj.SetActive(show);
    }

    public void UpdateStatIcon(GameStep gameStep)
    {
        GameStage gameStage = PlayManager.Instance.gameStage;
        Sprite iconSprite;
        if (stepSprite.Length > (int)gameStep)
        {
            iconSprite = stepSprite[(int)gameStep];
        }
        else
        {
            iconSprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
        }
        switch (gameStage)
        {
            case GameStage.Wash:
                if (gameStep == GameStep.Cheek) //넥스트 버튼 클릭시  볼터치단계
                {
                    stepIcon[0].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                }
                else
                {
                    stepIcon[0].sprite = iconSprite;
                    stepIcon[1].sprite = DataManager.Instance.GetMenuHolderSprite("07step_cheek_OFF");
                    stepIcon[2].sprite = DataManager.Instance.GetMenuHolderSprite("11step_BackHair_OFF");
                    stepIcon[3].sprite = DataManager.Instance.GetMenuHolderSprite("18step_Earring_OFF");
                }
                break;
            case GameStage.Beauty:
                if(gameStep == GameStep.BackHair) //넥스트 버튼 클릭시 프론트헤어 단계, 컨티뉴 클릭시 스테이지업
                {
                    stepIcon[1].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                }
                else if(gameStep == GameStep.Mouth)
                {
                    stepIcon[1].sprite = iconSprite;
                    stepIcon[2].sprite = DataManager.Instance.GetMenuHolderSprite("11step_BackHair_OFF");
                }
                else
                {
                    stepIcon[0].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                    stepIcon[1].sprite = iconSprite;
                }
                break;
            case GameStage.Closet:
                if (gameStep == GameStep.Earring) //넥스트 버튼 클릭시 프론트헤어 단계, 컨티뉴 클릭시 스테이지업
                {
                    stepIcon[2].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                }
                else if(gameStep == GameStep.HeadDress || gameStep == GameStep.Shirts)
                {
                    stepIcon[2].sprite = iconSprite;
                    stepIcon[3].sprite = DataManager.Instance.GetMenuHolderSprite("18step_Earring_OFF");
                }
                else
                {
                    stepIcon[0].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                    stepIcon[1].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                    stepIcon[2].sprite = iconSprite;
                }
                break;
            case GameStage.Accessory:
                if (gameStep == GameStep.Bg) //넥스트 버튼 클릭시 프론트헤어 단계, 컨티뉴 클릭시 스테이지업
                {
                    stepIcon[3].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                }
                else
                {
                    stepIcon[0].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                    stepIcon[1].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                    stepIcon[2].sprite = DataManager.Instance.GetMenuHolderSprite(Statics.stepIconClearMark);
                    stepIcon[3].sprite = iconSprite;
                }
                break;
            case GameStage.Bg:
                stepIconObj.SetActive(false);
                break;
        }

        //스탭 아이콘 사이즈 변경
        for (int i = 0; i < stepIcon.Count; i++)
        {
            stepIcon[i].SetNativeSize();
        }
    }
}
