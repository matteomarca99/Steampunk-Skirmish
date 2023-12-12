using System.Collections.Generic;
using UnityEngine;

public class DefaultGameManager : MonoBehaviour, IGameManager
{
    private ActionQueueManager actionQueueManager;

    public GameObject cardPrefab;

    public List<CardData> playerCardsData;
    public List<CardData> opponentCardsData;

    public GameUI gameUI;
    public Board board;
    public ScoreManager scoreManager;
    private TurnManager turnManager;
    private DamageManager damageManager;
    private AIBehaviour AIBehaviour;

    public int initialCardsNumber;
    public float aiDelay;

    public int initialSteamPoints;
    public int pointsGeneratedByZone;
    public int pointsToWin;

    private Player player;
    private Player opponent;

    private Deck playerDeck;
    private Deck opponentDeck;

    private List<IPlayer> players = new();

    void Awake()
    {
        actionQueueManager = GetComponent<ActionQueueManager>();
        EventManager.SubscribeToEvent<IPlayer>(EventType.OnDrawCard, OnDrawcard);
        EventManager.SubscribeToEvent<IVisualCard>(EventType.OnCardDrag, OnCardDrag);
        EventManager.SubscribeToEvent<IVisualCard, IBoardSlot>(EventType.OnTryPlayCard, OnTryPlayCard);
        EventManager.SubscribeToEvent<IVisualCard, IBoardSlot>(EventType.OnTryMoveCard, OnTryMoveCard);
        EventManager.SubscribeToEvent(EventType.OnStartTurn, OnStartTurn);
        EventManager.SubscribeToEvent(EventType.OnEndTurn, OnEndTurn);
        EventManager.SubscribeToEvent<IVisualCard, IVisualCard>(EventType.OnTryCardAttack, OnTryCardAttack);
        EventManager.SubscribeToEvent<IVisualCard>(EventType.OnBeginTargeting, OnBeginTargeting);
        EventManager.SubscribeToEvent<IVisualCard>(EventType.OnEndTargeting, OnEndTargeting);
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
        playerDeck = new Deck(playerCardsData, cardPrefab);

        // Inizializzazione del deck dell'IA (il deck si occupera' di creare le carte che corrispondono alle CardData)
        opponentDeck = new Deck(opponentCardsData, cardPrefab);

        // Inizializzazione del giocatore (che inizializza in automatico anche la PlayerHand associata)
        player = new Player("Matteo", PlayerType.Player, playerDeck, initialSteamPoints);
        players.Add(player);

        // Inizializzazione dell'avversario (che inizializza in automatico anche la PlayerHand associata)
        opponent = new Player("IA", PlayerType.Opponent, opponentDeck, initialSteamPoints);
        players.Add(opponent);

        // I giocatori pescano le carte iniziali
        player.DrawCardsFromDeck(initialCardsNumber);
        opponent.DrawCardsFromDeck(initialCardsNumber);

        // Le carte sono state inizializzate, aggiorniamo la UI
        gameUI.RefreshHand(players);

        // Inizializzazione del TurnManager
        turnManager = new TurnManager(players);

        // Inizializzazione del DamageManager
        damageManager = new DamageManager();

        // Inizializzazione dello ScoreManager
        scoreManager = new ScoreManager();

        // Inizializzazione dell'AIBehaviour
        AIBehaviour = new AIBehaviour(board, damageManager, opponent, player, aiDelay);

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
            if (player.CanDrawCardFromDeck())
            {
                Debug.Log("Pesco...");
                player.DrawCardFromDeck();
                gameUI.DoDrawAnim(player);
            }
        }, 0f, 1f);
    }

    void OnCardDrag(IVisualCard visualCard)
    {
        gameUI.ShowEligibleslots(visualCard);
    }

    void OnStartTurn()
    {
        IPlayer curTurnPlayer = turnManager.GetCurrentTurnPlayer();

        // E assegnamo gli steamPoints del turno
        OnAssignSteamPoints(curTurnPlayer);

        // Inizio del turno, peschiamo una carta
        OnDrawcard(curTurnPlayer);

        // Ripristiniamo gli ActionPoints delle carte del giocatore di turno
        OnResetActionPoints(curTurnPlayer);

        curTurnPlayer.CanPlay = true;

        gameUI.StartTurn(curTurnPlayer);

        if (curTurnPlayer.GetPlayerType() == PlayerType.Opponent)
        {
            _ = AIBehaviour.PlayTurn();
        }
    }

    public void OnEndTurn()
    {
        IPlayer curTurnPlayer = turnManager.GetCurrentTurnPlayer();

        float delay = GetPlayerDelay(curTurnPlayer);

        actionQueueManager.EnqueueAction(() =>
        {
            curTurnPlayer.CanPlay = false;
            OnDisableActionPoints(curTurnPlayer);
            OnAssignZonePoints(pointsGeneratedByZone);
            turnManager.EndTurn();
            gameUI.EndTurn(scoreManager);

            OnCheckWinCondition();

        }, delay, 0f);
    }

    void OnTryPlayCard(IVisualCard card, IBoardSlot slot)
    {
        IPlayer curTurnPlayer = turnManager.GetCurrentTurnPlayer();
        float delay = GetPlayerDelay(curTurnPlayer);

        actionQueueManager.EnqueueAction(() =>
        {
            if (board.CanPlaceCard(card, slot) && curTurnPlayer.SteamPoints >= card.GetCard().CardData.steamCost)
            {
                PlayCard(curTurnPlayer, card, slot);
            }
            else
            {
                UpdateUI(curTurnPlayer);
            }
        }, delay, 0f);
    }

    void PlayCard(IPlayer curTurnPlayer, IVisualCard card, IBoardSlot slot)
    {
        curTurnPlayer.RemoveCardFromHand(card);
        board.PlaceCard(card, slot);
        card.GetCard().CardDirectionType = CardDirectionType.FaceUp;
        card.GetCard().ActionPoints = 0;
        card.GetCard().CardOwner.SteamPoints -= card.GetCard().CardData.steamCost;

        UpdateUI(curTurnPlayer);
    }

    void OnTryMoveCard(IVisualCard card, IBoardSlot slot)
    {
        IPlayer curTurnPlayer = turnManager.GetCurrentTurnPlayer();

        float delay = GetPlayerDelay(curTurnPlayer);

        actionQueueManager.EnqueueAction(() =>
        {
            // Per prima cosa verifichiamo se la carta puo' essere posisizonata sullo slot corrispondente
            if (board.CanPlaceCard(card, slot) && card.GetCard().ActionPoints > 0)
            {
                MoveCard(card, slot);
            }
            // Infine aggiorniamo la UI
            gameUI.RefreshBoard();
        }, delay, 0f);
    }

    void MoveCard(IVisualCard card, IBoardSlot slot)
    {
        // Rimuoviamo la carta dallo slot attuale
        board.FindSlotByCard(card).RemoveCard();

        // E la posizioniamo nello slot corrispondente
        board.PlaceCard(card, slot);

        // Infine, consumiamo un actionPoint
        card.GetCard().ActionPoints--;
    }

    void OnTryCardAttack(IVisualCard attacker, IVisualCard target)
    {
        // Aggiungi l'animazione dell'attacco in coda
        if(damageManager.CanAttack(attacker, target) && attacker.GetCard().ActionPoints > 0)
        {
            float animLength = attacker.GetCard().CardData.attackAnimationData.GetAnimLength();
            actionQueueManager.EnqueueAction(() =>
            {
                if (damageManager.CanAttack(attacker, target) && attacker.GetCard().ActionPoints > 0)
                {
                    CardAttack(attacker, target);
                } else
                {
                    animLength = 0;
                }
            }, 0f, animLength);
        }
    }

    void CardAttack(IVisualCard attacker, IVisualCard target)
    {
        damageManager.Attack(attacker, target);
        gameUI.DoAttackAnim(attacker, target);

        // Infine, consumiamo un actionPoint
        attacker.GetCard().ActionPoints--;
    }

    void UpdateUI(IPlayer curTurnPlayer)
    {
        gameUI.RefreshHand(curTurnPlayer);
        gameUI.RefreshBoard();
    }

    void OnCardDestroyed(IVisualCard visualCard)
    {
        Debug.Log(visualCard.GetCard().CardData.name + " è stata distrutta!");

        Destroy(visualCard.GetTransform().gameObject);
    }

    void OnBeginTargeting(IVisualCard attacker)
    {
        if (attacker.GetCard().ActionPoints > 0)
        {
            List<IVisualCard> eligibleTargets = damageManager.GetEligibleTargets(attacker, board.GetVisualCards());

            gameUI.BeginTargeting(attacker, attacker.GetTransform(), eligibleTargets);
            gameUI.ShowEligibleslots(attacker);
        }
    }

    void OnEndTargeting(IVisualCard attacker)
    {
        List<IVisualCard> eligibleTargets = damageManager.GetEligibleTargets(attacker, board.GetVisualCards());

        gameUI.EndTargeting(eligibleTargets);
    }

    void OnResetActionPoints(IPlayer player)
    {
        List<IVisualCard> playerCardsOnBoard = board.GetVisualCardsOfPlayer(player);

        playerCardsOnBoard.ForEach(visualCard => visualCard.GetCard().ActionPoints = visualCard.GetCard().CardData.actionPointsPerTurn);

        Debug.Log("Resetto AP di " + player.GetPlayerName());
    }

    void OnDisableActionPoints(IPlayer player)
    {
        List<IVisualCard> playerCardsOnBoard = board.GetVisualCardsOfPlayer(player);

        playerCardsOnBoard.ForEach(visualCard => visualCard.GetCard().ActionPoints = 0);

        Debug.Log("Disabilito AP di " + player.GetPlayerName());
    }

    void OnAssignZonePoints(int points)
    {
        if (points > 0)
        {
            ZoneStatus zoneStatus = board.GetZoneStatus();

            if (zoneStatus == ZoneStatus.PlayerControlled)
            {
                Debug.Log("ZONA CONTROLLATA DA " + zoneStatus.ToString() + " ASSEGNO " + points + " PUNTI");
                scoreManager.PlayerScore += points;
                scoreManager.LastPlayerScoreAdded += points;
            }
            else if (zoneStatus == ZoneStatus.OpponentControlled)
            {
                Debug.Log("ZONA CONTROLLATA DA " + zoneStatus.ToString() + " ASSEGNO " + points + " PUNTI");
                scoreManager.OpponentScore += points;
                scoreManager.LastOpponentScoreAdded += points;
            }
        }
    }

    void OnAssignSteamPoints(IPlayer player1)
    {
        int pointToAssign = initialSteamPoints + turnManager.GetCurrentRound();
        player1.SteamPoints = pointToAssign;

        Debug.Log("SP PLAYER: " + player.SteamPoints);
        Debug.Log("SP OPPONENT: " + opponent.SteamPoints);
    }

    void OnCheckWinCondition()
    {
        if(scoreManager.PlayerScore >= pointsToWin)
        {
            Debug.Log("HAI VINTO!!!");
        }
        else if(scoreManager.OpponentScore >= pointsToWin)
        {
            Debug.Log("HAI PERSO!!!");
        }
    }

    public void OnEndTurnBtn()
    {
        OnEndTurn();
    }

    float GetPlayerDelay(IPlayer player)
    {
        return player.GetPlayerType() == PlayerType.Opponent ? aiDelay : 0f;
    }

    void PrintDebug()
    {
        Debug.Log("Giocatore attuale: " + turnManager.GetCurrentTurnPlayer().GetPlayerName());
        Debug.Log("Turno attuale: " + turnManager.GetCurrentTurn());
        Debug.Log("Numero di carte nel deck del giocatore: " + playerDeck.GetCards().Count);
        Debug.Log("Numero di carte nel deck dell'avversario: " + opponentDeck.GetCards().Count);
    }
}