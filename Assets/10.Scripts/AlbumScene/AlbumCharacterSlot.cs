using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AlbumCharacterSlot : Character
{
    [Header("AlbumCharacter")]
    [SerializeField] private List<Texture2D> cheekTexture;
    public int slotId;
    public RawImage cheekImage;
    public Image bgImage;
    private AlbumSceneManager albumSceneManager;

    [Header("ScreenShot")]
    [SerializeField] private Transform screenShotParent;

    public void Init(SaveCharacter saveCharacter)
    {
        partsSetting(saveCharacter);
        albumSceneManager = FindObjectOfType<AlbumSceneManager>();
    }

    public void BtnClicked()
    {
        SoundManager.Instance.OnPlayOneShot("se_01");
        albumSceneManager.albumPrefabs.SetActive(true);
        albumSceneManager.albumPrefabs.GetComponent<Album>().Init(this);
    }


    public void partsSetting(SaveCharacter saveCharacter)
    {
        Dictionary<Image, int> characterPart = new Dictionary<Image, int>();
        characterPart.Add(backHair, saveCharacter.backHairId);
        characterPart.Add(body, saveCharacter.bodyId);
        characterPart.Add(eyebrows, saveCharacter.eyesbrowId);
        characterPart.Add(eyes, saveCharacter.eyesId);
        characterPart.Add(mouth, saveCharacter.mouthId);
        characterPart.Add(shirts, saveCharacter.shirtsId);
        characterPart.Add(pants, saveCharacter.pantsId);
        characterPart.Add(socks, saveCharacter.socksId);
        characterPart.Add(shoes, saveCharacter.shoesId);
        characterPart.Add(necklace, saveCharacter.necklaceId);
        characterPart.Add(headDress, saveCharacter.headDressId);
        characterPart.Add(earring, saveCharacter.earringId);
        characterPart.Add(glasses, saveCharacter.glassesId);
        characterPart.Add(faceAc, saveCharacter.faceAcId);
        characterPart.Add(bracelet, saveCharacter.braceletId);
        characterPart.Add(pet, saveCharacter.petId);
        characterPart.Add(bag, saveCharacter.bagId);

        foreach (var item in characterPart)
        {
            ClosetData closetData = DataManager.Instance.GetClosetDataWithId(item.Value);
            item.Key.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
            item.Key.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, closetData.posY * -1);
            item.Key.SetNativeSize();
        }

        cheekImage.texture = cheekTexture.Find(x => x.name == saveCharacter.cheekName);
        bgImage.sprite = DataManager.Instance.GetBackGroundSprite(saveCharacter.bgName);
        slotId = saveCharacter.saveId;
    }
}
