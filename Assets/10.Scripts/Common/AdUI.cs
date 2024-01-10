using System;
using UnityEngine;
using UnityEngine.UI;

public class AdUI : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action callback;

    private void Awake()
    {
        yesButton.onClick.AddListener(ClickYes);
        noButton.onClick.AddListener(ClickNo);
    }

    public void Init(Action callback)
    {
        this.callback = callback;
    }

    public void ClickYes()
    {
        SoundManager.Instance.OnClickSoundEffect();
        callback?.Invoke();
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
