using UnityEngine;

public class Card : ICard, IDamageable
{
    private CardData cardData;
    private IPlayer owner;
    private CardDirectionType cardDirection;
    private int curhealth;
    private bool isInHand;

    public CardData CardData => cardData;

    public int CurHealth => curhealth;

    public IPlayer CardOwner { get => owner; set => owner = value; }

    public CardDirectionType CardDirectionType { get => cardDirection; set => cardDirection = value; }

    public bool IsInHand { get => isInHand; set => isInHand = value; }

    public Card(CardData cardData)
    {
        this.cardData = cardData;
        this.curhealth = cardData.health;
    }

    public void Play()
    {
        // Implementazione della logica per giocare la carta utilizzando i dati da cardData
    }

    public void Attack(ICard target)
    {
        // Implementazione della logica di attacco utilizzando i dati da cardData
    }

    public void TakeDamage(int damageAmount)
    {
        curhealth -= damageAmount;
    }
}