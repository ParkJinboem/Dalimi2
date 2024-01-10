using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotCharacter : Character
{
    [Header("ScreenShotCharacter")]
    public RawImage cheekImage;
    public Image bgImage;


    public void Init(AlbumCharacterSlot albumCharacter)
    {
        
        backHair.sprite = albumCharacter.backHair.sprite;
        backHair.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.backHair.GetComponent<RectTransform>().anchoredPosition;
        backHair.SetNativeSize();

        body.sprite = albumCharacter.body.sprite;
        body.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.body.GetComponent<RectTransform>().anchoredPosition;
        body.SetNativeSize();

        eyebrows.sprite = albumCharacter.eyebrows.sprite;
        eyebrows.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.eyebrows.GetComponent<RectTransform>().anchoredPosition;
        eyebrows.SetNativeSize();

        eyes.sprite = albumCharacter.eyes.sprite;
        eyes.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.eyes.GetComponent<RectTransform>().anchoredPosition;
        eyes.SetNativeSize();

        cheekImage.texture = albumCharacter.cheekImage.texture;

        mouth.sprite = albumCharacter.mouth.sprite;
        mouth.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.mouth.GetComponent<RectTransform>().anchoredPosition;
        mouth.SetNativeSize();

        shirts.sprite = albumCharacter.shirts.sprite;
        shirts.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.shirts.GetComponent<RectTransform>().anchoredPosition;
        shirts.SetNativeSize();

        pants.sprite = albumCharacter.pants.sprite;
        pants.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.pants.GetComponent<RectTransform>().anchoredPosition;
        pants.SetNativeSize();

        socks.sprite = albumCharacter.socks.sprite;
        socks.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.socks.GetComponent<RectTransform>().anchoredPosition;
        socks.SetNativeSize();

        shoes.sprite = albumCharacter.shoes.sprite;
        shoes.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.shoes.GetComponent<RectTransform>().anchoredPosition;
        shoes.SetNativeSize();

        necklace.sprite = albumCharacter.necklace.sprite;
        necklace.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.necklace.GetComponent<RectTransform>().anchoredPosition;
        necklace.SetNativeSize();

        headDress.sprite = albumCharacter.headDress.sprite;
        headDress.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.headDress.GetComponent<RectTransform>().anchoredPosition;
        headDress.SetNativeSize();

        earring.sprite = albumCharacter.earring.sprite;
        earring.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.earring.GetComponent<RectTransform>().anchoredPosition;
        earring.SetNativeSize();

        glasses.sprite = albumCharacter.glasses.sprite;
        glasses.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.glasses.GetComponent<RectTransform>().anchoredPosition;
        glasses.SetNativeSize();

        bracelet.sprite = albumCharacter.bracelet.sprite;
        bracelet.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.bracelet.GetComponent<RectTransform>().anchoredPosition;
        bracelet.SetNativeSize();

        pet.sprite = albumCharacter.pet.sprite;
        pet.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.pet.GetComponent<RectTransform>().anchoredPosition;
        pet.SetNativeSize();

        faceAc.sprite = albumCharacter.faceAc.sprite;
        faceAc.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.faceAc.GetComponent<RectTransform>().anchoredPosition;
        faceAc.SetNativeSize();

        bag.sprite = albumCharacter.bag.sprite;
        bag.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.bag.GetComponent<RectTransform>().anchoredPosition;
        bag.SetNativeSize();

        string[] bgNmae = albumCharacter.bgImage.sprite.name.Split('(');
        bgImage.sprite = DataManager.Instance.GetBackGroundSprite(bgNmae[0]);
    }
}
