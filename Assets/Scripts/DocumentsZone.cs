using DefaultNamespace;
using UnityEngine;

public class DocumentsZone : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Collider2D spawnZoneCollider;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || mainCamera == null || spawnZoneCollider == null)
            return;

        var documentStorage = GameServices.Instance?.DocumentStorage;
        if (documentStorage == null || documentStorage.IsDocumentsNull)
            return;

        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

        if (spawnZoneCollider.OverlapPoint(mousePosition2D))
            SpawnItem(mousePosition);
    }

    private void SpawnItem(Vector3 spawnPosition)
    {
        if (itemPrefab == null) return;

        spawnPosition.z = 0f;

        GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        if (spawnedItem.TryGetComponent<DraggableItem>(out DraggableItem draggable))
        {
            draggable.ForceStartDrag();
        }
    }
}
