using Assets.Scripts;
using DefaultNamespace;
using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;

public class Shradder : MonoBehaviour
{
    [SerializeField] private Trashcan trashcan;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private Transform list;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private int maxEnergy = 100;
    public int CurrentEnergy { get; private set; }

    private State _currentState; 
    public State CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            UpdateText(value);
        }
    }

    private void Awake()
    {
        CurrentEnergy = maxEnergy;
        CurrentState = State.Idle;
    }

    public void Activate()
    {
        if (CurrentState != State.Idle) return;

        CurrentState = State.Woriking;

        GameServices.Instance.DocumentStorage.RemoveDocument();

        StartCoroutine(ActivateCoroutine());
    }
    public void Rechange()
    {
        CurrentState = State.Idle;

        CurrentEnergy = maxEnergy;
    } 

    private IEnumerator ActivateCoroutine()
    {
        list.gameObject.SetActive(true);

        yield return Tween.Position(list, startPoint.position, endPoint.position, duration: 5f, ease: Ease.InSine);

        list.gameObject.SetActive(false);

        CurrentEnergy--;

        trashcan.AddTrash();

        if (CurrentEnergy > 0) CurrentState = State.Idle;
        else CurrentState = State.Discharged;
    }

    private void UpdateText(State state)
    {
        switch (state)
        {
            case State.Idle:
                stateText.text = "Простаивает";
                stateText.color = Color.green;
                break;
            case State.Woriking:
                stateText.text = "Работает";
                stateText.color = Color.yellow;
                break;
            case State.Discharged:
                stateText.text = "Разряжен";
                stateText.color = Color.red;
                break;
        }
    }

    private void OnMouseEnter()
    {
        stateText.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        stateText.gameObject.SetActive(false);
    }
}
