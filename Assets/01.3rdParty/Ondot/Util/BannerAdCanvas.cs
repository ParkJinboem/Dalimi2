using UnityEngine;

public interface IBannerAdHandler
{
    public void SetPositionY(float positionY);
}

public class BannerAdCanvas : MonoBehaviour
{
    private RectTransform canvasRectTransform;
    private IBannerAdHandler[] handlers;

    private void Awake()
    {
        canvasRectTransform = GetComponent<RectTransform>();
        handlers = GetComponentsInChildren<IBannerAdHandler>(true);

        AdsManager.OnLoadedBanner += OnLoadedBanner;
    }

    private void OnDestroy()
    {
        AdsManager.OnLoadedBanner -= OnLoadedBanner;
    }

    #region Actions-Events
    public void OnLoadedBanner(bool isShow)
    {
        for (int i = 0; i < handlers.Length; i++)
        {
            handlers[i].SetPositionY(isShow ? -AdsManager.Instance.GetBannerHeight(canvasRectTransform.rect) : 0f);            
        }
    }
    #endregion
}
