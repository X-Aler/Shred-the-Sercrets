using System.Collections;
using UnityEngine;
using PrimeTween;

public class Shradder : MonoBehaviour
{
    [SerializeField] private Trashcan trashcan;
    [SerializeField] private Transform list;
    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform EndPoint;

    private const int maxEnergy = 100;

    private int currentEnergy;
    public int CurrentEnergy => currentEnergy;

    public void Activate() => StartCoroutine(ActivateCoroutine());

    private IEnumerator ActivateCoroutine()
    {
        list.gameObject.SetActive(true);
        var tween = Tween.Position(list, StartPoint.position, EndPoint.position, duration: 5f, ease: Ease.InSine);
        yield return new WaitForSeconds(tween.duration);
        list.gameObject.SetActive(false);
    }
}
