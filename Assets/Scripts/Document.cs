using DefaultNamespace;
using System.Collections.Generic;
using UnityEngine;

public class Document : DraggableItem
{
    private struct ShredderZone
    {
        public Collider2D Collider;
        public Shradder Shredder;

        public ShredderZone(Collider2D collider, Shradder shredder)
        {
            Collider = collider;
            Shredder = shredder;
        }
    }

    private readonly List<ShredderZone> overlappingShredderZones = new(3);
    private Shradder dropShradder;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ShrederZone") || !collision.TryGetComponent(out Shradder shradder))
            return;

        for (int i = 0; i < overlappingShredderZones.Count; i++)
        {
            if (overlappingShredderZones[i].Collider != collision)
                continue;

            overlappingShredderZones[i] = new ShredderZone(collision, shradder);
            RefreshDropTarget();
            return;
        }

        overlappingShredderZones.Add(new ShredderZone(collision, shradder));
        RefreshDropTarget();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("ShrederZone"))
            return;

        for (int i = overlappingShredderZones.Count - 1; i >= 0; i--)
        {
            if (overlappingShredderZones[i].Collider != collision)
                continue;

            overlappingShredderZones.RemoveAt(i);
            break;
        }

        RefreshDropTarget();
    }

    private void RefreshDropTarget()
    {
        for (int i = overlappingShredderZones.Count - 1; i >= 0; i--)
        {
            ShredderZone zone = overlappingShredderZones[i];

            if (zone.Collider == null || !zone.Collider.enabled || !zone.Collider.gameObject.activeInHierarchy || zone.Shredder == null)
            {
                overlappingShredderZones.RemoveAt(i);
                continue;
            }

            dropShradder = zone.Shredder;
            inZone = true;
            return;
        }

        dropShradder = null;
        inZone = false;
    }

    protected override void OnSuccessfulDrop()
    {
        RefreshDropTarget();

        if (dropShradder != null)
        {
            dropShradder.Activate();
        }

        base.OnSuccessfulDrop();
    }
}
