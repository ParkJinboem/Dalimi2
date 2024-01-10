using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueImage : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image logoImage;
    [SerializeField] private Sprite[] logoSprite;

    public void Init()
    {
        if (PlayerDataManager.Instance.language == "korean")
        {
            logoImage.sprite = logoSprite[1];
        }
        else if (PlayerDataManager.Instance.language == "english")
        {
            logoImage.sprite = logoSprite[0];
        }
        logoImage.SetNativeSize();
    }
}
