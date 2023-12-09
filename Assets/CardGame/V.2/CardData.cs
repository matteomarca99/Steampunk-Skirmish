using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    public Sprite sprite;
    public Sprite backSprite;
    public string cardName;
    public string cardDescription;
    public int steamCost;
    public int health;
    public int armor;
    public int baseDamage;
    public PlacementType cardPlacement;
    public PlacementType cardAttackType;

    [Serializable]
    public class CardEffectData
    {
        public CardEffectType cardEffectType;
        public int value;
    }

    [Serializable]
    public class AttackAnimationData
    {
        public float rotationDuration = 0.5f;
        public float backwardMoveDuration = 1f;
        public float forwardMoveDuration = 0.3f;
        public float returnMoveDuration = 0.3f;
        public float returnRotationDuration = 0.5f;

        public float GetAnimLength()
        {
            return rotationDuration + backwardMoveDuration + forwardMoveDuration + returnMoveDuration + returnRotationDuration;
        }
    }

    public List<CardEffectData> cardEffect = new List<CardEffectData>();
    public AttackAnimationData attackAnimationData = new AttackAnimationData();
}