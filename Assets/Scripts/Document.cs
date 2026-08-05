using DefaultNamespace;
using UnityEngine;

public class Document : DraggableItem
{
    private Shradder dropShradder = null;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShrederZone"))
        {
            dropShradder = collision.GetComponent<Shradder>();
            inZone = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ShrederZone"))
        {
            dropShradder = null;
            inZone = false;
        }
    }

    protected override void OnSuccessfulDrop()
    {
        if (dropShradder != null)
        {
            dropShradder.Activate();
        }

        base.OnSuccessfulDrop();
    }
}