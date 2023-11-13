using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 rectTransform;
    private Vector2 originalSizeDelta;
    public float hoverScale = 1.2f; // Fattore di scala durante il passaggio del mouse

    void Start()
    {
        rectTransform = transform.localScale;
        //originalSizeDelta = rectTransform.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(rectTransform * hoverScale, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(rectTransform, 0.3f);
    }
}
