using UnityEngine;

public class Move
{
    public int Score { get; set; }

    public IVisualCard Card { get; set; }
    public IVisualCard Target { get; set; }
    public IBoardSlot Slot { get; set; }
    public MoveType MoveType { get; set; }

    public Move(IVisualCard card, IVisualCard target, IBoardSlot slot, MoveType moveType)
    {
        this.Card = card;
        this.Target = target;
        this.Slot = slot;
        this.MoveType = moveType;
    }
    // Aggiungi altre informazioni necessarie per rappresentare una mossa
    // ad esempio, la carta mossa, lo slot selezionato, ecc.

    public void PrintMove()
    {
        if(Slot == null)
        {
            // Mossa di attacco
            Debug.Log("MOSSA ATTACCO DA " + Card.GetCard().CardData.cardName + " A " + Target.GetCard().CardData.name);
        }
        else
        {
            // Mossa movimento
            Debug.Log("MOSSA MOVIMENTO DA " + Card.GetCard().CardData.cardName + " A " + Slot.ToString());
        }
    }
}