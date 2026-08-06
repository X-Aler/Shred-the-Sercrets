using System.Collections.Generic;
using UnityEngine;

public enum UpgradeEffectType
{
    Additive,       // Прибавление: baseValue + (level * valuePerLevel)
    Subtractive,    // Уменьшение (например, задержка): baseValue - (level * valuePerLevel)
    Multiplier      // Умножение: baseValue * (1 + level * valuePerLevel)
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Основная информация")]
    public string upgradeName;
    public int maxLevel = 5;

    [Header("Настройка стоимости")]
    [Tooltip("Если включено, стоимость берётся из списка customCosts. Если выключено — считается по формуле.")]
    public bool useCustomCosts = false;

    [Tooltip("Кастомные цены для каждого уровня (0 = цена 1-го ур., 1 = цена 2-го ур. и т.д.)")]
    public List<int> customCosts = new List<int> { 100, 3000, 10000 };

    [Tooltip("Базовая цена для формулы")]
    public int baseCost = 100;
    [Tooltip("Множитель цены для формулы")]
    public float costMultiplier = 1.5f;

    [Header("Настройка эффекта")]
    public UpgradeEffectType effectType = UpgradeEffectType.Additive;
    public float baseValue = 1f;
    public float valuePerLevel = 0.1f;

    [Tooltip("Минимально допустимое значение (актуально для уменьшения времени, чтобы не уйти в 0 или минус)")]
    public float minValue = 0.05f;

    /// <summary>
    /// Получение стоимости следующего уровня
    /// </summary>
    public int GetCost(int currentLevel)
    {
        if (currentLevel >= maxLevel) return 0;

        // 1. Ручные цены из списка (например: 100, 3000, 10000)
        if (useCustomCosts)
        {
            if (customCosts != null && currentLevel < customCosts.Count)
            {
                return customCosts[currentLevel];
            }
            // Если массив кончился раньше maxLevel, берем последнюю указанную цену
            if (customCosts != null && customCosts.Count > 0)
            {
                return customCosts[customCosts.Count - 1];
            }
        }

        // 2. Стандартный расчёт по формуле
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
    }

    /// <summary>
    /// Получение значения параметра в зависимости от типа эффекта
    /// </summary>
    public float GetValue(int currentLevel)
    {
        float result = baseValue;

        switch (effectType)
        {
            case UpgradeEffectType.Additive:
                result = baseValue + (currentLevel * valuePerLevel);
                break;

            case UpgradeEffectType.Subtractive:
                // Уменьшаем время/задержку с каждым уровнем
                result = baseValue - (currentLevel * valuePerLevel);
                result = Mathf.Max(minValue, result); // Не даем опуститься ниже минимального порога
                break;

            case UpgradeEffectType.Multiplier:
                result = baseValue * Mathf.Pow(1f + valuePerLevel, currentLevel);
                break;
        }

        return result;
    }
}