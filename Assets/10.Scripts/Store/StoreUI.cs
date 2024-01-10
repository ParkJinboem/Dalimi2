using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void ExitBtn()
    {
        this.gameObject.SetActive(false);
    }

}
