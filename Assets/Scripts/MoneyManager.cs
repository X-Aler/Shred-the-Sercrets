using System;
using UnityEngine;

public class MoneyManager
{
    private int balance;
    public int Balance => balance;

    public event Action<int> BalanceChanged;
    public void AddBalance(int amount)
    {
        balance += amount;

        BalanceChanged?.Invoke(balance);
    }

    public void RemoveBalance(int amount)
    {
        if (amount > balance) return;

        balance -= amount;

        BalanceChanged?.Invoke(balance);
    }

    public void SetBalance(int newBalance) => balance = newBalance;
}
