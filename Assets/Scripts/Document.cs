using DefaultNamespace;
using UnityEngine;

public class Document : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = true;
    private bool isInZone = false;

    private Shradder dropShradder = null;

    private void Start()
    {
        mainCamera = Camera.main;
        UpdatePositionToMouse();
    }

    private void Update()
    {
        if (isDragging)
        {
            UpdatePositionToMouse();

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                CheckDrop();
            }
        }
    }

    private void UpdatePositionToMouse()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = mousePosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShrederZone"))
        {
            dropShradder = collision.gameObject.GetComponent<Shradder>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ShrederZone"))
        {
            dropShradder = null;
        }
    }

    private void CheckDrop()
    {
        if (dropShradder)
        {
            OnSuccessfulDrop();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSuccessfulDrop()
    {
        GameServices.Instance.DocumentStorage.RemoveDocument();

        dropShradder.Activate();
    }
}