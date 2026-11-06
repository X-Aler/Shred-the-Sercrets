using System.Collections;
using UnityEngine;
using GamePlatform;

namespace DefaultNamespace
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("UI Screens")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private HudScreenController hudScreen;

        [Header("Gameplay Systems")]
        [SerializeField] private DocumentSpawner documentSpawner;
        [SerializeField] private GameData gameData;

        private bool _isInitialized;

        private void Awake()
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(true);

            GameServices.Initialize();
        }

        private void OnEnable()
        {
            PlatformServices.Ready += HandleSDKReady;
        }

        private void OnDisable()
        {
            PlatformServices.Ready -= HandleSDKReady;
        }

        private void Start() { PlatformServices.Initialize(); if(PlatformServices.IsReady) HandleSDKReady(); }

        private void OnApplicationPause(bool paused) { if (paused && _isInitialized) SaveManager.SaveAllData(); }
        private void OnApplicationFocus(bool focused) { if (!focused && _isInitialized) SaveManager.SaveAllData(); }

        private void HandleSDKReady()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            try
            {
                GameServices.Instance.MoneyManager.SetBalance(PlatformServices.Progress.coins);
                UpgradeManager.Instance?.LoadLevels();

                if (gameData != null) gameData.Initialize();
                if (hudScreen != null) hudScreen.Initialize();
                if (documentSpawner != null) documentSpawner.Initialize();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GameBootstrapper] Error during system initialization: {ex}");
            }
            finally
            {
                // Запускаем финализацию показа
                StartCoroutine(FinalizeLoadingRoutine());
            }
        }

        private IEnumerator FinalizeLoadingRoutine()
        {
            // 1. Выключаем экран загрузки
            if (loadingScreen != null)
                loadingScreen.SetActive(false);

            // 2. Ждем завершения отрисовки текущего кадра, чтобы браузер реально отобразил готовый UI
            yield return new WaitForEndOfFrame();

            // 3. Отправляем сигнал готовности платформе
            PlatformServices.Service.GameReady();
            PlatformServices.Service.GameplayStart();

            // 4. Небольшая задержка перед включением баннера (чтобы запросы не накладывались)
            yield return new WaitForSecondsRealtime(0.2f);

        }
    }
}
