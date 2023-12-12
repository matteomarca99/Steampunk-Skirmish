using System.Collections.Generic;

public class PlayerHand : IPlayerHand
{
    private List<IVisualCard> cardsInHand;
    private readonly int maxCardsInHand = 8;

    public PlayerHand()
    {
        cardsInHand = new List<IVisualCard>();
    }

    public bool CanAddCartToHand()
    {
        return cardsInHand.Count < maxCardsInHand;
    }

    public void AddCardToHand(IVisualCard visualCard)
    {
        if (CanAddCartToHand())
        {
            cardsInHand.Add(visualCard);
            visualCard.GetCard().IsInHand = true;
        }
    }

    public void RemoveCardFromHand(IVisualCard visualCard)
    {
        cardsInHand.Remove(visualCard);
        visualCard.GetCard().IsInHand = false;
    }

    public List<IVisualCard> GetCardsInHand()
    {
        return cardsInHand;
    }
}