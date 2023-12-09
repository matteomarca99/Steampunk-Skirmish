using System.Collections.Generic;

public class Player : IPlayer
{
    private string playerName;
    private PlayerType playerType;
    private PlayerHand hand;
    private Deck deck;

    private bool canPlay;

    public Player(string playerName, PlayerType playerType, Deck playerDeck)
    {
        this.playerName = playerName;
        this.playerType = playerType;
        hand = new PlayerHand();
        this.deck = playerDeck;

        // Inoltre impostiamo le carte del deck come di proprieta' del giocatore
        playerDeck.SetCardsOwner(this);
    }

    public void AddCardToHand(IVisualCard visualCard)
    {
        hand.AddCardToHand(visualCard);
    }

    public void RemoveCardFromHand(IVisualCard visualCard)
    {
        hand.RemoveCardFromHand(visualCard);
    }

    public List<IVisualCard> GetCardsInHand()
    {
        return hand.GetCardsInHand();
    }

    public void DrawCardFromDeck()
    {
        if(CanDrawCardFromDeck())
            hand.AddCardToHand(deck.DrawCard());
    }

    public void DrawCardsFromDeck(int n)
    {
        for (int i = 0; i < n; i++)
        {
            DrawCardFromDeck();
        }
    }

    public bool CanDrawCardFromDeck()
    {
        return deck.CanDrawCard() && hand.CanAddCartToHand();
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

    public bool CanPlay { get => canPlay; set => canPlay = value; }
}