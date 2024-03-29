using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class AdsEvent
{
    public delegate void AdsRemovedHandler();
    public static event AdsRemovedHandler OnAdsRemoved;
    public static void AdRemoved()
    {
        OnAdsRemoved?.Invoke();
    }
}

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance = null;

    private AdSize bannerAdSize = AdSize.Banner;

    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private RewardedAd attendanceAd;
    private RewardedAd clothItemAd;
    private InterstitialAd funcUseAd;

    private bool isLoadingAttendanceAd;
    private bool isLoadingClothItemAd;
    private bool isLoadingFuncUseAd;

    [Header("AD UNIT ID")]
    public string androidAppOpenId;
    public string iOSAppOpenId;
    public string androidBannerId;
    public string iOSBannerId;
    public string androidAttendancId;
    public string iOSAttendancId;
    public string androidClothItemId;
    public string iOSClothItemId;
    public string androidFuncUseId;
    public string iOSFuncUseId;

    public static event Action<bool> OnLoadedBanner = delegate { };
    private Action<AdsCallbackState> callback;

    public bool HasRemoveAds
    {
        get { return IAPManager.Instance.HasProduct("RemoveAds"); }
    }

    public enum AdsCallbackState
    {
        Fail, // 광고 시청 실패
        Loading, // 광고 요청하고 로딩 중
        Success // 광고 시청 완료
    }

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

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            ShowAppOpen();
        }
    }

    private void Complete()
    {
        callback?.Invoke(AdsCallbackState.Success);
        callback = null;
    }

    /// <summary>
    /// 광고 초기화
    /// </summary>
    public void InitAds()
    {
        if (HasRemoveAds)
        {
            return;
        }

        RequestAppOpen();
        RequestAttendanceVideo();
        RequestClothItemVideo();
        RequestFuncUse();
    }

    /// <summary>
    /// 광고 제거
    /// </summary>
    public void RemoveAds()
    {
        if (!HasRemoveAds)
        {
            return;
        }

        RequestBanner();
        AdsEvent.AdRemoved();
    }

    #region 앱 오픈
    private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
    private DateTime appOpenExpireTime;
    private bool isLockAppOpen; // 앱 오픈 일시적 잠금
    private bool isOnceLockAppOpen; // 기타 광고 종료 후 OnApplicationPause 선 호출 광고 노출되는 현상 1회 막기

    private bool IsAppOpenAdAvailable
    {
        get
        {
            return !isLockAppOpen
                && appOpenAd != null
                && appOpenAd.CanShowAd()
                && DateTime.Now < appOpenExpireTime;
        }
    }

    public void SetLockAppOpen(bool isLockAppOpen)
    {
        this.isLockAppOpen = isLockAppOpen;
    }

    /// <summary>    
    /// 앱 오픈
    /// </summary>
    public void RequestAppOpen()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        string adUnitId;
        if (GameManager.Instance.GameSettings.IsAdLive)
        {
            #if UNITY_ANDROID
                adUnitId = androidAppOpenId;
            #elif UNITY_IPHONE
                adUnitId = iOSAppOpenId;
            #else
		        adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/3419835294";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/5662855259";
            #else
		        adUnitId = "unexpected_platform";
            #endif
        }

        AdRequest request = new AdRequest.Builder()
            .Build();

        AppOpenAd.Load(adUnitId, ScreenOrientation.Portrait, request, (AppOpenAd appOpenAd, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.Log($"ads Load :{error.GetMessage()}");
            }

            if (error != null ||
                appOpenAd == null)
            {
                return;
            }

            this.appOpenAd = appOpenAd;
            appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;

            appOpenAd.OnAdFullScreenContentClosed += HandleAdFullScreenContentClosedWithAppOpen;
            appOpenAd.OnAdFullScreenContentFailed += HandleAdFullScreenContentFailedWithAppOpen;
            appOpenAd.OnAdFullScreenContentOpened += HandleAdFullScreenContentOpenedWithAppOpen;
            appOpenAd.OnAdPaid += HandleAdPaidWithAppOpen;
            appOpenAd.OnAdImpressionRecorded += HandleAdImpressionRecordedWithAppOpen;
            appOpenAd.OnAdClicked += HandleAdClickedWithAppOpen;
        });
    }

    private void HandleAdFullScreenContentClosedWithAppOpen()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
        RequestAppOpen();
    }

    private void HandleAdFullScreenContentFailedWithAppOpen(AdError error)
    {
        Debug.Log($"ads Handle :{error.GetMessage()}");

        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
        RequestAppOpen();
    }

    private void HandleAdFullScreenContentOpenedWithAppOpen() { }
    private void HandleAdPaidWithAppOpen(AdValue adValue) { }
    private void HandleAdImpressionRecordedWithAppOpen() { }
    private void HandleAdClickedWithAppOpen() { }

    public void ShowAppOpen()
    {
        if (HasRemoveAds)
        {
            return;
        }

        if (IsAppOpenAdAvailable)
        {
            if (isOnceLockAppOpen)
            {
                isOnceLockAppOpen = false;
                return;
            }

            appOpenAd.Show();
        }
        else
        {
            RequestAppOpen();
        }
    }
    #endregion

    #region 배너
    public float GetBannerHeight(Rect canvasSize)
    {
        return Screen.dpi * canvasSize.height / Screen.height / 160 * bannerAdSize.Height;
    }

    /// <summary>    
    /// 배너 요청
    /// (배너는 메인 진입 완료 시 요청)
    /// </summary>
    public void RequestBanner()
    {
        DestroyBanner();

        if (!GameManager.Instance.GameSettings.UseAd)
        {
            return;
        }

        if (HasRemoveAds)
        {
            return;
        }

        string adUnitId;
        if (GameManager.Instance.GameSettings.IsAdLive)
        {
            #if UNITY_ANDROID
                adUnitId = androidBannerId;
            #elif UNITY_IPHONE
                adUnitId = iOSBannerId;
            #else
		        adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/6300978111";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/2934735716";
            #else
		        adUnitId = "unexpected_platform";
            #endif
        }

        bannerView = new BannerView(adUnitId, bannerAdSize, AdPosition.Top);
        bannerView.OnBannerAdLoaded += HandleBannerAdLoadedWithBanner;
        bannerView.OnBannerAdLoadFailed += HandleBannerAdLoadFailedWithBanner;
        bannerView.OnAdPaid += HandleAdPaidWithBanner;
        bannerView.OnAdImpressionRecorded += HandleAdImpressionRecordedWithBanner;
        bannerView.OnAdClicked += HandleAdClickedWithBanner;
        bannerView.OnAdFullScreenContentOpened += HandleAdFullScreenContentOpenedWithBanner;
        bannerView.OnAdFullScreenContentClosed += HandleAdFullScreenContentClosedWithBanner;

        AdRequest request = new AdRequest.Builder()
            .Build();
        bannerView.LoadAd(request);
    }

    public void RefreshBanner()
    {
        RequestBanner();
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
        OnLoadedBanner(false);
    }

    private void HandleBannerAdLoadedWithBanner()
    {
        OnLoadedBanner(true);
    }

    private void HandleBannerAdLoadFailedWithBanner(LoadAdError error) { }

    private void HandleAdPaidWithBanner(AdValue adValue) { }
    private void HandleAdImpressionRecordedWithBanner() { }
    private void HandleAdClickedWithBanner() { }
    private void HandleAdFullScreenContentOpenedWithBanner() { }
    private void HandleAdFullScreenContentClosedWithBanner() { }
    #endregion

    #region 출석 광고
    /// <summary>
    /// 출석 광고 요청
    /// </summary>
    private void RequestAttendanceVideo()
    {
        if (attendanceAd != null &&
            attendanceAd.CanShowAd())
        {
            return;
        }
        if (isLoadingAttendanceAd)
        {
            return;
        }
        isLoadingAttendanceAd = true;

        string adUnitId;
        if (GameManager.Instance.GameSettings.IsAdLive)
        {
            #if UNITY_ANDROID
                adUnitId = androidAttendancId;
            #elif UNITY_IPHONE
                adUnitId = iOSAttendancId;
            #else
                adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/5224354917";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
            #else
                adUnitId = "unexpected_platform";
            #endif
        }

        AdRequest request = new AdRequest.Builder()
            .Build();

        RewardedAd.Load(adUnitId, request, (RewardedAd rewardedAd, LoadAdError error) =>
        {
            isLoadingAttendanceAd = false;

            if (error != null ||
                rewardedAd == null)
            {
                return;
            }

            attendanceAd = rewardedAd;

            attendanceAd.OnAdFullScreenContentOpened += HandleAdFullScreenContentOpenedAttendance;
            attendanceAd.OnAdFullScreenContentClosed += HandleAdFullScreenContentClosedAttendance;
            attendanceAd.OnAdImpressionRecorded += HandleAdImpressionRecordedAttendance;
            attendanceAd.OnAdClicked += HandleAdClickedAttendance;
            attendanceAd.OnAdFullScreenContentFailed += HandleAdFullScreenContentFailedAttendance;
            attendanceAd.OnAdPaid += HandleAdPaidAttendance;
        });
    }

    /// <summary>
    /// 광고 시청
    /// </summary>
    public void WatchVideoWithAttendance(Action<AdsCallbackState> callback)
    {
        if (HasRemoveAds)
        {
            this.callback = callback;
            Complete();
            return;
        }

        if (attendanceAd != null &&
            attendanceAd.CanShowAd())
        {
            this.callback = callback;
            attendanceAd.Show((Reward reward) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Complete();
                });
            });
        }
        else
        {
            // 광고 로딩 중 알림창 필요
            callback?.Invoke(AdsCallbackState.Loading);
            RequestAttendanceVideo();
        }
    }

    private void HandleAdFullScreenContentOpenedAttendance()
    {
        isLockAppOpen = true;
        isOnceLockAppOpen = true;
    }

    private void HandleAdFullScreenContentClosedAttendance()
    {
        isLockAppOpen = false;
        RequestAttendanceVideo();
    }

    private void HandleAdImpressionRecordedAttendance() { }
    private void HandleAdClickedAttendance() { }
    private void HandleAdFullScreenContentFailedAttendance(AdError error) { }
    private void HandleAdPaidAttendance(AdValue adValue) { }
    #endregion

    #region 옷입히기 페이지 아이템 광고
    /// <summary>
    /// 옷입히기 페이지 아이템 광고 요청
    /// </summary>
    private void RequestClothItemVideo()
    {
        if (clothItemAd != null &&
            clothItemAd.CanShowAd())
        {
            return;
        }
        if (isLoadingClothItemAd)
        {
            return;
        }
        isLoadingClothItemAd = true;

        string adUnitId;
        if (GameManager.Instance.GameSettings.IsAdLive)
        {
            #if UNITY_ANDROID
                adUnitId = androidClothItemId;
            #elif UNITY_IPHONE
                adUnitId = iOSClothItemId;
            #else
                adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/5224354917";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/1712485313";
            #else
                adUnitId = "unexpected_platform";
            #endif
        }

        AdRequest request = new AdRequest.Builder()
            .Build();

        RewardedAd.Load(adUnitId, request, (RewardedAd rewardedAd, LoadAdError error) =>
        {
            isLoadingClothItemAd = false;

            if (error != null ||
                rewardedAd == null)
            {
                return;
            }

            clothItemAd = rewardedAd;

            clothItemAd.OnAdFullScreenContentOpened += HandleAdFullScreenContentOpenedClothItem;
            clothItemAd.OnAdFullScreenContentClosed += HandleAdFullScreenContentClosedClothItem;
            clothItemAd.OnAdImpressionRecorded += HandleAdImpressionRecordedClothItem;
            clothItemAd.OnAdClicked += HandleAdClickedClothItem;
            clothItemAd.OnAdFullScreenContentFailed += HandleAdFullScreenContentFailedClothItem;
            clothItemAd.OnAdPaid += HandleAdPaidClothItem;
        });
    }

    /// <summary>
    /// 광고 시청
    /// </summary>
    public void WatchVideoWithClothItem(Action<AdsCallbackState> callback)
    {
        if (HasRemoveAds)
        {
            this.callback = callback;
            Complete();
            return;
        }

        if (clothItemAd != null &&
            clothItemAd.CanShowAd())
        {
            this.callback = callback;
            clothItemAd.Show((Reward reward) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Complete();
                });
            });
        }
        else
        {
            // 광고 로딩 중 알림창 필요
            callback?.Invoke(AdsCallbackState.Loading);
            RequestClothItemVideo();
        }
    }

    private void HandleAdFullScreenContentOpenedClothItem()
    {
        isLockAppOpen = true;
        isOnceLockAppOpen = true;
    }

    private void HandleAdFullScreenContentClosedClothItem()
    {
        isLockAppOpen = false;
        RequestClothItemVideo();
    }

    private void HandleAdImpressionRecordedClothItem() { }
    private void HandleAdClickedClothItem() { }
    private void HandleAdFullScreenContentFailedClothItem(AdError error) { }
    private void HandleAdPaidClothItem(AdValue adValue) { }
    #endregion

    #region 기능 사용
    /// <summary>
    /// 기능 사용 광고 요청
    /// </summary>
    private void RequestFuncUse()
    {
        if (funcUseAd != null &&
            funcUseAd.CanShowAd())
        {
            return;
        }
        if (isLoadingFuncUseAd)
        {
            return;
        }
        isLoadingFuncUseAd = true;

        string adUnitId;
        if (GameManager.Instance.GameSettings.IsAdLive)
        {
            #if UNITY_ANDROID
                adUnitId = androidFuncUseId;
            #elif UNITY_IPHONE
                adUnitId = iOSFuncUseId;
            #else
                adUnitId = "unexpected_platform";
            #endif
        }
        else
        {
            #if UNITY_ANDROID
                adUnitId = "ca-app-pub-3940256099942544/1033173712";
            #elif UNITY_IPHONE
                adUnitId = "ca-app-pub-3940256099942544/4411468910";
            #else
                adUnitId = "unexpected_platform";
            #endif
        }

        AdRequest request = new AdRequest.Builder()
            .Build();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd interstitialAd, LoadAdError error) =>
        {
            isLoadingFuncUseAd = false;

            if (error != null ||
                interstitialAd == null)
            {
                return;
            }

            funcUseAd = interstitialAd;

            funcUseAd.OnAdFullScreenContentOpened += HandleAdFullScreenContentOpenedFuncUse;
            funcUseAd.OnAdFullScreenContentClosed += HandleAdFullScreenContentClosedFuncUse;
            funcUseAd.OnAdImpressionRecorded += HandleAdImpressionRecordedFuncUse;
            funcUseAd.OnAdClicked += HandleAdClickedFuncUse;
            funcUseAd.OnAdFullScreenContentFailed += HandleAdFullScreenContentFailedFuncUse;
            funcUseAd.OnAdPaid += HandleAdPaidFuncUse;
        });
    }

    /// <summary>
    /// 광고 시청
    /// </summary>
    public void WatchAdWithFuncUse(string key = null)
    {
        if (HasRemoveAds)
        {
            return;
        }

        bool isPass = false;
        if (!string.IsNullOrEmpty(key))
        {
            isPass = PlayerPrefs.GetInt(key, 0) == 0;
        }

        if (funcUseAd != null &&
            funcUseAd.CanShowAd())
        {
            if (isPass)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            else
            {
                funcUseAd.Show();
            }
        }
        else
        {
            RequestFuncUse();
        }
    }

    private void HandleAdFullScreenContentOpenedFuncUse()
    {
        isLockAppOpen = true;
        isOnceLockAppOpen = true;
    }

    private void HandleAdFullScreenContentClosedFuncUse()
    {
        isLockAppOpen = false;
        RequestFuncUse();
    }

    private void HandleAdImpressionRecordedFuncUse() { }
    private void HandleAdClickedFuncUse() { }
    private void HandleAdFullScreenContentFailedFuncUse(AdError error) { }
    private void HandleAdPaidFuncUse(AdValue adValue) { }
    #endregion
}
