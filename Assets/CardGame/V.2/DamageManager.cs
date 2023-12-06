using UnityEngine;

public class DamageManager : IDamageManager
{
    public void Attack(IVisualCard attacker, IVisualCard target)
    {
        if(target.GetCard() is IDamageable)
        {
            IDamageable card = (IDamageable)target.GetCard();
            
            card.TakeDamage(attacker.GetCard().CardData.baseDamage);

            // Se la carta non ha piu' vita, notifichiamo la sua distruzione
            if (target.GetCard().CurHealth <= 0)
            {
                Debug.Log("OK!!!");
                EventManager.TriggerEvent<IVisualCard>(EventType.OnCardDestroyed, target);
            }
        }
    }
}