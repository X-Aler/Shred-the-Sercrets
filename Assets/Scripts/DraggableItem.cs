using UnityEngine;

public abstract class DraggableItem : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    protected bool inZone;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    public void ForceStartDrag()
    {
        isDragging = true;
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        inZone = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        inZone = false;
    }

    private void CheckDrop()
    {
        if (inZone)
        {
            OnSuccessfulDrop();
        }
        else
        {
            OnFailedDrop();
        }
    }

    protected virtual void OnSuccessfulDrop()
    {
        Destroy(gameObject);
    }

    protected virtual void OnFailedDrop()
    {
        Destroy(gameObject);
    }
}