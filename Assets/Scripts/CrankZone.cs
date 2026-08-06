using UnityEngine;

public class CrankZone : MonoBehaviour
{
    [SerializeField] private Crank crank;

    private void OnMouseDown()
    {
        crank.StartDrag();
    }
    private void OnMouseUp()
    {
        crank.StopDrag();
    }
}
