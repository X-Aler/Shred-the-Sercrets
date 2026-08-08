using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [TextArea] public string russianText;
    [TextArea] public string englishText;

    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    private void OnEnable()
    {
        LocalizationManager.OnLanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        LocalizationManager.OnLanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (textMesh == null) return;

        if (LocalizationManager.Instance != null && LocalizationManager.Instance.CurrentLanguage == Language.English)
        {
            textMesh.text = englishText;
        }
        else
        {
            textMesh.text = russianText;
        }
    }
}