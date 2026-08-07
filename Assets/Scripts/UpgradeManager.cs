using DefaultNamespace;
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

    [Header("Ссылки на контроллеры сцены")]
    public ShredderController shredderController;

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
        if (YG2.isSDKEnabled)
        {
            // Если сохранения уже загружены, применяем их
            LoadLevels();
        }
        else
        {
            // ВАЖНО: Если SDK еще грузится, нам нужно инициализировать игру базовыми значениями!
            // Так как localLevels сейчас везде равны 0, метод GetValue(0) возьмет именно baseValue из ваших ScriptableObject.
            ApplyAllEffectsOnLoad();
        }
    }

    public void LoadLevels()
    {
        if (YG2.saves.upgradeLevels == null || YG2.saves.upgradeLevels.Length < 6)
        {
            YG2.saves.upgradeLevels = new int[6];
        }

        System.Array.Copy(YG2.saves.upgradeLevels, localLevels, 6);

        ApplyAllEffectsOnLoad();
    }

    public void Upgrade(UpgradeData upgrade)
    {
        int index = GetIndexByData(upgrade);
        if (index == -1) return;

        localLevels[index]++;

        YG2.saves.upgradeLevels[index] = localLevels[index];

        SaveManager.SaveAllData();

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
        if (upgrade == null) return;

        int calculatedValue = upgrade.GetValue(level);

        if (upgrade == shredderCountData)
        {
            if (shredderController != null)
            {
                shredderController.UpdateShreddersVisibility();
            }
        }
        else if (upgrade == capacityData)
        {
            int newCapacity = Mathf.RoundToInt(calculatedValue);

            if (GameServices.Instance != null && GameServices.Instance.DocumentStorage != null)
            {
                GameServices.Instance.DocumentStorage.SetCapacity(newCapacity);
            }
        }
        else if (upgrade == refreshSpeedData)
        {
            int newSpawnTime = Mathf.RoundToInt(calculatedValue);

            if (GameServices.Instance != null && GameServices.Instance.DocumentStorage != null)
            {
                GameServices.Instance.DocumentStorage.SetSpawnTime(newSpawnTime);
            }
        }
        else if (upgrade == coinCountData)
        {
            if (GameServices.Instance != null && GameServices.Instance.MoneyManager != null)
            {
                GameServices.Instance.MoneyManager.IncomeMultiplier = calculatedValue;
            }
        }
    }

    public void ApplyAllEffectsOnLoad()
    {
        ApplyEffect(shredderCountData, ShredderCountLevel);
        ApplyEffect(shredderSpeedData, ShredderSpeedLevel);
        ApplyEffect(capacityData, CapacityLevel);
        ApplyEffect(refreshSpeedData, RefreshSpeedLevel);
        ApplyEffect(recycleSpeedData, RecycleSpeedLevel);
        ApplyEffect(coinCountData, CoinCountLevel);
    }
}