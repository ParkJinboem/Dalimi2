using System.Collections;
using UnityEngine;
using TMPro;

public class LikeChat : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public int number;

    public void RadomValue()
    {
        StopAllCoroutines();
        number = Random.Range(90, 140);
        numberText.text = number.ToString();
    }


    public void clicked()
    {
        Debug.Log("aaaaa");
    }
}
