using System.Collections.Generic;
using System.Linq;

public class DamageManager : IDamageManager
{
    public bool Attack(IVisualCard attacker, IVisualCard target)
    {
        if(CanAttack(attacker, target))
        {
            IDamageable card = (IDamageable)target.GetCard();

            card.TakeDamage(attacker.GetCard().CardData.baseDamage);

            if (target.GetCard().CurHealth <= 0)
                target.SetMarkedForDestruction(true);

            return true;
        }

        return false;
    }

    public bool CanAttack(IVisualCard attacker, IVisualCard target)
    {
        if (target.GetCard() is IDamageable &&
            target.GetCard().CurHealth > 0 &&
            attacker.GetCard().CurHealth > 0 &&
            attacker.GetCard().CardOwner != target.GetCard().CardOwner &&
            attacker.GetCard().CardData.cardAttackType == target.GetCard().CardData.cardPlacement &&
            target.GetCard().IsInHand == false &&
            attacker != target)
        {
            return true;
        }

        return false;
    }

    public List<IVisualCard> GetEligibleTargets(IVisualCard attacker, List<IVisualCard> visualCards)
    {
        return visualCards.Where(target => CanAttack(attacker, target)).ToList();
    }
}