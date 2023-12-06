using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour, IBoard
{
    public List<BoardSlot> boardSlots;

    public bool CanPlaceCard(ICard card, IBoardSlot slot)
    {
        // Logica per verificare se la carta può essere posizionata in una specifica posizione sulla board
        return slot != null && slot.CanDropCard(card);
    }

    public void PlaceCard(ICard card, IBoardSlot slot)
    {
        // Logica per posizionare effettivamente la carta sulla board e quindi nello slot corrispondente
        if(CanPlaceCard(card, slot))
            slot.DropCard(card);
    }

    public ICard SelectCard(IBoardSlot slot)
    {
        return slot.GetCardInSlot();
    }

    public List<IBoardSlot> GetBoardSlots()
    {
        return boardSlots.Cast<IBoardSlot>().ToList();
    }

    public List<IBoardSlot> GetEligibleSlots(ICard card)
    {
        return boardSlots.Where(slot => CanPlaceCard(card, slot)).ToList<IBoardSlot>();
    }
}