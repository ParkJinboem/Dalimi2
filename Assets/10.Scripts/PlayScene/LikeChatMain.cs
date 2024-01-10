using System.Collections;
using UnityEngine;
using TMPro;

public class LikeChatMain : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public int number;

    private void OnEnable()
    {
        StartCoroutine(MainRandomValue());
    }

    IEnumerator MainRandomValue()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            number = Random.Range(141, 251);
            numberText.text = number.ToString();
            yield return null;
        }
    }
}
