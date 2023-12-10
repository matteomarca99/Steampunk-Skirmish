/// <summary>
/// Enum che identifica il tipo di evento.
/// </summary>
public enum EventType
{
    OnDrawCard,
    OnCardDrag,
    OnTryPlayCard,
    OnTryMoveCard,
    OnStartTurn,
    OnEndTurn,
    OnTryCardAttack,
    OnBeginTargeting,
    OnEndTargeting,
    OnCardDestroyed
    // Altri tipi di evento
}