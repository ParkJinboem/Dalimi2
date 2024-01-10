using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRemoveAds : MonoBehaviour
{
    [SerializeField] private string productId = "RemoveAds";
    [SerializeField] private Transform root;
    [SerializeField] private Button button;
    [SerializeField] private UnityEvent onPurchased;

    private void OnEnable()
    {
        IAPEvent.OnPurchased += Purchased;

        SetUI();
    }

    private void OnDisable()
    {
        IAPEvent.OnPurchased -= Purchased;
    }

    private void Purchased(UnityEngine.Purchasing.Product product, bool isSuccess)
    {
        SetUI();

        ScreenFaderManager.DirectFadeIn();

        if (isSuccess)
        {
            AdsManager.Instance.RemoveAds();
            onPurchased?.Invoke();
        }
    }

    private void SetUI()
    {
        button.onClick.RemoveListener(Purchase);
        button.onClick.AddListener(Purchase);

        root.gameObject.SetActive(!IAPManager.Instance.HasProduct(productId));
    }

    private void Purchase()
    {
        ScreenFaderManager.DirectFadeOut(ScreenFaderManager.FadeType.Black, 0.5f);

        IAPManager.Instance.Purchase(productId);
    }
}
