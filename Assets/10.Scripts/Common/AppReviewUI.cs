using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppReviewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        #if PLATFORM_IOS
            titleText.text = LocalizationManager.GetTermTranslation("RequestReviewAppStore");
        #else
            titleText.text = LocalizationManager.GetTermTranslation("RequestReviewGooglePlay");
        #endif
        yesButton.onClick.AddListener(ClickYes);
        noButton.onClick.AddListener(ClickNo);
    }

    public void ClickYes()
    {
        SoundManager.Instance.OnClickSoundEffect();
        AppReviewManager.Instance.ShowAppReview();
        Hide();
    }

    public void ClickNo()
    {
        SoundManager.Instance.OnClickSoundEffect();
        Hide();
    }

    public void Hide()
    {
        Destroy(gameObject);
    }
}
