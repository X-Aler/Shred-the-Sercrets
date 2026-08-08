using System;
using UnityEngine;
using YG;

public enum Language
{
    Russian,
    English
}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }
    public Language CurrentLanguage { get; private set; } = Language.Russian;

    public static event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DetectLanguage();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void DetectLanguage()
    {
        string langCode = "";

#if !UNITY_EDITOR
    // В билде берем язык строго из SDK Яндекса
    if (YG2.envir != null && !string.IsNullOrEmpty(YG2.envir.language))
    {
        langCode = YG2.envir.language;
    }
#else
        if (Application.systemLanguage != SystemLanguage.Russian &&
            Application.systemLanguage != SystemLanguage.Ukrainian &&
            Application.systemLanguage != SystemLanguage.Belarusian)
        {
            langCode = "en";
        }
        else
        {
            langCode = "ru";
        }
#endif

        if (!string.IsNullOrEmpty(langCode) &&
           (langCode.StartsWith("ru") || langCode.StartsWith("be") || langCode.StartsWith("uk")))
        {
            CurrentLanguage = Language.Russian;
        }
        else
        {
            CurrentLanguage = Language.English;
        }

        OnLanguageChanged?.Invoke();
    }

    [ContextMenu("Test Switch Language")]
    public void ToggleLanguage()
    {
        CurrentLanguage = (CurrentLanguage == Language.Russian) ? Language.English : Language.Russian;
        OnLanguageChanged?.Invoke();
        Debug.Log($"[Localization] Язык изменен на: {CurrentLanguage}");
    }

    private void OnEnable()
    {
        // Подписываемся на ивент готовности данных YG2
        YG2.onGetSDKData += DetectLanguage;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= DetectLanguage;
    }
}