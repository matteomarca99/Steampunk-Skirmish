using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataInstance : MonoBehaviour
{
    public CardData cardData;
    public Sprite sprite;
    public int steamCost;
    public int health;
    public int armor;
    public int damage;

    public void Initialize(CardData cardData)
    {
        this.cardData = cardData;
        this.sprite = cardData.sprite;
        this.steamCost = cardData.steamCost;
        this.health = cardData.health;
        this.armor = cardData.armor;
        this.damage = cardData.baseDamage;
    }
}
