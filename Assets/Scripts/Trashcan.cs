using UnityEngine;

public class Trashcan : DraggableItem
{
    [SerializeField] private TrashcanVisualizer visualizer;

    [SerializeField] private int trashCapacity = 10;

    private Vector2 location;

    private Recycler recycler;

    private int trashCount;
    public int TrashCount
    {
        get => trashCount;
        private set
        {
            int clampedValue = Mathf.Clamp(value, 0, trashCapacity);

            if (trashCount != clampedValue)
            {
                trashCount = clampedValue;
                visualizer.UpdateVisuals(trashCount, trashCapacity);
            }
        }
    }

    protected override void Start()
    {
        visualizer.UpdateVisuals(trashCount, trashCapacity);
        location = transform.localPosition;
        base.Start();
    }

    public bool AddTrash()
    {
        if (TrashCount >= trashCapacity) return false;

        TrashCount++;
        return true;
    }

    public void Clear() => TrashCount = 0;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RecyclerZone"))
        {
            recycler = collision.GetComponent<Recycler>();
            inZone = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("RecyclerZone"))
        {
            recycler = null;
            inZone = false;
        }
    }

    protected override void OnSuccessfulDrop()
    {
        if (recycler.Activate(trashCount))
            Clear();

        transform.localPosition = location;
    }

    protected override void OnFailedDrop()
    {
        transform.localPosition = location;
    }
}