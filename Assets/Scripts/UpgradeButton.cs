using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UpgradeButton : MonoBehaviour
{
    [Header("Данные")]
    public UpgradeData data;

    [Header("UI Элементы")]
    public TMP_Text costText;
    public Button buyButton;

    private MoneyManager subscribedMoneyManager;
    private UpgradeManager subscribedUpgradeManager;

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += UpdateUI;
        LocalizationManager.OnLanguageChanged += UpdateUI;

        if (buyButton != null)
            buyButton.onClick.AddListener(TryUpgrade);

        UpdateUI();
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= UpdateUI;
        LocalizationManager.OnLanguageChanged -= UpdateUI;

        if (buyButton != null)
            buyButton.onClick.RemoveListener(TryUpgrade);

        if (subscribedMoneyManager != null)
            subscribedMoneyManager.BalanceChanged -= OnBalanceChanged;
        subscribedMoneyManager = null;

        if (subscribedUpgradeManager != null)
            subscribedUpgradeManager.UpgradesChanged -= UpdateUI;
        subscribedUpgradeManager = null;
    }

    public void UpdateUI()
    {
        SubscribeToStateChanges();
        RefreshUI();
    }

    private void SubscribeToStateChanges()
    {
        MoneyManager moneyManager = GameServices.Instance?.MoneyManager;
        if (subscribedMoneyManager != moneyManager)
        {
            if (subscribedMoneyManager != null)
                subscribedMoneyManager.BalanceChanged -= OnBalanceChanged;

            subscribedMoneyManager = moneyManager;
            if (subscribedMoneyManager != null)
                subscribedMoneyManager.BalanceChanged += OnBalanceChanged;
        }

        UpgradeManager upgradeManager = UpgradeManager.Instance;
        if (subscribedUpgradeManager != upgradeManager)
        {
            if (subscribedUpgradeManager != null)
                subscribedUpgradeManager.UpgradesChanged -= UpdateUI;

            subscribedUpgradeManager = upgradeManager;
            if (subscribedUpgradeManager != null)
                subscribedUpgradeManager.UpgradesChanged += UpdateUI;
        }
    }

    private void OnBalanceChanged(int balance) => RefreshUI();

    private void RefreshUI()
    {
        UpgradeManager upgradeManager = UpgradeManager.Instance;
        MoneyManager moneyManager = GameServices.Instance?.MoneyManager;

        if (data == null || upgradeManager == null || moneyManager == null)
        {
            if (buyButton != null)
                buyButton.interactable = false;
            return;
        }

        bool isEnglish = LocalizationManager.Instance != null &&
                         LocalizationManager.Instance.CurrentLanguage == Language.English;

        int currentLevel = upgradeManager.GetLevelByData(data);

        if (currentLevel >= data.maxLevel)
        {
            if (costText != null)
            {
                costText.text = isEnglish ? "MAX" : "МАКС.";
            }
        }
        else
        {
            int cost = data.GetCost(currentLevel);
            if (costText != null) costText.text = cost.ToString("#,##0");
        }

        if (buyButton != null)
        {
            buyButton.interactable = currentLevel < data.maxLevel &&
                                     moneyManager.Balance >= data.GetCost(currentLevel);
        }
    }

    private void TryUpgrade()
    {
        UpgradeManager upgradeManager = UpgradeManager.Instance;
        MoneyManager moneyManager = GameServices.Instance?.MoneyManager;
        if (upgradeManager == null || moneyManager == null || data == null)
            return;

        int currentLevel = upgradeManager.GetLevelByData(data);
        if (currentLevel >= data.maxLevel) return;

        int cost = data.GetCost(currentLevel);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

        if (moneyManager.Balance >= cost)
        {
            moneyManager.RemoveBalance(cost);

            upgradeManager.Upgrade(data);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.buySound);

            UpdateUI();
            YG2.InterstitialAdvShow();
        }
    }
}
