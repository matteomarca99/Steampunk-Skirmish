using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualCard : MonoBehaviour, IVisualCard
{
    public Image image;
    public TextMeshProUGUI steamCost;
    public TextMeshProUGUI health;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI armor;

    private ICard card;

    public void SetCardData(ICard card)
    {
        this.card = card;

        // Se la carta e' coperta, allora impostiamo solo l'immagine corrispondente
        if(card.CardDirectionType == CardDirectionType.FaceDown)
        {
            // Impostiamo la carta coperta
            image.sprite = card.CardData.backSprite;

            return;
        }

        image.sprite = card.CardData.sprite;

        if (card.IsInHand)
        {
            steamCost.text = card.CardData.steamCost.ToString();
        }
        else
        {
            health.text = card.CurHealth.ToString();
            damage.text = card.CardData.baseDamage.ToString();
            armor.text = card.CardData.armor.ToString();

            if (card.CardData.armor < 1)
                armor.transform.parent.gameObject.SetActive(false);
        }
    }

    public ICard GetCard()
    {
        return card;
    }

    public void DestroyVisualCard()
    {
        // Eventuali Feedback di distruzione //

        Debug.Log("DISTRUGGO!");

        // Se la carta non è in mano, significa che è in uno slot, quindi la rimuoviamo dallo slot
        if(!card.IsInHand)
            GetComponentInParent<IBoardSlot>().RemoveCard();

        // E infine distruggiamo la VisualCard associata alla carta (cioè questo oggetto)
        Destroy(gameObject);
    }
}