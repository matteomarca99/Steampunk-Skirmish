using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DefaultGameManager : MonoBehaviour, IGameManager
{
    private ActionQueueManager actionQueueManager;

    public List<CardData> playerCardsData;
    public List<CardData> opponentCardsData;

    public GameUI gameUI;
    public Board board;
    public int initialCardsNumber;

    private TurnManager turnManager;
    private DamageManager damageManager;

    private Player player;
    private AIPlayer opponent;

    private Deck playerDeck;
    private Deck opponentDeck;

    private List<IPlayer> players = new();

    void Awake()
    {
        actionQueueManager = GetComponent<ActionQueueManager>();
        EventManager.SubscribeToEvent<IPlayer>(EventType.OnDrawCard, OnDrawcard);
        EventManager.SubscribeToEvent<ICard>(EventType.OnCardDrag, OnCardDrag);
        EventManager.SubscribeToEvent<ICard, IBoardSlot>(EventType.OnTryPlayCard, OnTryPlayCard);
        EventManager.SubscribeToEvent<IPlayer>(EventType.OnPlayerCanPlay, OnPlayerCanPlay);
        EventManager.SubscribeToEvent(EventType.OnEndTurn, OnEndTurn);
        EventManager.SubscribeToEvent<IVisualCard, IVisualCard>(EventType.OnTryCardAttack, OnTryCardAttack);
        EventManager.SubscribeToEvent<Transform>(EventType.OnBeginTargeting, OnBeginTargeting);
        EventManager.SubscribeToEvent(EventType.OnEndTargeting, OnEndTargeting);
        EventManager.SubscribeToEvent<IVisualCard>(EventType.OnCardDestroyed, OnCardDestroyed);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // IMPLEMENTAZIONE PER INIZIARE LA PARTITA

        // Inizializzazione del deck del giocatore (il deck si occupera' di creare le carte che corrispondono alle CardData)
        playerDeck = new Deck(playerCardsData);

        // Inizializzazione del deck dell'IA (il deck si occupera' di creare le carte che corrispondono alle CardData)
        opponentDeck = new Deck(opponentCardsData);

        // Inizializzazione del giocatore (che inizializza in automatico anche la PlayerHand associata)
        player = new Player("Matteo", PlayerType.Player, playerDeck);
        players.Add(player);

        // Inizializzazione dell'avversario (che inizializza in automatico anche la PlayerHand associata)
        opponent = new AIPlayer("IA", PlayerType.IA, opponentDeck);
        players.Add(opponent);

        // I giocatori pescano le carte iniziali
        player.DrawCardsFromDeck(initialCardsNumber);
        opponent.DrawCardsFromDeck(initialCardsNumber);

        // Le carte sono state inizializzate, aggiorniamo la UI
        gameUI.RefreshHand(players);

        // Inizializzazione del DamageManager
        damageManager = new DamageManager();

        // Inizializzazione del TurnManager
        turnManager = new TurnManager(players);

        // Comunichiamo al TurnManager di iniziare il turno.
        turnManager.StartTurn();

        PrintDebug();
    }

    public void PauseGame()
    {
        // Implementazione per mettere in pausa la partita
    }

    public void EndGame()
    {
        // Implementazione per concludere la partita
    }

    void OnDrawcard(IPlayer player)
    {
        actionQueueManager.EnqueueAction(() =>
        {
            player.DrawCardFromDeck();
            gameUI.RefreshHand(player);
        });
    }

    void OnCardDrag(ICard card)
    {
        gameUI.ShowEligibleslots(board, card);
    }

    void OnPlayerCanPlay(IPlayer curTurnPlayer)
    {
        if (curTurnPlayer is IAIPlayer aiPlayer)
        {
            aiPlayer.PlayTurn(board);

            if(turnManager.GetCurrentTurn() > 1)
                gameUI.TogglePlayerControls(player);
        } else
        {
            gameUI.TogglePlayerControls(player);
        }
    }


    void OnTryPlayCard(ICard card, IBoardSlot slot)
    {
        actionQueueManager.EnqueueAction(() =>
        {
            IPlayer curTurnPlayer = turnManager.GetCurrentTurnPlayer();

            // Per prima cosa verifichiamo se la carta puo' essere posisizonata sullo slot corrispondente
            if (board.CanPlaceCard(card, slot))
            {
                // Procediamo con la rimozione della carta dalla mano del giocatore
                curTurnPlayer.RemoveCardFromHand(card);

                // E posizioniamo la carta sul terreno di gioco, nello slot corrispondente
                board.PlaceCard(card, slot);

                // Inoltre ovviamente impostiamo la carta in posizione scoperta
                card.CardDirectionType = CardDirectionType.FaceUp;
            }
            // Infine aggiorniamo la UI
            gameUI.RefreshHand(curTurnPlayer);
            gameUI.RefreshBoard(board);
        });
    }

    void OnTryCardAttack(IVisualCard attacker, IVisualCard target)
    {
        if (attacker.GetCard().CurHealth > 0 && target.GetCard().CurHealth > 0)
        {
            damageManager.Attack(attacker, target);
            actionQueueManager.EnqueueAction(() =>
            {
                // Animazione Attacco //

                if (attacker.GetCard().CurHealth > 0)
                    gameUI.RefreshBoard(board);
            });
        }
    }

    void OnCardDestroyed(IVisualCard visualCard)
    {
        Debug.Log(visualCard.GetCard().CardData.name + " è stata distrutta!");

        actionQueueManager.EnqueueAction(() =>
        {
            visualCard.DestroyVisualCard();

            gameUI.RefreshBoard(board);
            gameUI.RefreshHand(players);
        });
    }

    void OnBeginTargeting(Transform attacker)
    {
        gameUI.BeginTargeting(attacker);
    }

    void OnEndTargeting()
    {
        gameUI.EndTargeting();
    }

    public void OnEndTurn()
    {
        actionQueueManager.EnqueueAction(() =>
        {
            turnManager.EndTurn();
        });
    }

    public void RefreshPlayerHandTEST()
    {
        gameUI.RefreshHand(opponent);
    }

    void PrintDebug()
    {
        Debug.Log("Giocatore attuale: " + turnManager.GetCurrentTurnPlayer().GetPlayerName());
        Debug.Log("Turno attuale: " + turnManager.GetCurrentTurn());
        Debug.Log("Numero di carte nel deck del giocatore: " + playerDeck.GetCards().Count);
        Debug.Log("Numero di carte nel deck dell'avversario: " + opponentDeck.GetCards().Count);
    }
}