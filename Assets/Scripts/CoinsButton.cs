using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DefaultNamespace;
using YG;

public class CoinsButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button coinsButton;

    [Header("Settings")]
    [SerializeField] private float initialWaitTime = 300f; // 5 минут для первого раза
    [SerializeField] private float regularWaitTime = 600f; // 10 минут для последующих
    [SerializeField] private float buffDuration = 300f;    // 5 минут длится бафф

    private float currentWaitTime;

    // Ссылки на запущенные корутины, чтобы можно было их безопасно остановить
    private Coroutine waitCoroutine;
    private Coroutine buffCoroutine;

    private void Awake()
    {
        // Базовая безопасность: проверяем, что в инспекторе всё назначено
        if (coinsButton == null || timerText == null)
        {
            Debug.LogError("[CoinsButton] UI элементы не назначены в инспекторе!", this);
            return;
        }

        currentWaitTime = initialWaitTime;

        // Скрываем элементы на старте
        coinsButton.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // Подписываемся на кнопку
        coinsButton.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        // Запускаем ожидание первой кнопки
        StartWaitCooldown();
    }

    private void OnDestroy()
    {
        // БЕЗОПАСНОСТЬ: Обязательно отписываемся от событий при удалении объекта, 
        // чтобы избежать утечек памяти (Memory Leaks)
        if (coinsButton != null)
        {
            coinsButton.onClick.RemoveListener(OnClick);
        }
    }

    private void StartWaitCooldown()
    {
        // БЕЗОПАСНОСТЬ: Останавливаем предыдущую корутину, если она почему-то работает, 
        // чтобы они не плодились и не вызывали кнопку дважды
        if (waitCoroutine != null)
            StopCoroutine(waitCoroutine);

        waitCoroutine = StartCoroutine(WaitCoroutine());
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(currentWaitTime);

        // После первого срабатывания меняем время на регулярное (600 сек)
        currentWaitTime = regularWaitTime;

        coinsButton.gameObject.SetActive(true);
    }

    private void OnClick()
    {
        // БЕЗОПАСНОСТЬ: защита от двойного клика или клика по скрытой кнопке
        if (!coinsButton.gameObject.activeSelf) return;

        coinsButton.gameObject.SetActive(false);

        YG2.RewardedAdvShow("coins");

        // Запускаем сам бафф
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        buffCoroutine = StartCoroutine(TimerCoroutine());

        // Сразу запускаем кулдаун на появление СЛЕДУЮЩЕЙ кнопки
        // (если нужно, чтобы таймер 600 сек начинался только ПОСЛЕ окончания баффа, 
        // то перенеси StartWaitCooldown() в самый конец TimerCoroutine)
        StartWaitCooldown();
    }

    private IEnumerator TimerCoroutine()
    {
        // БЕЗОПАСНОСТЬ: Проверяем, существует ли синглтон перед обращением к нему
        if (GameServices.Instance != null && GameServices.Instance.MoneyManager != null)
        {
            GameServices.Instance.MoneyManager.ADMultipler = 2;
        }
        else
        {
            Debug.LogWarning("[CoinsButton] GameServices или MoneyManager не найдены!");
        }

        timerText.gameObject.SetActive(true);

        float timeLeft = buffDuration;

        // Визуально обновляем таймер каждый кадр, а не просто ждем 300 секунд
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            // Форматируем время в ММ:СС
            int minutes = Mathf.FloorToInt(timeLeft / 60F);
            int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);
            timerText.text = string.Format("2X ({0:00}:{1:00})", minutes, seconds);

            yield return null; // Ждем до следующего кадра
        }

        // Возвращаем множитель обратно
        if (GameServices.Instance != null && GameServices.Instance.MoneyManager != null)
        {
            GameServices.Instance.MoneyManager.ADMultipler = 1;
        }

        timerText.gameObject.SetActive(false);
    }
}