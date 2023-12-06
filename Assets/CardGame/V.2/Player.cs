using System.Collections.Generic;

public class Player : IPlayer
{
    private string playerName;
    private PlayerType playerType;
    private PlayerHand hand;
    private Deck deck;

    public Player(string playerName, PlayerType playerType, Deck playerDeck)
    {
        this.playerName = playerName;
        this.playerType = playerType;
        hand = new PlayerHand();
        this.deck = playerDeck;

        // Inoltre impostiamo le carte del deck come di proprieta' del giocatore
        playerDeck.SetCardsOwner(this);
    }

    public void AddCardToHand(ICard card)
    {
        hand.AddCardToHand(card);
    }

    public void RemoveCardFromHand(ICard card)
    {
        hand.RemoveCardFromHand(card);
    }

    public List<ICard> GetCardsInHand()
    {
        return hand.GetCardsInHand();
    }

    public void DrawCardFromDeck()
    {
        if(deck.CanDrawCard() && hand.CanAddCartToHand())
            hand.AddCardToHand(deck.DrawCard());
    }

    public void DrawCardsFromDeck(int n)
    {
        for (int i = 0; i < n; i++)
        {
            DrawCardFromDeck();
        }
    }


    public List<ICard> GetCardsInDeck()
    {
        return deck.GetCards();
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public PlayerType GetPlayerType()
    {
        return playerType;
    }
}