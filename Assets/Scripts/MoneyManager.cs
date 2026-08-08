using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoneyManager
    {
        public int Balance { get; private set; }

        public int IncomeMultiplier { get; set; } = 1;

        public int ADMultipler { get; set; } = 1;

        public event Action<int> BalanceChanged;

        public void AddBalance(int amount)
        {
            int finalAmount = (amount * IncomeMultiplier) * ADMultipler;

            Balance += finalAmount;

            BalanceChanged?.Invoke(Balance);
        }

        public void RemoveBalance(int amount)
        {
            if (Balance < amount) return;

            Balance -= amount;

            BalanceChanged?.Invoke(Balance);
        }

        public void SetBalance(int newBalance)
        {
            Balance = newBalance;
            BalanceChanged?.Invoke(Balance);
        }
    }
}