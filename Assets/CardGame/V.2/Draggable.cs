using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private IVisualCard visualCard;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        visualCard = GetComponent<IVisualCard>();
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

        // Notifichiamo che stiamo effettuando il drag di una carta
        EventManager.TriggerEvent(EventType.OnCardDrag, visualCard.GetCard());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //CardsGameManager.Instance.DisableAllSlotImages();
        canvasGroup.blocksRaycasts = true;

        GameObject dropZone = eventData.pointerEnter;

        IBoardSlot slot = dropZone?.GetComponent<IBoardSlot>();

        // Se il drag finisce, proviamo a giocare la carta
        EventManager.TriggerEvent(EventType.OnTryPlayCard, visualCard.GetCard(), slot);
    }
}
