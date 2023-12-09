using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour, IBoard
{
    public List<BoardSlot> boardSlots;

    public bool CanPlaceCard(IVisualCard visualCard, IBoardSlot slot)
    {
        // Logica per verificare se la carta può essere posizionata in una specifica posizione sulla board
        return slot != null && slot.CanDropCard(visualCard);
    }

    public void PlaceCard(IVisualCard visualCard, IBoardSlot slot)
    {
        // Logica per posizionare effettivamente la carta sulla board e quindi nello slot corrispondente
        if(CanPlaceCard(visualCard, slot))
            slot.DropCard(visualCard);
    }

    public IVisualCard SelectCard(IBoardSlot slot)
    {
        return slot.GetCardInSlot();
    }

    public List<IBoardSlot> GetBoardSlots()
    {
        return boardSlots.Cast<IBoardSlot>().ToList();
    }

    public List<IVisualCard> GetVisualCards()
    {
        return boardSlots.Select(slot => slot.GetCardInSlot())
                .Where(visualCard => visualCard != null)
                .ToList();
    }

    public List<IBoardSlot> GetEligibleSlots(IVisualCard visualCard)
    {
        return boardSlots.Where(slot => CanPlaceCard(visualCard, slot)).ToList<IBoardSlot>();
    }
}