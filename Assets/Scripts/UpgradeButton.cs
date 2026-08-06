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
    public TMP_Text titleText;
    public TMP_Text costText;
    public Button buyButton;

    private void OnEnable()
    {
        // Подписываемся на загрузку данных из облака
        YG2.onGetSDKData += UpdateUI;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= UpdateUI;
    }

    private void Start()
    {
        buyButton.onClick.AddListener(TryUpgrade);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (UpgradeManager.Instance == null || data == null) return;

        int currentLevel = UpgradeManager.Instance.GetLevelByData(data);

        // Название и уровень
        if (titleText != null)
        {
            titleText.text = $"{data.upgradeName} (Ур. {currentLevel}/{data.maxLevel})";
        }

        // Состояние кнопки и цены
        if (currentLevel >= data.maxLevel)
        {
            if (costText != null) costText.text = "МАКС.";
            buyButton.interactable = false;
        }
        else
        {
            int cost = data.GetCost(currentLevel);

            // Форматируем число с пробелами (например, 10 000 вместо 10000)
            if (costText != null) costText.text = cost.ToString("#,##0");

            // Проверяем баланс для активности кнопки
            bool canAfford = GameServices.Instance.MoneyManager.Balance >= cost;
            buyButton.interactable = canAfford;
        }
    }

    private void TryUpgrade()
    {
        int currentLevel = UpgradeManager.Instance.GetLevelByData(data);
        if (currentLevel >= data.maxLevel) return;

        int cost = data.GetCost(currentLevel);

        if (GameServices.Instance.MoneyManager.Balance >= cost)
        {
            // Списываем монеты
            GameServices.Instance.MoneyManager.RemoveBalance(cost);

            // Повышаем уровень и применение эффекта (происходит внутри UpgradeManager)
            UpgradeManager.Instance.Upgrade(data);

            // Обновляем UI
            UpdateUI();
        }
        else
        {
            Debug.Log("Недостаточно монет!");
        }
    }
}