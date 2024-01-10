using I2.Loc;
using UnityEngine;

public class NextVersionAlarm : MonoBehaviour
{
    private void OnEnable()
    {
        TMPro.TextMeshProUGUI textMesh = transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        textMesh.text = LocalizationManager.GetTermTranslation("WaitNextVersionMessage");
    }
}
