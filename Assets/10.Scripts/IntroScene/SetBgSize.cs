using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBgSize : MonoBehaviour
{
    public Image bgImage;
    public float imageX;
    public float imageY;
    public float ratioX;
    public float ratioY;
    // Start is called before the first frame update
    void Start()
    {
        bgImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 1920);

        imageX = 1080.0f;
        imageY = 1920.0f;
        ratioX = Screen.width / imageX;
        ratioY = Screen.height / imageY;
        float value = ratioX > ratioY ? ratioX : ratioY;
        bgImage.GetComponent<RectTransform>().sizeDelta = new Vector2(bgImage.GetComponent<RectTransform>().sizeDelta.x * value, bgImage.GetComponent<RectTransform>().sizeDelta.y * value);
        //bgImage.GetComponent<RectTransform>().localScale = new Vector3(value, value, value);
    }
}
