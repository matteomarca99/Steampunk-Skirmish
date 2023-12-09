using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTargeting : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private IVisualCard attackingVisualCard;

    void Start()
    {
        attackingVisualCard = GetComponent<IVisualCard>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ovviamente se la carta non è di proprietà del player, non possiamo fare il targeting
        if (attackingVisualCard.GetCard().CardOwner.GetPlayerType() != PlayerType.Player)
            return;

        Debug.Log("ATTACCANTE: " + attackingVisualCard.GetCard().CardData.name);

        // Notifichiamo l'inizio del targeting
        EventManager.TriggerEvent<IVisualCard>(EventType.OnBeginTargeting, attackingVisualCard);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Raycast per trovare la carta di destinazione sotto il punto di rilascio
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            IVisualCard targetVisualCard = result.gameObject.GetComponent<IVisualCard>();

            if (targetVisualCard != null && targetVisualCard != attackingVisualCard && targetVisualCard.GetCard().CurHealth > 0 && targetVisualCard.GetCard() is IDamageable)
            {
                Debug.Log("ATTACCANTE: " + attackingVisualCard.GetCard().CardData.name + ", TARGET: " + targetVisualCard.GetCard().CardData.name);

                // Notifichiamo il possibile attacco, poi chi ricevera' la notifica si occupera' di effettuarlo
                EventManager.TriggerEvent<IVisualCard, IVisualCard>(EventType.OnTryCardAttack, attackingVisualCard, targetVisualCard);

                break;
            }
        }

        // Notifichiamo la fine del targeting
        EventManager.TriggerEvent<IVisualCard>(EventType.OnEndTargeting, attackingVisualCard);
    }
}
