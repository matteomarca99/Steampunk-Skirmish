using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : IDeck
{
    private List<ICard> cards;

    public Deck(List<CardData> initialCardsData)
    {
        cards = new List<ICard>();

        foreach (CardData data in initialCardsData)
        {
            ICard newCard = new Card(data);

            cards.Add(newCard);
        }

        // Dopo aver inizializzato il deck lo mischiamo, in modo che le carte non siano in ordine sequenziale
        Shuffle();
    }

    public void Shuffle()
    {
        cards = cards.OrderBy(x => Random.value).ToList();
    }

    public bool CanDrawCard()
    {
        return cards.Count > 0;
    }

    public ICard DrawCard()
    {
        if (CanDrawCard())
        {
            ICard drawnCard = cards.FirstOrDefault();

            if (drawnCard != null)
            {
                cards.Remove(drawnCard);
            }

            return drawnCard;
        }
        return null;
    }

    public List<ICard> GetCards()
    {
        return cards;
    }

    public void SetCardsOwner(IPlayer owner)
    {
        cards.ForEach(card => card.CardOwner = owner);
    }
}
