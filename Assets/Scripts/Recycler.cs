using Assets.Scripts;
using DefaultNamespace;
using PrimeTween;
using System.Collections;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    [SerializeField] private Sprite woringSprite;
    [SerializeField] private Sprite idleSprite;

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
            yield return Tween.ShakeLocalPosition(transform, new Vector3(0.5f, 0.5f, 0), 3);

            GameServices.Instance.MoneyManager.AddBalance(1);
        }

        spriteRenderer.sprite = idleSprite;

        currentState = State.Idle;
    }
}
