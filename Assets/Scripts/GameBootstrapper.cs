using System.Collections;
using UnityEngine;
using YG;

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
            YG2.onGetSDKData += HandleSDKReady;
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= HandleSDKReady;
        }

        private void Start()
        {
            if (YG2.isSDKEnabled)
            {
                HandleSDKReady();
            }
            else
            {
                StartCoroutine(FallbackTimeoutRoutine(3.5f));
            }
        }

        private IEnumerator FallbackTimeoutRoutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            if (!_isInitialized)
            {
                HandleSDKReady();
            }
        }

        private void HandleSDKReady()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            try
            {
#if YG2_LOCALIZATION
                YG2.SwitchLang(YG2.lang);
                if (LocalizationManager.Instance != null)
                {
                    LocalizationManager.Instance.SetLanguage(
                        (YG2.lang == "en") ? Language.English : Language.Russian);
                }
#endif

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
            YG2.GameReadyAPI();

            // 4. Небольшая задержка перед включением баннера (чтобы запросы не накладывались)
            yield return new WaitForSecondsRealtime(0.2f);
            YG2.StickyAdActivity(true);
        }
    }
}
