using UnityEngine;
using UnityEngine.UI;

public class Crank : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject crankUI;

    private Camera mainCamera;
    private Shradder shradder;

    private bool isDragging = false;
    private float previousAngle = 0f;
    private float totalRotation = 0f;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging)
        {
            float currentAngle = GetMouseAngle();

            float deltaAngle = Mathf.DeltaAngle(previousAngle, currentAngle);

            if (deltaAngle < 0)
            {
                transform.Rotate(0, 0, deltaAngle);

                totalRotation += Mathf.Abs(deltaAngle);

                if (totalRotation >= 360f)
                {
                    OnFullCircleComplete();
                    totalRotation -= 360f;
                }
            }

            previousAngle = currentAngle;
        }
    }

    public void Activate(Shradder shradder)
    {
        this.shradder = shradder;
        progressBar.value = progressBar.minValue;
        UI.SetActive(true);
        crankUI.SetActive(true);
    }

    private void OnFullCircleComplete()
    {
        if (progressBar.value == progressBar.maxValue)
        {
            StopDrag();
            UI.SetActive(false);
            crankUI.SetActive(false);
            shradder.Rechange();
        }
        else progressBar.value += 0.1f;
    }

    public void StartDrag()
    {
        AudioManager.Instance.PlayLoopingSound(AudioManager.Instance.crankSound, 1);
        isDragging = true;
        previousAngle = GetMouseAngle();
    }

    public void StopDrag()
    {
        isDragging = false;
        AudioManager.Instance.StopLoopingSound(AudioManager.Instance.crankSound);
    }

    private float GetMouseAngle()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return previousAngle;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 direction = Input.mousePosition - screenPos;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
