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

    private void OnEnable()
    {
        // Подписываемся на Я.Игры и сразу обновляем тексты при открытии окна
        YG2.onGetSDKData += UpdateUI;
        UpdateUI();
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

    // Проверяем баланс каждый кадр. 
    // Это самый надежный способ для UI, исключающий любые баги с событиями.
    private void Update()
    {
        // Если данные еще не загрузились — ничего не делаем
        if (UpgradeManager.Instance == null || data == null || GameServices.Instance == null || GameServices.Instance.MoneyManager == null)
            return;

        int currentLevel = UpgradeManager.Instance.GetLevelByData(data);

        // Если уровень максимальный — намертво отключаем
        if (currentLevel >= data.maxLevel)
        {
            buyButton.interactable = false;
        }
        else
        {
            // Сверяем цену и текущий баланс
            int cost = data.GetCost(currentLevel);
            buyButton.interactable = GameServices.Instance.MoneyManager.Balance >= cost;
        }
    }

    // UpdateUI теперь отвечает только за обновление ТЕКСТОВ (цена, название, уровень)
    public void UpdateUI()
    {
        if (UpgradeManager.Instance == null || data == null) return;

        bool isEnglish = LocalizationManager.Instance != null &&
                         LocalizationManager.Instance.CurrentLanguage == Language.English;

        int currentLevel = UpgradeManager.Instance.GetLevelByData(data);

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
    }

    private void TryUpgrade()
    {
        int currentLevel = UpgradeManager.Instance.GetLevelByData(data);
        if (currentLevel >= data.maxLevel) return;

        int cost = data.GetCost(currentLevel);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSound);

        if (GameServices.Instance.MoneyManager.Balance >= cost)
        {
            // Списываем монеты
            GameServices.Instance.MoneyManager.RemoveBalance(cost);

            // Повышаем уровень и применяем эффект
            UpgradeManager.Instance.Upgrade(data);

            // Обновляем тексты
            UpdateUI();

            AudioManager.Instance.PlaySFX(AudioManager.Instance.buySound);

            YG2.InterstitialAdvShow();
        }
    }
}