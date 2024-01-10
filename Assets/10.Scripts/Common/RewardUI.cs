using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    public Image itmeImage;

    public void Init(Sprite itemsprite)
    {
        itmeImage.sprite = itemsprite;
    }

    public void ClickedOkBtn()
    {
        Destroy(gameObject);
    }
}
