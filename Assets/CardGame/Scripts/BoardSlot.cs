using UnityEngine;
using UnityEngine.EventSystems;

public class BoardSlot : MonoBehaviour, IDropHandler
{
    public string slotName;
    public bool isPlayerSlot;
    public bool isBusy;

    public void OnDrop(PointerEventData eventData)
    {
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggable != null && isPlayerSlot && !isBusy)
        {
            // La carta è stata rilasciata sopra questo pannello
            // Puoi implementare la tua logica di drop qui
            //draggable.OnEndDrag(eventData);
            //Debug.Log("SOPRA AL PANNELLO DEL PLAYER");
        }
    }
}
