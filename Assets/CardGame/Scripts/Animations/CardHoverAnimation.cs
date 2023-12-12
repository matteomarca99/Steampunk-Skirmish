using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 rectTransform;
    private Tween hoverTween; // Riferimento all'animazione di hover in corso
    public float hoverScale = 1.2f; // Fattore di scala durante il passaggio del mouse

    void Start()
    {
        rectTransform = transform.localScale;
        //originalSizeDelta = rectTransform.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Interrompe l'animazione precedente se ne esiste una in corso
        if (hoverTween != null && hoverTween.IsActive())
        {
            hoverTween.Kill(); // Interrompe l'animazione in corso
        }

        hoverTween = transform.DOScale(rectTransform * hoverScale, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverTween != null && hoverTween.IsActive())
        {
            hoverTween.Kill(); // Interrompe l'animazione in corso
        }

        hoverTween = transform.DOScale(rectTransform, 0.3f);
    }

    // Metodo per interrompere l'animazione manualmente
    void OnDisable()
    {
        if (hoverTween != null && hoverTween.IsActive())
        {
            hoverTween.Kill(); // Interrompe l'animazione in corso
        }
    }
}
