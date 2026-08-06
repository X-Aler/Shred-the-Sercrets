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

        for (int i = 0; i < trashCount; i++)
        {
            yield return Tween.ShakeLocalPosition(transform, new Vector3(0.1f, 0, 0), 3, 25, true, Ease.InOutSine);

            SpawnCoin();
        }

        spriteRenderer.sprite = idleSprite;

        currentState = State.Idle;
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
