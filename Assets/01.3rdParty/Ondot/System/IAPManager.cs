using System;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPEvent
{
    public delegate void InitedHandler(bool isSuccess);
    public static event InitedHandler OnInited;
    public static void Inited(bool isSuccess)
    {
        OnInited?.Invoke(isSuccess);
    }

    public delegate void PurchasedHandler(Product product, bool isSuccess);
    public static event PurchasedHandler OnPurchased;
    public static void Purchased(Product product, bool isSuccess)
    {
        OnPurchased?.Invoke(product, isSuccess);
    }

    public delegate void RestoredHandler(bool isSuccess);
    public static event RestoredHandler OnRestored;
    public static void Restored(bool isSuccess)
    {
        OnRestored?.Invoke(isSuccess);
    }
}

public class IAPManager : MonoBehaviour, IStoreListener
{
    [Serializable]
    public class AppProduct
    {
        public string name;
        public string android;
        public string iOS;
        public ProductType productType;
    }

    public static IAPManager Instance = null;

    private IStoreController m_StoreController;          // Unity 구매 시스템
    private IExtensionProvider m_StoreExtensionProvider; // 상점 구매 하위 시스템 
    private CrossPlatformValidator m_Validator;
    private bool isRestore = false; // 구매 복구 진행 상태

    [SerializeField] private List<AppProduct> appProducts = new List<AppProduct>();
    public List<AppProduct> AppProducts
    {
        get { return appProducts; }
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

    public void Init()
    {
        InitializePurchasing();
    }

    async void InitializePurchasing()
    {
        if (IsInitialized())
        {
            IAPEvent.Inited(true);
            return;
        }

        try
        {
            var options = new InitializationOptions().SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception ex)
        {
            IAPEvent.Inited(false);
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < appProducts.Count; i++)
        {
            builder.AddProduct(appProducts[i].name, appProducts[i].productType, new IDs()
                {
                    { appProducts[i].android, GooglePlay.Name },
                    { appProducts[i].iOS, AppleAppStore.Name },
                });
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        InitializeValidator();

        LogProducts(m_StoreController.products.all);

        if (isRestore)
        {
            Restored();
        }
        else
        {
            IAPEvent.Inited(true);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        IAPEvent.Inited(false);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        IAPEvent.Inited(false);
    }

    public void Purchase(string productId)
    {
        if (!GameManager.Instance.GameSettings.IsIAPLive)
        {
            PlayerPrefs.SetInt(productId, 1);
            IAPEvent.Purchased(null, true);
            return;
        }

        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                IAPEvent.Purchased(product, false);
            }
        }
        else
        {
            IAPEvent.Purchased(null, false);
        }
    }

    public void Restore()
    {
        if (!GameManager.Instance.GameSettings.IsIAPLive)
        {
            IAPEvent.Restored(true);
            return;
        }

        if (isRestore)
        {
            return;
        }
        isRestore = true;

        m_StoreController = null;
        m_StoreExtensionProvider = null;

        Init();
    }

    private void Restored()
    {
        if (!IsInitialized())
        {
            IAPEvent.Restored(false);
            isRestore = false;
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((success, error) =>
            {
                if (success)
                {
                    IAPEvent.Restored(true);
                }
                else
                {
                    IAPEvent.Restored(false);
                }
                isRestore = false;
            });
        }
        else
        {
            IAPEvent.Restored(true);
            isRestore = false;
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;

        // 영수증 검증
        if (IsPurchaseValid(product))
        {
            var result = m_Validator.Validate(product.receipt);
            foreach (var receipt in result)
            {
                if (receipt is GooglePlayReceipt google)
                {
                    if ((int)google.purchaseState == 4)
                    {
                        // 결제 대기중
                        return PurchaseProcessingResult.Pending;
                    }
                }
            }
            LogProduct(product);
            IAPEvent.Purchased(product, true);
        }
        else
        {
            IAPEvent.Purchased(product, false);
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        IAPEvent.Purchased(product, false);
    }

    private void InitializeValidator()
    {
        if (IsCurrentStoreSupportedByValidator())
        {
#if !UNITY_EDITOR
                //var appleTangleData = AppleStoreKitTestTangle.Data(); // 샌드박스 결제 테스트
                var appleTangleData = AppleTangle.Data();
                m_Validator = new CrossPlatformValidator(GooglePlayTangle.Data(), appleTangleData, Application.identifier);
#endif
        }
    }

    private bool IsCurrentStoreSupportedByValidator()
    {
        return IsGooglePlayStoreSelected() || IsAppleAppStoreSelected();
    }

    private bool IsGooglePlayStoreSelected()
    {
        var currentAppStore = StandardPurchasingModule.Instance().appStore;
        return currentAppStore == AppStore.GooglePlay;
    }

    private bool IsAppleAppStoreSelected()
    {
        var currentAppStore = StandardPurchasingModule.Instance().appStore;
        return currentAppStore == AppStore.AppleAppStore ||
               currentAppStore == AppStore.MacAppStore;
    }

    private bool IsPurchaseValid(Product product)
    {
        if (IsCurrentStoreSupportedByValidator())
        {
            try
            {
                var result = m_Validator.Validate(product.receipt);
                LogReceipts(result);
                return true;
            }
            catch (IAPSecurityException reason)
            {
                Debug.Log($"Invalid receipt: {reason}");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void LogProducts(Product[] products)
    {
        foreach (Product product in products)
        {
            LogProduct(product);
        }
    }

    private void LogProduct(Product product)
    {
        Debug.Log("transactionID : " + product.transactionID);
        Debug.Log("receipt : " + product.receipt);
        Debug.Log("metadata isoCurrencyCode : " + product.metadata.isoCurrencyCode);
        Debug.Log("metadata localizedDescription : " + product.metadata.localizedDescription);
        Debug.Log("metadata localizedPrice : " + product.metadata.localizedPrice);
        Debug.Log("metadata localizedPriceString : " + product.metadata.localizedPriceString);
        Debug.Log("metadata localizedTitle : " + product.metadata.localizedTitle);
        Debug.Log("hasReceipt : " + product.hasReceipt);
        Debug.Log("availableToPurchase : " + product.availableToPurchase);
        Debug.Log("enabled : " + product.definition.enabled);
        Debug.Log("id : " + product.definition.id);
        Debug.Log("payout : " + product.definition.payout);
        Debug.Log("payouts : " + product.definition.payouts);
        Debug.Log("storeSpecificId : " + product.definition.storeSpecificId);
        Debug.Log("type : " + product.definition.type);

        if (product != null &&
            product.hasReceipt)
        {
            PlayerPrefs.SetInt(product.definition.id, 1);
        }
        else
        {
            PlayerPrefs.SetInt(product.definition.id, 0);
        }
    }

    private void LogReceipts(IEnumerable<IPurchaseReceipt> receipts)
    {
        Debug.Log("Receipt is valid. Contents:");
        foreach (var receipt in receipts)
        {
            LogReceipt(receipt);
        }
    }

    private void LogReceipt(IPurchaseReceipt receipt)
    {
        Debug.Log($"Product ID: {receipt.productID}\n" +
                  $"Purchase Date: {receipt.purchaseDate}\n" +
                  $"Transaction ID: {receipt.transactionID}");

        if (receipt is GooglePlayReceipt googleReceipt)
        {
            Debug.Log($"Purchase State: {googleReceipt.purchaseState}\n" +
                      $"Purchase Token: {googleReceipt.purchaseToken}");
        }

        if (receipt is AppleInAppPurchaseReceipt appleReceipt)
        {
            Debug.Log($"Original Transaction ID: {appleReceipt.originalTransactionIdentifier}\n" +
                      $"Subscription Expiration Date: {appleReceipt.subscriptionExpirationDate}\n" +
                      $"Cancellation Date: {appleReceipt.cancellationDate}\n" +
                      $"Quantity: {appleReceipt.quantity}");
        }
    }

    public Product GetProduct(string productId)
    {
        Product product = null;
        if (IsInitialized())
        {
            product = m_StoreController.products.WithID(productId);
        }
        return product;
    }

    public bool HasProduct(string productId)
    {
        return PlayerPrefs.GetInt(productId, 0) == 1;
    }
}