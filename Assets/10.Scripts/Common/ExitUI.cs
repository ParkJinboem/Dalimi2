using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : MonoBehaviour
{
    public void ClickedYes()
    {
        MainSceneController.Instance.ShowMainScene();
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    public void ClickedNo()
    {
        SoundManager.Instance.OnClickSoundEffect();
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
}
