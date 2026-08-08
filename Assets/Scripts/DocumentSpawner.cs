using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class DocumentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject documents;

    public void Initialize()
    {
        // Подписываемся на изменение количества документов
        GameServices.Instance.DocumentStorage.DocumentsUpdated += OnDocumentsUpdated;

        ShowDocuments();
        StartCoroutine(SpawnCoroutine());
    }

    private void OnDestroy()
    {
        // Обязательно отписываемся, чтобы избежать утечек памяти
        if (GameServices.Instance?.DocumentStorage != null)
        {
            GameServices.Instance.DocumentStorage.DocumentsUpdated -= OnDocumentsUpdated;
        }
    }

    // Обработчик события
    private void OnDocumentsUpdated(int current, int capacity)
    {
        ShowDocuments();
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            var documentStorage = GameServices.Instance.DocumentStorage;
            var count = documentStorage.CurrentDocumentsCount;

            if (count >= documentStorage.DocumentsCapacity)
            {
                yield return StartCoroutine(WaitTimerCoroutine());
                continue;
            }

            documentStorage.AddDocument();

            // Вызов ShowDocuments() отсюда можно даже убрать, 
            // так как он всё равно сработает через событие из AddDocument()

            yield return StartCoroutine(WaitTimerCoroutine());
        }
    }

    private void ShowDocuments()
    {
        var currentCount = GameServices.Instance.DocumentStorage.CurrentDocumentsCount;
        var countToSpawn = Mathf.CeilToInt((float)currentCount / DocumentStorage.DocumentsInOneStack);

        // 1. Выключаем все пачки
        foreach (Transform document in documents.transform)
        {
            document.gameObject.SetActive(false);
        }

        // 2. Включаем только нужные (если countToSpawn == 0, цикл просто не выполнится)
        for (var i = 1; i <= countToSpawn; i++)
        {
            var document = documents.transform.Find(i.ToString());

            if (document && !document.gameObject.activeSelf)
            {
                document.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator WaitTimerCoroutine()
    {
        var time = GameServices.Instance.DocumentStorage.DocumentsSpawnTime;
        var timer = GameServices.Instance.DocumentTimer;

        while (time > 0)
        {
            timer.SetTime(time);

            yield return new WaitForSeconds(1f);

            time--;
        }

        timer.SetTime(0);
    }
}