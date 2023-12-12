using UnityEngine;

public class Card : ICard, IDamageable
{
    private CardData cardData;
    private IPlayer owner;
    private CardDirectionType cardDirection;
    private int curhealth;
    private bool isInHand;
    private int curActionPoints;

    public CardData CardData => cardData;

    public int CurHealth => curhealth;

    public IPlayer CardOwner { get => owner; set => owner = value; }

    public CardDirectionType CardDirectionType { get => cardDirection; set => cardDirection = value; }

    public bool IsInHand { get => isInHand; set => isInHand = value; }

    public int ActionPoints { get => curActionPoints; set => curActionPoints = value; }

    public Card(CardData cardData)
    {
        this.cardData = cardData;
        this.curhealth = cardData.health;
        this.curActionPoints = cardData.actionPointsPerTurn;
    }

    public void TakeDamage(int damageAmount)
    {
        curhealth -= damageAmount;
    }
}