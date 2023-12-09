using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : IPlayer, IAIPlayer
{
    private string playerName;
    private PlayerType playerType;
    private PlayerHand hand;
    private Deck deck;

    private bool canPlay;

    public AIPlayer(string playerName, PlayerType playerType, Deck playerDeck)
    {
        this.playerName = playerName;
        this.playerType = playerType;
        hand = new PlayerHand();
        this.deck = playerDeck;

        // Inoltre impostiamo le carte del deck come di proprieta' del giocatore
        playerDeck.SetCardsOwner(this);
    }

    public void PlayTurn(IBoard board)
    {
        if (canPlay)
        {
            List<IVisualCard> cardsInHand = hand.GetCardsInHand();

            foreach (IVisualCard card in cardsInHand)
            {
                List<IBoardSlot> availableSlots = board.GetEligibleSlots(card);

                if (availableSlots.Count > 0)
                {
                    // Scegli un slot casuale tra quelli disponibili
                    int randomSlotIndex = Random.Range(0, availableSlots.Count);
                    IBoardSlot slotToPlaceCard = availableSlots[randomSlotIndex];

                    // Notifichiamo che vogliamo giocare la carta in quello slot
                    EventManager.TriggerEvent(EventType.OnTryPlayCard, card, slotToPlaceCard);

                    // Se hai eseguito un'azione, esci dal metodo PlayTurn
                    break;
                }
            }

            // Notifichiamo anche che abbiamo finito il turno
            EventManager.TriggerEvent(EventType.OnEndTurn);
        }
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
        if (CanDrawCardFromDeck())
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
