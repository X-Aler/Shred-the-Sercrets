using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TrashcanVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Sprite trashcanFullSprite;
    [SerializeField] private Sprite trashcanHalfSprite;
    [SerializeField] private Sprite trashcanSmallSprite;
    [SerializeField] private Sprite trashcanEmptySprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateVisuals(int currentCount, int capacity)
    {
        UpdateSprite(currentCount, capacity);
        UpdateText(currentCount, capacity);
    }

    private void UpdateSprite(int currentCount, int capacity)
    {
        if (currentCount == capacity)
        {
            spriteRenderer.sprite = trashcanFullSprite;
            return;
        }

        float fillPercentage = (float)currentCount / capacity;

        if (fillPercentage >= 0 && fillPercentage < 0.35f)
        {
            spriteRenderer.sprite = trashcanEmptySprite;
        }
        else if (fillPercentage >= 0.35f && fillPercentage < 0.75f)
        {
            spriteRenderer.sprite = trashcanSmallSprite;
        }
        else
        {
            spriteRenderer.sprite = trashcanHalfSprite;
        }
    }

    private void UpdateText(int currentCount, int capacity)
    {
        countText.text = $"{currentCount}/{capacity}";
        countText.color = currentCount == capacity ? Color.red : Color.white;
    }

    private void OnMouseEnter()
    {
        countText.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        countText.gameObject.SetActive(false);
    }
}