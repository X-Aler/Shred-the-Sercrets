using DefaultNamespace;
using UnityEngine;
using GamePlatform;

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

    private void OnEnable() => PlatformServices.Ready += LoadAllData;
    private void OnDisable() => PlatformServices.Ready -= LoadAllData;

    private void Start()
    {
        if (PlatformServices.IsReady)
        {
            LoadAllData();
        }
    }

    private void OnApplicationPause(bool paused) { if (paused) SaveAllData(); }
    private void OnApplicationFocus(bool focused) { if (!focused) SaveAllData(); }

    // Загрузка монет и передача их в MoneyManager
    public void LoadAllData()
    {
        if (!PlatformServices.IsReady) return;
        // 1. Загружаем баланс монет
        int savedCoins = PlatformServices.Progress.coins;

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
        if (!PlatformServices.IsReady) return;
        if (GameServices.Instance != null && GameServices.Instance.MoneyManager != null)
        {
            PlatformServices.Progress.coins = GameServices.Instance.MoneyManager.Balance;
        }

        // Сохранение через выбранную платформу
        PlatformServices.Save();
    }
}