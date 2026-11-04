using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoneyUI : IDisposable
    {
        private static readonly string[] NumberSuffixes = { "", "К", "М", "Б", "Т" };

        private readonly TMP_Text moneyText;
        private readonly MoneyManager moneyManager;

        public MoneyUI(TMP_Text moneyText)
        {
            this.moneyText = moneyText;

            moneyManager = GameServices.Instance.MoneyManager;

            UpdateUI(moneyManager.Balance);

            moneyManager.BalanceChanged += UpdateUI;
        }

        private void UpdateUI(int balance)
        {
            if (moneyText == null) return;

            moneyText.text = FormatNumber(balance);
        }

        private string FormatNumber(double number)
        {
            if (number < 1000)
                return number.ToString("F0");

            int k;

            if (number == 0) k = 0;
            else k = (int)Mathf.Floor((float)Math.Log10(number) / 3);

            k = Mathf.Min(k, NumberSuffixes.Length - 1);

            var shortNumber = number / Math.Pow(10, k * 3);

            return shortNumber.ToString("F1") + NumberSuffixes[k];
        }

        public void Dispose()
        {
            if (moneyManager is null) return;

            moneyManager.BalanceChanged -= UpdateUI;
        }
    }
}
