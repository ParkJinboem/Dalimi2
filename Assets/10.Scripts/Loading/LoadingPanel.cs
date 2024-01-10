using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadingPanel : MonoBehaviour
{
    public static LoadingPanel Instance = null;

    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private Image loadingBar;
    [SerializeField] private float defaultTime = 1f;
    [SerializeField] [Range(0f, 1f)] private float defaultProgress = 0.5f;

    private bool loaded = false;
    private Action callback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        loadingCanvas.enabled = false;
    }

    IEnumerator ILoading()
    {
        float loadingTime = 0f;
        while (loadingTime < defaultTime)
        {
            loadingTime += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(0f, defaultProgress, loadingTime / defaultTime);
            yield return null;
        }
        loadingBar.fillAmount = defaultProgress;

        loadingTime = 0f;
        while (loadingTime < 1f)
        {
            if (loaded)
            {
                loadingTime += Time.deltaTime;
                loadingBar.fillAmount = Mathf.Lerp(defaultProgress, 1f, loadingTime);
            }
            yield return null;
        }
        loadingBar.fillAmount = 1f;

        callback?.Invoke();
    }

    public void Show(Action callback)
    {
        this.callback = callback;

        loaded = false;
        loadingBar.fillAmount = 0f;

        AdsManager.Instance.DestroyBanner();
        loadingCanvas.enabled = true;

        StopAllCoroutines();
        StartCoroutine(ILoading());
    }

    public void End()
    {
        loaded = true;
    }

    public void Hide()
    {
        AdsManager.Instance.WatchAdWithFuncUse("Loading");
        AdsManager.Instance.RequestBanner();
        loadingCanvas.enabled = false;
    }
}