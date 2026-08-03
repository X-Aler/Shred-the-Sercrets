using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class DocumentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject documents;

    public void Initialize()
    {
        StartCoroutine(SpawnCoroutine());
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

            var currentCount = documentStorage.CurrentDocumentsCount;
            var countToSpawn = currentCount / DocumentStorage.DocumentsInOneStack;

            for (var i = 1; i <= countToSpawn; i++)
            {
                var document = documents.transform.Find(i.ToString());

                if (document && !document.gameObject.activeSelf)
                {
                    document.gameObject.SetActive(true);
                }
            }

            yield return StartCoroutine(WaitTimerCoroutine());
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