using System.Collections.Generic;

public class PlayerHand : IPlayerHand
{
    private List<ICard> cardsInHand;
    private readonly int maxCardsInHand = 8;

    public PlayerHand()
    {
        cardsInHand = new List<ICard>();
    }

    public bool CanAddCartToHand()
    {
        return cardsInHand.Count < maxCardsInHand;
    }

    public void AddCardToHand(ICard card)
    {
        if (CanAddCartToHand())
        {
            cardsInHand.Add(card);
            card.IsInHand = true;
        }
    }

    public void RemoveCardFromHand(ICard card)
    {
        cardsInHand.Remove(card);
        card.IsInHand = false;
    }

    public List<ICard> GetCardsInHand()
    {
        return cardsInHand;
    }
}