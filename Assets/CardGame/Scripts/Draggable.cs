using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private CardDataInstance cardDataIstance;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        cardDataIstance = GetComponent<CardDataInstance>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        // Resettiamo la rotazione causata dall'animazione della curvatura della carta
        rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;

        CardsGameManager.Instance.ShowEligibleSlots(cardDataIstance.cardData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CardsGameManager.Instance.DisableAllSlotImages();
        canvasGroup.blocksRaycasts = true;

        GameObject dropZone = eventData.pointerEnter;

        BoardSlot slot = dropZone?.GetComponent<BoardSlot>();

        // Se il drag finisce su uno slot valido, proviamo a giocare la carta
        CardsGameManager.Instance.TryPlayCard(cardDataIstance, slot);
    }
}
