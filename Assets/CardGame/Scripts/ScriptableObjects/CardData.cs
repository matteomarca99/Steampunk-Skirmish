using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    public Sprite sprite;
    public string cardName;
    public string cardDescription;
    public int steamCost;
    public int health;
    public int armor;
    public int baseDamage;
    public CardPlacement cardPlacement;
    public CardAttackType cardAttackType;

    [Serializable]
    public class CardEffectData
    {
        public CardEffectType cardEffectType;
        public int value;
    }

    public List<CardEffectData> cardEffect = new List<CardEffectData>();
}