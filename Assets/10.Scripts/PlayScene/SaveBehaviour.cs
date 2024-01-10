using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBehaviour : MonoBehaviour
{
    SaveCharacter saveCharacter;
    public void CharacterSave(Character character, AdvancedMobilePaint.AdvancedMobilePaint cheekPaint, BgStage bgStage, bool trueSave)
    {
        int saveId = character.saveId;
        bool isHave = PlayerDataManager.Instance.IsHaveSaveCharacter(saveId);
        if (isHave)
        {
            saveCharacter = PlayerDataManager.Instance.GetSaveCharacterWithId(saveId);
        }
        else
        {
            saveCharacter = new SaveCharacter();
        }
        
        saveCharacter.saveId = saveId;
        saveCharacter.characterId = character.characterId;
        saveCharacter.backHairId = DataManager.Instance.GetClosetDataWithName(character.backHair.sprite.name.Split('(')[0]).id;
        saveCharacter.bodyId = DataManager.Instance.GetClosetDataWithName(character.body.sprite.name.Split('(')[0]).id;
        saveCharacter.eyesbrowId = DataManager.Instance.GetClosetDataWithName(character.eyebrows.sprite.name.Split('(')[0]).id;
        saveCharacter.eyesId = DataManager.Instance.GetClosetDataWithName(character.eyes.sprite.name.Split('(')[0]).id;
        if (cheekPaint.patternBrushBytes.Length != 0)
        {
            saveCharacter.cheekName = cheekPaint.pattenTexture.name;
        }
        saveCharacter.mouthId = DataManager.Instance.GetClosetDataWithName(character.mouth.sprite.name.Split('(')[0]).id;
        saveCharacter.shirtsId = DataManager.Instance.GetClosetDataWithName(character.shirts.sprite.name.Split('(')[0]).id;
        saveCharacter.pantsId = DataManager.Instance.GetClosetDataWithName(character.pants.sprite.name.Split('(')[0]).id;
        saveCharacter.socksId = DataManager.Instance.GetClosetDataWithName(character.socks.sprite.name.Split('(')[0]).id;
        saveCharacter.shoesId = DataManager.Instance.GetClosetDataWithName(character.shoes.sprite.name.Split('(')[0]).id;
        saveCharacter.necklaceId = DataManager.Instance.GetClosetDataWithName(character.necklace.sprite.name.Split('(')[0]).id;
        saveCharacter.headDressId = DataManager.Instance.GetClosetDataWithName(character.headDress.sprite.name.Split('(')[0]).id;
        saveCharacter.earringId = DataManager.Instance.GetClosetDataWithName(character.earring.sprite.name.Split('(')[0]).id;
        saveCharacter.glassesId = DataManager.Instance.GetClosetDataWithName(character.glasses.sprite.name.Split('(')[0]).id;
        saveCharacter.faceAcId = DataManager.Instance.GetClosetDataWithName(character.faceAc.sprite.name.Split('(')[0]).id;
        saveCharacter.braceletId = DataManager.Instance.GetClosetDataWithName(character.bracelet.sprite.name.Split('(')[0]).id;
        saveCharacter.petId = DataManager.Instance.GetClosetDataWithName(character.pet.sprite.name.Split('(')[0]).id;
        saveCharacter.bagId = DataManager.Instance.GetClosetDataWithName(character.bag.sprite.name.Split('(')[0]).id;
        saveCharacter.bgName = bgStage.backgroundImage.sprite.name.Split('(')[0];
        saveCharacter.trueSave = trueSave;
        PlayerDataManager.Instance.AddSaveCharacter(saveCharacter);
    }
}
