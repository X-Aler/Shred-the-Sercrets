using DefaultNamespace;
using UnityEngine;
using YG;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => YG2.onGetSDKData += LoadAllData;
    private void OnDisable() => YG2.onGetSDKData -= LoadAllData;

    private void Start()
    {
        if (YG2.isSDKEnabled)
        {
            LoadAllData();
        }
    }

    // Загрузка монет и передача их в MoneyManager
    public void LoadAllData()
    {
        // 1. Загружаем баланс монет
        int savedCoins = YG2.saves.coins;

        // Присваиваем баланс в ваш MoneyManager
        if (GameServices.Instance != null && GameServices.Instance.MoneyManager != null)
        {
            // Установите загруженный баланс (реализуйте метод SetBalance или аналогичный у себя)
            GameServices.Instance.MoneyManager.SetBalance(savedCoins);
        }
    }

    // Вызывайте этот метод при сохранении игры (или при покупке / изменении монет)
    public static void SaveAllData()
    {
        if (GameServices.Instance != null && GameServices.Instance.MoneyManager != null)
        {
            YG2.saves.coins = GameServices.Instance.MoneyManager.Balance;
        }

        // Запись в облако Яндекса
        YG2.SaveProgress();
    }
}