using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : IPlayer, IAIPlayer
{
    private string playerName;
    private PlayerType playerType;
    private PlayerHand hand;
    private Deck deck;

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
        List<ICard> cardsInHand = hand.GetCardsInHand();

        foreach (ICard card in cardsInHand)
        {
            List<IBoardSlot> availableSlots = board.GetEligibleSlots(card);

            if (availableSlots.Count > 0)
            {
                // Scegli un slot casuale tra quelli disponibili
                int randomSlotIndex = Random.Range(0, availableSlots.Count);
                IBoardSlot slotToPlaceCard = availableSlots[randomSlotIndex];

                // Notifichiamo che vogliamo giocare la carta in quello slot
                EventManager.TriggerEvent(EventType.OnTryPlayCard, card, slotToPlaceCard);

                // Notifichiamo anche che abbiamo finito il turno
                EventManager.TriggerEvent(EventType.OnEndTurn);

                // Se hai eseguito un'azione, esci dal metodo PlayTurn
                return;
            }
        }

        // Notifichiamo anche che abbiamo finito il turno
        EventManager.TriggerEvent(EventType.OnEndTurn);
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
        if (deck.CanDrawCard() && hand.CanAddCartToHand())
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
