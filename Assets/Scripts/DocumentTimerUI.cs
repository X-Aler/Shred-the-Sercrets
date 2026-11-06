using System;
using TMPro;


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

            LocalizationManager.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            UpdateUI(timer.TimeLeft);
        }

        private void UpdateUI(int time)
        {

            bool isEnglish = LocalizationManager.Instance != null &&
                             LocalizationManager.Instance.CurrentLanguage == Language.English;


            string label = isEnglish ? "Refresh time" : "Время обновления";
            timerText.text = $"{label} - {time}";
        }

        public void Dispose()
        {
            timer.TimeChanged -= UpdateUI;

            LocalizationManager.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}