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

            UpdateUI(timer.TimeLeft);

            timer.TimeChanged += UpdateUI;
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
        }
    }
}