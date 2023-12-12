using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AIBehaviour : IAIBehaviour
{
    private IBoard board;
    private IDamageManager damageManager;
    private IPlayer thisPlayer;
    private IPlayer opponent;
    private float AIdelay;

    public AIBehaviour(IBoard board, IDamageManager damageManager, IPlayer thisPlayer, IPlayer opponent, float AIdelay)
    {
        this.board = board;
        this.damageManager = damageManager;
        this.thisPlayer = thisPlayer;
        this.opponent = opponent;
        this.AIdelay = AIdelay;
    }

    public async Task PlayTurn()
    {
        bool finish = false;
        while (thisPlayer.CanPlay && !finish)
        {

            Move bestMove = GetBestMove();

            if (bestMove == null)
            {
                // Non ci sono più mosse valide, passa il turno
                EventManager.TriggerEvent(EventType.OnEndTurn);
                finish = true;
                break;
            }

            await ApplyMove(board, bestMove);
        }
    }


    Move GetBestMove()
    {
        List<Move> possibleMoves = GenerateMoves();
        int bestValue = int.MinValue;
        Move bestMove = null;

        foreach (Move move in possibleMoves)
        {
            int value = EvaluateMove(move, board);
            if (value > bestValue && value != -100)
            {
                bestValue = value;
                bestMove = move;
            }
        }

        return bestMove;
    }

    List<Move> GenerateMoves()
    {
        List<Move> possibleMoves = new List<Move>();

        List<IVisualCard> cardsInHand = thisPlayer.GetCardsInHand();
        List<IVisualCard> cardsOnBoard = board.GetVisualCardsOfPlayer(thisPlayer);

        foreach (IVisualCard card in cardsInHand)
        {
            // Genera possibili mosse di evocazione
            List<IBoardSlot> eligibleSlots = board.GetEligibleSlots(card);
            foreach (IBoardSlot slot in eligibleSlots)
            {
                if (thisPlayer.SteamPoints >= card.GetCard().CardData.steamCost)
                    possibleMoves.Add(new Move(card, null, slot, MoveType.Place));
            }
        }

        foreach (IVisualCard card in cardsOnBoard)
        {
            if (card.GetCard().ActionPoints > 0)
            {
                // Genera mosse di movimento per la carta sul terreno
                List<IBoardSlot> eligibleSlots = board.GetEligibleSlots(card);
                foreach (IBoardSlot slot in eligibleSlots)
                {
                    if (slot.GetSlotType() == SlotType.Zone)
                        possibleMoves.Add(new Move(card, null, slot, MoveType.Move)); // Ci muoviamo solo verso la zona, per ora
                }

                // Genera mosse di attacco contro le carte del giocatore avversario
                List<IVisualCard> opponentCards = board.GetVisualCardsOfPlayer(opponent);
                foreach (IVisualCard opponentCard in opponentCards)
                {
                    if (damageManager.CanAttack(card, opponentCard))
                    {
                        possibleMoves.Add(new Move(card, opponentCard, null, MoveType.Attack));
                    }
                }
            }
        }

        return possibleMoves;
    }

    int EvaluateMove(Move move, IBoard board)
    {
        IVisualCard attackingCard = move.Card;
        IVisualCard targetCard = move.Target;

        switch (move.MoveType)
        {
            case MoveType.Place:
                // Valutazione basata sul punteggio di attacco delle carte evocate
                int damageScore = attackingCard.GetCard().CardData.baseDamage;

                // Ritorna i punteggi di attacco
                return damageScore;

            case MoveType.Move:

                // Valutazione basata sul tipo di spostamento
                // Il movimento lo possiamo fare solo verso la zona, quindi se lo possiamo fare, valutiamo lo stato della zona

                int moveScore = 0;

                ZoneStatus zoneStatus = board.GetZoneStatus();

                // Se la zona è neutrale o se è controllata dal giocatore, allora dobbiamo assolutamente catturarla!
                // Altrimenti avrà priorità bassa, perché significa che la controlliamo noi, oppure che è contesa
                if (zoneStatus == ZoneStatus.Neutral || zoneStatus == ZoneStatus.PlayerControlled)
                    moveScore = 100;

                if (board.FindSlotByCard(move.Card).GetSlotType() == SlotType.Zone)
                    moveScore = -100;

                return moveScore;

            case MoveType.Attack:
                // Valutazione basata sul punteggio di attacco delle carte coinvolte
                int attackScore = attackingCard.GetCard().CardData.baseDamage;
                int targetScore = targetCard.GetCard().CardData.baseDamage;

                // Ritorna la differenza tra i punteggi di attacco
                return attackScore - targetScore;

            default:
                return 0;
        }
    }

    async Task ApplyMove(IBoard board, Move move)
    {
        float moveDelay = GetDelay(move);
        switch (move.MoveType)
        {
            case MoveType.Place:
                EventManager.TriggerEvent<IVisualCard, IBoardSlot>(EventType.OnTryPlayCard, move.Card, move.Slot);
                await Task.Delay(TimeSpan.FromSeconds(moveDelay));
                break;

            case MoveType.Move:
                EventManager.TriggerEvent<IVisualCard, IBoardSlot>(EventType.OnTryMoveCard, move.Card, move.Slot);
                await Task.Delay(TimeSpan.FromSeconds(moveDelay));
                break;

            case MoveType.Attack:
                EventManager.TriggerEvent<IVisualCard, IVisualCard>(EventType.OnTryCardAttack, move.Card, move.Target);
                await Task.Delay(TimeSpan.FromSeconds(moveDelay + 1f));
                break;

            default:
                break;
        }
    }

    float GetDelay(Move move)
    {
        // Se la mossa prevede un attacco, allora il delay sarà uguale alla durata dell'animazione
        if (move.MoveType == MoveType.Attack)
            return move.Card.GetCard().CardData.attackAnimationData.GetAnimLength();

        // Altrimenti il delay sarà quello di default
        return AIdelay;
    }
}
