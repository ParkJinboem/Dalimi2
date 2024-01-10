using UnityEngine;
using System;
using OnDot.System;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField]
    private GameSettings gameSettings;
    public GameSettings GameSettings
    {
        get { return gameSettings; }
    }

    private Action callback;

    void Awake()
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
        RequestTrackingAuthorizationPlugin.OnRequestTA += HandlerRequestTA;
    }

    private void OnDestroy()
    {
        RequestTrackingAuthorizationPlugin.OnRequestTA -= HandlerRequestTA;
    }

    public void Init(Action callback)
    {
        this.callback = callback;

        // 인앱 초기화
        IAPManager.Instance.Init();

        // 광고 초기화
        RequestTrackingAuthorizationPlugin.RequestTrackingAuthorization();
    }

    private void HandlerRequestTA(bool isAgree)
    {
        MobileAds.Initialize(initStatus =>
        {
            AdsManager.Instance.InitAds();
            callback?.Invoke();
        });
    }
}
