using Assets.Scripts;
using DefaultNamespace;
using PrimeTween;
using System.Collections;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    [SerializeField] private Sprite woringSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Collider2D coinsSpawnZone;
    [SerializeField] private GameObject coinPrefab;

    [Header("Базовое время переработки (если UpgradeManager отсутствует)")]
    [SerializeField] private float defaultRecycleDuration = 3f;

    private SpriteRenderer spriteRenderer;
    private State currentState;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = State.Idle;
        spriteRenderer.sprite = idleSprite;
    }

    public bool Activate(int trashCount)
    {
        if (currentState != State.Idle) return false;

        currentState = State.Woriking;
        StartCoroutine(ActivateCoroutine(trashCount));

        return true;
    }

    private IEnumerator ActivateCoroutine(int trashCount)
    {
        spriteRenderer.sprite = woringSprite;

        float recycleDuration = GetCurrentRecycleDuration();

        for (int i = 0; i < trashCount; i++)
        {
            yield return Tween.ShakeLocalPosition(transform, new Vector3(0.1f, 0, 0), duration: recycleDuration, frequency: 25, true, Ease.Linear);

            SpawnCoin();
        }

        spriteRenderer.sprite = idleSprite;
        currentState = State.Idle;
    }

    private float GetCurrentRecycleDuration()
    {
        if (UpgradeManager.Instance != null && UpgradeManager.Instance.recycleSpeedData != null)
        {
            int level = UpgradeManager.Instance.RecycleSpeedLevel;
            return UpgradeManager.Instance.recycleSpeedData.GetValue(level);
        }

        return defaultRecycleDuration;
    }

    private void SpawnCoin()
    {
        var bounds = coinsSpawnZone.bounds;
        var randomPosX = Random.Range(bounds.min.x, bounds.max.x);
        var spawnPos = new Vector3(randomPosX, coinsSpawnZone.transform.position.y, 0);

        var coinObj = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        var coin = coinObj.GetComponent<Coin>();
        coin.Initialize();
    }
}