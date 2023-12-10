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

    public IBoardSlot FindSlotByCard(IVisualCard visualCard)
    {
        return boardSlots.FirstOrDefault(slot => slot.GetCardInSlot() == visualCard);
    }

    public List<IBoardSlot> GetBoardSlots()
    {
        return boardSlots.Cast<IBoardSlot>().ToList();
    }

    public List<IBoardSlot> GetSlotsOfType(SlotType slotType)
    {
        return boardSlots.Where(slot => slot.GetSlotType() == slotType).Cast<IBoardSlot>().ToList();
    }

    public List<IVisualCard> GetVisualCards()
    {
        return boardSlots.Select(slot => slot.GetCardInSlot())
                .Where(visualCard => visualCard != null)
                .ToList();
    }

    public List<IVisualCard> GetVisualCardsOfPlayer(IPlayer owner)
    {
        return GetVisualCards().Where(visualCard => visualCard.GetCard().CardOwner == owner).ToList();
    }

    public List<IBoardSlot> GetEligibleSlots(IVisualCard visualCard)
    {
        return boardSlots.Where(slot => CanPlaceCard(visualCard, slot)).ToList<IBoardSlot>();
    }

    public ZoneStatus GetZoneStatus()
    {
        List<IBoardSlot> zoneSlots = GetSlotsOfType(SlotType.Zone);

        int playerCount = zoneSlots.Count(slot => slot.GetCardInSlot() != null && slot.GetSlotOwner() == PlayerType.Player);
        int opponentCount = zoneSlots.Count(slot => slot.GetCardInSlot() != null && slot.GetSlotOwner() == PlayerType.Opponent);

        if (playerCount > 0 || opponentCount > 0)
        {
            if (playerCount > opponentCount)
            {
                return ZoneStatus.PlayerControlled;
            }
            else if (opponentCount > playerCount)
            {
                return ZoneStatus.OpponentControlled;
            }
            else
            {
                return ZoneStatus.Contested;
            }
        }
        else
        {
            return ZoneStatus.Neutral;
        }
    }
}