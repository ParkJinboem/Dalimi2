using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IScreenFaderHandler
{
    void FadeStarted(ScreenFaderManager.FadeType fadeType);
    void FadeEnded(ScreenFaderManager.FadeType fadeType);
}

public class ScreenFaderManager : MonoBehaviour
{
    public static ScreenFaderManager Instance = null;

    public enum FadeType
    {
        Black,
        Loading,
        Starting
    }

    [SerializeField] private float fadeDuration = 1f;

    [Header("Black")]
    [SerializeField] private CanvasGroup blackCanvasGroup;

    [Header("Loading")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;

    private HashSet<IScreenFaderHandler> screenFaderHandlers = new HashSet<IScreenFaderHandler>();

    private FadeType fadeType;

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
    }

    private void Start()
    {
        blackCanvasGroup.gameObject.SetActive(false);
        loadingCanvasGroup.gameObject.SetActive(false);
    }

    public static void DirectFadeOut(FadeType fadeType = FadeType.Black, float finalAlpha = 1f)
    {
        Instance.fadeType = fadeType;

        foreach (IScreenFaderHandler handler in Instance.screenFaderHandlers)
        {
            handler.FadeStarted(fadeType);
        }

        CanvasGroup canvasGroup;
        switch (fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.blackCanvasGroup;
                break;
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = finalAlpha;
    }

    public static void FadeOut(FadeType fadeType = FadeType.Black, float finalAlpha = 1f)
    {
        Instance.StartCoroutine(Instance.IFadeOut(fadeType, finalAlpha));
    }

    private IEnumerator IFadeOut(FadeType fadeType, float finalAlpha)
    {
        Instance.fadeType = fadeType;

        CanvasGroup canvasGroup;
        switch (fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.blackCanvasGroup;
                break;
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }
        canvasGroup.gameObject.SetActive(true);

        yield return Instance.StartCoroutine(Instance.Fade(finalAlpha, canvasGroup));
    }

    public static void DirectFadeIn()
    {
        CanvasGroup canvasGroup;
        switch (Instance.fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.blackCanvasGroup;
                break;
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);

        foreach (IScreenFaderHandler handler in Instance.screenFaderHandlers)
        {
            handler.FadeEnded(Instance.fadeType);
        }
    }

    public static void FadeIn()
    {
        Instance.StartCoroutine(Instance.IFadeIn());
    }

    private IEnumerator IFadeIn()
    {
        CanvasGroup canvasGroup;
        switch (Instance.fadeType)
        {
            case FadeType.Black:
                canvasGroup = Instance.blackCanvasGroup;
                break;
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }

        yield return new WaitForSeconds(1f);
        foreach (IScreenFaderHandler handler in Instance.screenFaderHandlers)
        {
            handler.FadeEnded(Instance.fadeType);
        }

        yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));

        canvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
    {
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        canvasGroup.alpha = finalAlpha;
    }
}