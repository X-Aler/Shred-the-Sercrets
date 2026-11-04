using System;
using TMPro;
using YG;

namespace DefaultNamespace
{
    public class DocumentTimerUI : IDisposable
    {
        private readonly TMP_Text timerText;
        private readonly DocumentTimer timer;

        public DocumentTimerUI(TMP_Text timerText)
        {
            this.timerText = timerText;
            timer = GameServices.Instance.DocumentTimer;

            // Сразу отрисовываем актуальное состояние
            UpdateUI(timer.TimeLeft);

            // Подписываемся на тики таймера
            timer.TimeChanged += UpdateUI;

            // Подписываемся на смену языка PluginYG2, чтобы текст менялся мгновенно без ожидания тика
#if YG2_LOCALIZATION
            YG2.onSwitchLang += OnLanguageChanged;
#endif
        }

        private void OnLanguageChanged(string lang)
        {
            UpdateUI(timer.TimeLeft);
        }

        private void UpdateUI(int time)
        {
            // Проверяем язык напрямую через YG2 (или через ваш LocalizationManager, если он синхронизирован)
#if YG2_LOCALIZATION
            bool isEnglish = YG2.lang == "en";
#else
            bool isEnglish = LocalizationManager.Instance != null &&
                             LocalizationManager.Instance.CurrentLanguage == Language.English;
#endif

            string label = isEnglish ? "Refresh time" : "Время обновления";
            timerText.text = $"{label} - {time}";
        }

        public void Dispose()
        {
            timer.TimeChanged -= UpdateUI;

#if YG2_LOCALIZATION
            YG2.onSwitchLang -= OnLanguageChanged;
#endif
        }
    }
}