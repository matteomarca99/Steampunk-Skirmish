using UnityEngine;
using System;

public class BoardSlot : MonoBehaviour, IBoardSlot
{
    private IVisualCard cardInSlot; // Riferimento alla carta visuale presente nello slot

    public PlayerType slotOwner;

    public PlacementType placementType;

    public bool CanDropCard(IVisualCard visualCard)
    {
        // Logica per verificare se la carta può essere posizionata in questa cella
        return cardInSlot == null 
            && visualCard.GetCard().CardData.cardPlacement == placementType 
            && visualCard.GetCard().CardOwner.GetPlayerType() == slotOwner;
    }

    public void DropCard(IVisualCard visualCard)
    {
        if (CanDropCard(visualCard))
        {
            cardInSlot = visualCard; // Assegna la carta allo slot
        }
        else
        {
            // Gestione della posizione non valida per la carta
            // ...
        }
    }

    public void RemoveCard()
    {
        cardInSlot = null;
    }

    public IVisualCard GetCardInSlot()
    {
        return cardInSlot != null ? cardInSlot : null;
    }

    public PlayerType GetSlotOwner()
    {
        return slotOwner;
    }

    public PlacementType GetPlacementType()
    {
        return placementType;
    }
}
