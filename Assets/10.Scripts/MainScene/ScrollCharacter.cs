using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCharacter : MonoBehaviour
{

    [Header("BaseCharacterSprite")]
    public Image body;
    public Image shirts;
    public Image backHair;
    public Image eyes;
    public Image eyebrows;
    public Image mouth;

    public ScrollCharacter CreateCharacter(int characterId)
    {
        List<ClosetPresetData> closetPresetDatas = DataManager.Instance.GetClosetPresetDataWithId(characterId);
        foreach (var item in closetPresetDatas)
        {
            ChangeScrollCharacterCloset(item.closetId);
        }
        //this.characterId = characterId;
        return this;
    }

    public void ChangeScrollCharacterCloset(int closetId)
    {
        ClosetData closetData = DataManager.Instance.GetClosetDataWithId(closetId);
        switch(closetData.kind)
        {
            case ClosetKind.Body:
                body.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
                body.GetComponent<Image>().SetNativeSize();
                body.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
                break;
            case ClosetKind.Shirts:
                shirts.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
                shirts.GetComponent<Image>().SetNativeSize();
                shirts.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
                break;
            case ClosetKind.BackHair:
                backHair.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
                backHair.GetComponent<Image>().SetNativeSize();
                backHair.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
                break;
            case ClosetKind.Eyes:
                eyes.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
                eyes.GetComponent<Image>().SetNativeSize();
                eyes.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
                break;
            case ClosetKind.Eyesbrow:
                eyebrows.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
                eyebrows.GetComponent<Image>().SetNativeSize();
                eyebrows.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
                break;
            case ClosetKind.Mouth:
                mouth.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
                mouth.GetComponent<Image>().SetNativeSize();
                mouth.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
                break;
        }
    }

    //클리어한 캐릭터의 다음캐릭터는 검정색 음영으로 처리
    public ScrollCharacter BlackCharacter()
    {
        body.color = new Color(0, 0, 0, 255);
        shirts.color = new Color(0, 0, 0, 255);
        backHair.color = new Color(0, 0, 0, 255);
        eyes.color = new Color(0, 0, 0, 255);
        eyebrows.color = new Color(0, 0, 0, 255);
        mouth.color = new Color(0, 0, 0, 255);
        return this;
    }
    //검정처리안한 캐릭터는 다시 흰색으로 변경
    public ScrollCharacter WhiteCharacter()
    {
        body.color = new Color(255, 255, 255, 255);
        shirts.color = new Color(255, 255, 255, 255);
        backHair.color = new Color(255, 255, 255, 255);
        eyes.color = new Color(255, 255, 255, 255);
        eyebrows.color = new Color(255, 255, 255, 255);
        mouth.color = new Color(255, 255, 255, 255);
        return this;
    }
}
