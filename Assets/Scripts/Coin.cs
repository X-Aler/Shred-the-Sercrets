using DefaultNamespace;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int time;

    private bool initialized = false;

    public void Initialize()
    {
        initialized = true;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.coinSound);

        StartCoroutine(WaitCoroutine());
    }

    private void Update()
    {
        if (!initialized) return;

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(time);
        GameServices.Instance.MoneyManager.AddBalance(1);
        Destroy(gameObject);
    }
}
