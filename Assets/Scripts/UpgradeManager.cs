using UnityEngine;
using YG;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Данные всех улучшений (по порядку 0-5)")]
    public UpgradeData shredderCountData;
    public UpgradeData shredderSpeedData;
    public UpgradeData capacityData;
    public UpgradeData refreshSpeedData;
    public UpgradeData recycleSpeedData;
    public UpgradeData coinCountData;

    private int[] localLevels = new int[6];

    public int ShredderCountLevel => localLevels[0];
    public int ShredderSpeedLevel => localLevels[1];
    public int CapacityLevel => localLevels[2];
    public int RefreshSpeedLevel => localLevels[3];
    public int RecycleSpeedLevel => localLevels[4];
    public int CoinCountLevel => localLevels[5];

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

    private void OnEnable() => YG2.onGetSDKData += LoadLevels;
    private void OnDisable() => YG2.onGetSDKData -= LoadLevels;

    private void Start()
    {
        if (YG2.isSDKEnabled) LoadLevels();
    }

    public void LoadLevels()
    {
        if (YG2.saves.upgradeLevels == null || YG2.saves.upgradeLevels.Length < 6)
        {
            YG2.saves.upgradeLevels = new int[6];
        }

        System.Array.Copy(YG2.saves.upgradeLevels, localLevels, 6);
    }

    public void Upgrade(UpgradeData upgrade)
    {
        int index = GetIndexByData(upgrade);
        if (index == -1) return;

        localLevels[index]++;

        // 1. Сохраняем в массиве
        YG2.saves.upgradeLevels[index] = localLevels[index];

        // 2. Сохраняем монеты и вызываем облачное сохранение
        SaveManager.SaveAllData();

        // 3. Применяем игровой эффект апгрейда
        ApplyEffect(upgrade, localLevels[index]);
    }

    public int GetLevelByData(UpgradeData upgrade)
    {
        int index = GetIndexByData(upgrade);
        return index != -1 ? localLevels[index] : 0;
    }

    private int GetIndexByData(UpgradeData upgrade)
    {
        if (upgrade == shredderCountData) return 0;
        if (upgrade == shredderSpeedData) return 1;
        if (upgrade == capacityData) return 2;
        if (upgrade == refreshSpeedData) return 3;
        if (upgrade == recycleSpeedData) return 4;
        if (upgrade == coinCountData) return 5;

        return -1;
    }

    private void ApplyEffect(UpgradeData upgrade, int level)
    {
        float value = upgrade.GetValue(level);

        if (upgrade == shredderCountData)
        {
            // Устанавливаем количество шредеров = (int)value
        }
        else if (upgrade == recycleSpeedData)
        {
            // Устанавливаем время задержки переработки = value (уменьшенное время)
        }
        // и т.д.
    }
}