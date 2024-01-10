using UnityEngine;
using TMPro;

public class AlarmUI : MonoBehaviour
{
    public TextMeshProUGUI alarmText;

    public void Init(string text)
    {
        alarmText.text = text;
    }

    public void ClickedConfirm()
    {
        Destroy(gameObject);
    }
}
