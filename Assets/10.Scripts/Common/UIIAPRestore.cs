using OnDot.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIIAPRestore : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Button button;
    [SerializeField] private UnityEvent onRestored;

    private void Awake()
    {
        button.onClick.RemoveListener(Restore);
        button.onClick.AddListener(Restore);
    }

    private void OnEnable()
    {
        IAPEvent.OnRestored += Restored;
    }

    private void OnDisable()
    {
        IAPEvent.OnRestored -= Restored;
    }

    private void Restored(bool isSuccess)
    {
        ScreenFaderManager.DirectFadeIn();

        if (isSuccess)
        {
            AdsManager.Instance.RemoveAds();
            onRestored?.Invoke();
        }
    }

    private void Restore()
    {
        ScreenFaderManager.DirectFadeOut(ScreenFaderManager.FadeType.Black, 0.5f);

        IAPManager.Instance.Restore();
    }
}
