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
            timerText.text = $"Время обновления - {time}";
        }

        public void Dispose()
        {
            timer.TimeChanged -= UpdateUI;
        }
    }
}