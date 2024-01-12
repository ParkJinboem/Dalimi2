using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Character : MonoBehaviour
{
    Dictionary<Image, ClosetKind> characterPartDictionary = new Dictionary<Image, ClosetKind>();

    [Header("BaseCharacterSprite")]
    public Image body;
    public Image shirts;
    public Image pants;
    public Image backHair;
    public Image eyes;
    public Image eyebrows;
    public Image mouth;
    [Header("CharacterSprite")]
    public Image socks;
    public Image shoes;
    public Image headDress;
    public Image glasses;
    public Image earring;
    public Image necklace;
    public Image bracelet;
    public Image faceAc;
    public Image pet;
    public Image bag;
    [Header("CharacterInfo")]
    public int saveId;
    public int characterId;
    public GameObject likeChat;
    [Header("Particle")]
    public GameObject changePartParticle;
    public bool particlePlay;

    //메인신에서 캐릭터를 생성
    public Character CreateCharacter(int characterId)
    {
        List<ClosetPresetData> closetPresetDatas = DataManager.Instance.GetClosetPresetDataWithId(characterId);
        foreach (var item in closetPresetDatas)
        {
            ChangeCloset(item.closetId);
        }
        return this;
    }

    public void Init()
    {
        ResetCharacter();
        CreateDirtyCharacter(Statics.selectedCharacter);
        ContinueCharacterPos(42.0f, 466f, 0.0f, 1.3f, 1.3f, 1.0f);
        characterId = Statics.selectedCharacter;
        likeChat.SetActive(false);
    }

    //게임시작시 더티헤어로 변경
    public void CreateDirtyCharacter(int characterId)
    {
        List<ClosetPresetData> closetPresetDatas = DataManager.Instance.GetClosetPresetDataWithId(characterId);
        foreach (var item in closetPresetDatas)
        {
            if (item.kind == ClosetKind.BackHair)
            {
                int backHairId = closetPresetDatas.Find(x => x.kind == ClosetKind.BackHair).closetId;
                ClosetData dirtyHairData = DataManager.Instance.GetClosetDataWithId(DataManager.Instance.GetClosetDataWithId(backHairId).linkId);
                backHair.sprite = DataManager.Instance.GetCharacterPartSprite(dirtyHairData.name);
                backHair.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(dirtyHairData.posX, -dirtyHairData.posY);
                backHair.SetNativeSize();
            }
            else
            {
                ChangeCloset(item.closetId);
            }
        }
    }

    //1단계완료시 더티헤어서 정상머리로 변경
    public void ChangeHair(int characterId)
    {
        List<ClosetPresetData> closetPresetDatas = DataManager.Instance.GetClosetPresetDataWithId(characterId);
        int closetId = closetPresetDatas.Find(x => x.kind == ClosetKind.BackHair).closetId;
        ClosetData closetData = DataManager.Instance.GetClosetDataWithId(closetId);
        backHair.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
        backHair.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
        backHair.SetNativeSize();
    }

    //좋아요 캐릭터 생성
    public void CreateLikeChatCharacter()
    {
        List<ClosetData> closetPresetDatas = DataManager.Instance.GetClosetDataWithKind(ClosetKind.Costume);
        int randomCount = Random.Range(0, closetPresetDatas.Count);
        int closetId = closetPresetDatas[randomCount].id;
        ClosetData closetData = DataManager.Instance.GetClosetDataWithId(closetId);
        shirts.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
        shirts.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
        shirts.SetNativeSize();
        likeChat.SetActive(true);
        likeChat.GetComponent<LikeChat>().RadomValue();
    }

    public void ChangeCloset(int closetId)
    {
        ClosetData closetData = DataManager.Instance.GetClosetDataWithId(closetId);
        ClosetKind resetClosetKind = closetData.kind;

        if (characterPartDictionary.Count == 0)
        {
            AddpartDictionary();
        }

        if(closetData.kind == ClosetKind.Dress || closetData.kind == ClosetKind.Costume)
        {
            closetData.kind = ClosetKind.Shirts;
        }

        if (characterPartDictionary.ContainsValue(closetData.kind))
        {
            if(closetData.kind == ClosetKind.Cheek)
            {
                return;
            }
            Image partObj = characterPartDictionary.FirstOrDefault(x => x.Value == closetData.kind).Key;
            partObj.sprite = DataManager.Instance.GetCharacterPartSprite(closetData.name);
            partObj.GetComponent<Image>().SetNativeSize();
            partObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(closetData.posX, -closetData.posY);
        }

        if(closetData.kind == ClosetKind.Shirts)
        {
            closetData.kind = resetClosetKind;
        }

        if (particlePlay)
        {
            ParticlePlay(closetData.kind);
        }
    }

    //드레스나 코스튭 입을시 바지를 제거
    public void RemoveSamePart(int closetId)
    {
        ClosetData closetData = DataManager.Instance.GetClosetDataWithId(closetId);
        switch (closetData.kind)
        {
            case ClosetKind.Dress:
            case ClosetKind.Costume:
                pants.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
                break;
        }
    }

    //상의 입을시 하의가 없다면 기본하의를 입혀줌
    public void ShowDefaultPants()
    {
        if (pants.sprite.name == "NullImage(Clone)")
        {
            pants.sprite = DataManager.Instance.GetCharacterPartSprite("Pants_000");
            pants.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(382, -1109);
            pants.SetNativeSize();
        }
    }

    public string GetShirtsName()
    {
        string[] spriteName = shirts.sprite.name.Split('_');
        return spriteName[0];
    }

    public void ParticlePlay(ClosetKind closetKind)
    {
        if (closetKind == ClosetKind.Eyes)
        {
            eyes.transform.GetChild(0).GetComponentInChildren<ParticleSystem>().Play();
            eyes.transform.GetChild(1).GetComponentInChildren<ParticleSystem>().Play();
        }
        else if (closetKind == ClosetKind.Eyesbrow)
        {
            eyebrows.transform.GetChild(0).GetComponentInChildren<ParticleSystem>().Play();
            eyebrows.transform.GetChild(1).GetComponentInChildren<ParticleSystem>().Play();
        }
        else if (closetKind == ClosetKind.Mouth)
        {
            mouth.transform.GetChild(0).GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            GameObject particleObj = Instantiate(changePartParticle, this.transform);
            Vector3 SpritePos;
            if (characterPartDictionary.ContainsValue(closetKind))
            {
                if (closetKind == ClosetKind.Cheek)
                {
                    return;
                }
                GameObject partObj = characterPartDictionary.FirstOrDefault(x => x.Value == closetKind).Key.gameObject;
                SpritePos = new Vector3(partObj.GetComponent<Image>().sprite.pivot.x, -partObj.GetComponent<Image>().sprite.pivot.y, 0);
                particleObj.transform.localPosition = partObj.transform.localPosition + SpritePos;
                //네임 스페이스 에러_240110 박진범
                //particleObj.GetComponent<Coffee.UIExtensions.UIParticle>().scale = 500.0f;
            }
        }
        particlePlay = false;
    }

    //기본 파트 외에는 모두 투명이미지 처리
    public void ResetCharacter()
    {
        pants.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        socks.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        shoes.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        headDress.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        glasses.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        earring.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        necklace.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        bracelet.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        pet.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        faceAc.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
        bag.sprite = DataManager.Instance.GetCharacterPartSprite("NullImage");
    }

    private void AddpartDictionary()
    {
        characterPartDictionary.Add(body, ClosetKind.Body);
        characterPartDictionary.Add(shirts, ClosetKind.Shirts);
        characterPartDictionary.Add(pants, ClosetKind.Pants);
        characterPartDictionary.Add(backHair, ClosetKind.BackHair);
        characterPartDictionary.Add(eyes, ClosetKind.Eyes);
        characterPartDictionary.Add(eyebrows, ClosetKind.Eyesbrow);
        characterPartDictionary.Add(mouth, ClosetKind.Mouth);
        characterPartDictionary.Add(socks, ClosetKind.Socks);
        characterPartDictionary.Add(shoes, ClosetKind.Shoes);
        characterPartDictionary.Add(headDress, ClosetKind.HeadDress);
        characterPartDictionary.Add(glasses, ClosetKind.Glasses);
        characterPartDictionary.Add(earring, ClosetKind.Earring);
        characterPartDictionary.Add(necklace, ClosetKind.Neckless);
        characterPartDictionary.Add(bracelet, ClosetKind.Bracelet);
        characterPartDictionary.Add(faceAc, ClosetKind.FaceAc);
        characterPartDictionary.Add(pet, ClosetKind.Pet);
        characterPartDictionary.Add(bag, ClosetKind.Bag);   
    }

    public void ContinueCharacterPos(float posX, float posY, float posZ, float sizeX, float sizeY, float sizeZ)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, posY, posZ);
        GetComponent<RectTransform>().localScale = new Vector3(sizeX, sizeY, sizeZ);
    }
}