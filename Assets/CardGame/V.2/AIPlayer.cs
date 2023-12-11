using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            List<IVisualCard> cardsOnBoard = board.GetVisualCardsOfPlayer(this);

            if (cardsOnBoard.Count == 0)
            {
                // Nessuna carta in campo, evoca una carta a caso
                IBoardSlot slot = GetEmptySlotOftype(board, SlotType.Default);
                if(slot != null)
                    PlayRandomCardFromHand(cardsInHand, board, slot);
            }
            else
            {
                // Hai carte in campo, gestisci le azioni
                //HandleActionPhase(cardsOnBoard, board);
            }

            // Notifica la fine del turno
            EventManager.TriggerEvent(EventType.OnEndTurn);
        }
    }

    private void PlayRandomCardFromHand(List<IVisualCard> cardsInHand, IBoard board, IBoardSlot slot)
    {
        foreach (IVisualCard card in cardsInHand)
        {
            if (board.CanPlaceCard(card, slot))
            {
                // Notifichiamo che vogliamo giocare la carta in quello slot
                EventManager.TriggerEvent(EventType.OnTryPlayCard, card, slot);

                // Se hai eseguito un'azione, esci dal metodo PlayTurn
                break;
            }
        }
    }

    private void HandleActionPhase(List<IVisualCard> cardsOnBoard, IBoard board)
    {
        foreach (IVisualCard card in cardsOnBoard)
        {
            if (card.GetCard().ActionPoints > 0)
            {
                // Se ci sono ActionPoints disponibili, gestisci le azioni
                if (NeedToCaptureZone(card, board))
                {
                    MoveToCaptureZone(card, board);
                }
                else if (CanAttackOpponent(card, board))
                {
                    AttackOpponent(card, board);
                }
            }
        }
    }

    private bool NeedToCaptureZone(IVisualCard card, IBoard board)
    {
        List<IBoardSlot> zoneSlots = board.GetSlotsOfType(SlotType.Zone);
        List<IBoardSlot> myZoneSlots = zoneSlots.Where(slot => slot.GetSlotOwner() == PlayerType.Opponent).ToList();

        // Se non ci sono slot nella zona o tutti gli slot sono occupati, non catturare la zona
        if (myZoneSlots.Count == 0 || myZoneSlots.All(slot => slot.GetCardInSlot() != null))
        {
            return false;
        }

        // Almeno uno degli slot nella zona è vuoto, quindi cattura la zona
        return true;
    }

    private IBoardSlot GetEmptySlotOftype(IBoard board, SlotType slotType)
    {
        List<IBoardSlot> slots = board.GetSlotsOfType(slotType);
        List<IBoardSlot> mySlots = slots.Where(slot => slot.GetSlotOwner() == PlayerType.Opponent).ToList();

        // Controlla se almeno uno degli slot è vuoto e restituisci il primo slot vuoto
        foreach (IBoardSlot slot in mySlots)
        {
            if (slot.GetCardInSlot() == null)
            {
                return slot;
            }
        }

        // Nessuno degli slot è vuoto, quindi restituisci null
        return null;
    }

    private void MoveToCaptureZone(IVisualCard card, IBoard board)
    {
        /*if (board.CanPlaceCard(card, slot) && card.GetCard().ActionPoints > 0)
        {
            MoveCard(card, slot);
        }*/
    }

    private bool CanAttackOpponent(IVisualCard card, IBoard board)
    {
        return false;
        // Implementa la logica per verificare se è possibile attaccare l'avversario
    }

    private void AttackOpponent(IVisualCard card, IBoard board)
    {
        // Implementa la logica per attaccare l'avversario
    }


    public void PlayTurn2(IBoard board)
    {
        if (canPlay)
        {
            // Lista delle carte in mano al giocatore
            List<IVisualCard> cardsInHand = hand.GetCardsInHand();

            // Lista delle carte sul terreno di gioco del giocatore
            List<IVisualCard> cardsOnBoard = board.GetVisualCardsOfPlayer(this);

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
