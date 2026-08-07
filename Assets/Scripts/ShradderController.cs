using UnityEngine;
using YG;

public class ShredderController : MonoBehaviour
{
    [Header("Ссылка на ScriptableObject шредеров")]
    public UpgradeData shredderCountData;

    [Header("Все шредеры на сцене (ровно 3 шт)")]
    public GameObject[] shredderObjects;

    private void OnEnable()
    {
        YG2.onGetSDKData += UpdateShreddersVisibility;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= UpdateShreddersVisibility;
    }

    private void Start()
    {
        UpdateShreddersVisibility();
    }

    public void UpdateShreddersVisibility()
    {
        if (UpgradeManager.Instance == null || shredderCountData == null) return;

        int currentLevel = UpgradeManager.Instance.GetLevelByData(shredderCountData);

        int targetCount = Mathf.RoundToInt(shredderCountData.GetValue(currentLevel));

        for (int i = 0; i < shredderObjects.Length; i++)
        {
            if (shredderObjects[i] != null)
            {
                shredderObjects[i].SetActive(i < targetCount);
            }
        }
    }
}