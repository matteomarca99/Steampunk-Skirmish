using UnityEngine;
using System;

public class BoardSlot : MonoBehaviour, IBoardSlot
{
    private ICard cardInSlot; // Riferimento alla carta presente nello slot

    public PlayerType slotOwner;

    public PlacementType placementType;

    public bool CanDropCard(ICard card)
    {
        // Logica per verificare se la carta può essere posizionata in questa cella
        return cardInSlot == null 
            && card.CardData.cardPlacement == placementType 
            && card.CardOwner.GetPlayerType() == slotOwner;
    }

    public void DropCard(ICard card)
    {
        if (CanDropCard(card))
        {
            cardInSlot = card; // Assegna la carta allo slot
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

    public ICard GetCardInSlot()
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
