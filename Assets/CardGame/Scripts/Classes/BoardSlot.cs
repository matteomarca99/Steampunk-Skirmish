using UnityEngine;
using System;

public class BoardSlot : MonoBehaviour, IBoardSlot
{
    [SerializeField] private IVisualCard cardInSlot; // Riferimento alla carta visuale presente nello slot

    [SerializeField] private PlayerType slotOwner;

    [SerializeField] private PlacementType placementType;

    [SerializeField] private SlotType slotType;

    public bool CanDropCard(IVisualCard visualCard)
    {
        if (cardInSlot != null || visualCard.GetCard().CardOwner.GetPlayerType() != slotOwner || visualCard.GetCard().CardData.cardPlacement != placementType)
        {
            return false;
        }

        if (slotType == SlotType.Zone)
        {
            // Se lo slot è di tipo Zona, controlla se la carta non è nella mano del giocatore
            if (visualCard.GetCard().IsInHand)
            {
                return false;
            }
        }
        return true;
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

    public SlotType GetSlotType()
    {
        return slotType;
    }
}
