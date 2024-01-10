using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Album : MonoBehaviour
{
    public AlbumSceneManager albumSceneManager;
    public AlbumCharacterSlot albumCharacter;
    public ScreenShot screenShot;
    public GameObject popDeleteObj;
    public int slotId;

    private void OnEnable()
    {
        AdsManager.Instance.SetLockAppOpen(true);
    }

    private void OnDisable()
    {
        AdsManager.Instance.SetLockAppOpen(false);
    }

    public void Init(AlbumCharacterSlot albumCharacter)
    {
        CharacterInit(albumCharacter);
        slotId = albumCharacter.slotId;
        screenShot.screenShotCharacter.Init(albumCharacter);
    }

    public void ExitClicked()
    {
        SoundManager.Instance.OnClickSoundEffect();
        gameObject.SetActive(false);
    }

    public void DeleteClicked()
    {
        SoundManager.Instance.OnClickSoundEffect();
        popDeleteObj.SetActive(true);
    }

    public void CharacterInit(AlbumCharacterSlot albumCharacter)
    {
        this.albumCharacter.backHair.sprite = albumCharacter.backHair.sprite;
        this.albumCharacter.backHair.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.backHair.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.backHair.SetNativeSize();

        this.albumCharacter.body.sprite = albumCharacter.body.sprite;
        this.albumCharacter.body.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.body.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.body.SetNativeSize();

        this.albumCharacter.eyebrows.sprite = albumCharacter.eyebrows.sprite;
        this.albumCharacter.eyebrows.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.eyebrows.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.eyebrows.SetNativeSize();

        this.albumCharacter.eyes.sprite = albumCharacter.eyes.sprite;
        this.albumCharacter.eyes.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.eyes.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.eyes.SetNativeSize();

        this.albumCharacter.cheekImage.texture = albumCharacter.cheekImage.texture;

        this.albumCharacter.mouth.sprite = albumCharacter.mouth.sprite;
        this.albumCharacter.mouth.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.mouth.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.mouth.SetNativeSize();

        this.albumCharacter.shirts.sprite = albumCharacter.shirts.sprite;
        this.albumCharacter.shirts.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.shirts.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.shirts.SetNativeSize();

        this.albumCharacter.pants.sprite = albumCharacter.pants.sprite;
        this.albumCharacter.pants.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.pants.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.pants.SetNativeSize();

        this.albumCharacter.socks.sprite = albumCharacter.socks.sprite;
        this.albumCharacter.socks.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.socks.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.socks.SetNativeSize();

        this.albumCharacter.shoes.sprite = albumCharacter.shoes.sprite;
        this.albumCharacter.shoes.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.shoes.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.shoes.SetNativeSize();

        this.albumCharacter.necklace.sprite = albumCharacter.necklace.sprite;
        this.albumCharacter.necklace.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.necklace.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.necklace.SetNativeSize();

        this.albumCharacter.headDress.sprite = albumCharacter.headDress.sprite;
        this.albumCharacter.headDress.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.headDress.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.headDress.SetNativeSize();

        this.albumCharacter.earring.sprite = albumCharacter.earring.sprite;
        this.albumCharacter.earring.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.earring.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.earring.SetNativeSize();

        this.albumCharacter.glasses.sprite = albumCharacter.glasses.sprite;
        this.albumCharacter.glasses.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.glasses.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.glasses.SetNativeSize();

        this.albumCharacter.bracelet.sprite = albumCharacter.bracelet.sprite;
        this.albumCharacter.bracelet.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.bracelet.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.bracelet.SetNativeSize();

        this.albumCharacter.pet.sprite = albumCharacter.pet.sprite;
        this.albumCharacter.pet.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.pet.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.pet.SetNativeSize();

        this.albumCharacter.faceAc.sprite = albumCharacter.faceAc.sprite;
        this.albumCharacter.faceAc.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.faceAc.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.faceAc.SetNativeSize();

        this.albumCharacter.bag.sprite = albumCharacter.bag.sprite;
        this.albumCharacter.bag.gameObject.GetComponent<RectTransform>().anchoredPosition = albumCharacter.bag.GetComponent<RectTransform>().anchoredPosition;
        this.albumCharacter.bag.SetNativeSize();

        this.albumCharacter.bgImage.sprite = albumCharacter.bgImage.sprite;
    }

    public void DeleteYes()
    {
        SoundManager.Instance.OnClickSoundEffect();
        PlayerDataManager.Instance.DeleteCharacterData(slotId);
        int abc = albumSceneManager.panelParent.transform.childCount;
        for (int i = 0; i < abc; i++)
        {
            albumSceneManager.panelParent.GetChild(0).SetParent(albumSceneManager.destroyParent);
        }
        foreach (Transform child in albumSceneManager.destroyParent)
        {
            Destroy(child.gameObject);
        }
        albumSceneManager.AlbumSetUp();
        popDeleteObj.SetActive(false);
        gameObject.SetActive(false);
    }

    public void DeleteNo()
    {
        SoundManager.Instance.OnClickSoundEffect();
        popDeleteObj.SetActive(false);
    }
}
