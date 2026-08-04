using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameServices
    {
        public static GameServices Instance { get; private set; }
        public MoneyManager MoneyManager { get; private set; }
        public DocumentStorage DocumentStorage { get; private set; }
        public DocumentTimer DocumentTimer { get; private set; }
        public static void Initialize()
        {
            if (Instance != null) return;

            Instance = new GameServices();

            Instance.CreateModules();
        }
        private void CreateModules()
        {
            MoneyManager = new MoneyManager();
            DocumentStorage = new DocumentStorage();
            DocumentTimer = new DocumentTimer();
        }
    }
}