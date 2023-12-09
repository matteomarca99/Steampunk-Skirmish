using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualCard : MonoBehaviour, IVisualCard
{
    public Image cardImage;
    public Image borderImage;
    public TextMeshProUGUI steamCost;
    public TextMeshProUGUI health;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI armor;

    private ICard card;

    private Draggable draggable;
    private CardTargeting cardTargeting;
    private CardHoverAnimation cardHoverAnimation;

    private bool markedForDestruction = false;

    void Awake()
    {
        draggable = GetComponent<Draggable>();
        cardTargeting = GetComponent<CardTargeting>();
        cardHoverAnimation = GetComponent<CardHoverAnimation>();
    }

    public void SetCardData(ICard card)
    {
        this.card = card;

        SetBehaviour();

        // Se la carta è coperta, impostiamo solo l'immagine corrispondente
        if (card.CardDirectionType == CardDirectionType.FaceDown)
        {
            // Impostiamo la carta coperta
            cardImage.sprite = card.CardData.backSprite;
            SetTextObjectsActive(false, false, false, false); // Disabilita tutti i testi
            return;
        }

        cardImage.sprite = card.CardData.sprite;

        if (card.IsInHand)
        {
            SetTextObjectsActive(true, false, false, false); // Se la carta è in mano, abilitiamo solo il costo in steam
            steamCost.text = card.CardData.steamCost.ToString();
        }
        else
        {
            SetTextObjectsActive(false, true, true, true); // Se non è in mano abilitiamo tutto tranne il costo in steam

            health.text = card.CurHealth.ToString();
            damage.text = card.CardData.baseDamage.ToString();
            armor.text = card.CardData.armor.ToString();

            if (card.CardData.armor < 1)
            {
                armor.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                armor.transform.parent.gameObject.SetActive(true);
            }
        }
    }

    public ICard GetCard()
    {
        return card;
    }

    public void RefreshVisualCard(Transform holder = null)
    {
        if (card != null)
            SetCardData(card);

        if (holder != null)
        {
            transform.SetParent(holder);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            GetComponent<RectTransform>().localPosition = Vector3.zero;
        }

        // Se è marchiata da distruggere, notifichiamo la sua distruzione
        if (IsMarkedForDestruction())
            DestroyVisualCard();
    }

    public void SetMarkedForDestruction(bool value)
    {
        markedForDestruction = value;
    }

    public bool IsMarkedForDestruction()
    {
        return markedForDestruction;
    }

    public void DestroyVisualCard()
    {
        // Eventuali Feedback di distruzione //

        // Se la carta non è in mano, significa che è in uno slot, quindi la rimuoviamo dallo slot
        if(!card.IsInHand)
            GetComponentInParent<IBoardSlot>().RemoveCard();

        // Notifichiamo la distruzione della carta
        EventManager.TriggerEvent<IVisualCard>(EventType.OnCardDestroyed, this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    void SetTextObjectsActive(bool steamCostActive, bool healthActive, bool damageActive, bool armorActive)
    {
        steamCost.transform.parent.gameObject.SetActive(steamCostActive);
        health.transform.parent.gameObject.SetActive(healthActive);
        damage.transform.parent.gameObject.SetActive(damageActive);
        armor.transform.parent.gameObject.SetActive(armorActive);
    }

    void SetBehaviour()
    {
        bool canPlay = card.CardOwner.CanPlay;

        draggable.enabled = canPlay && card.IsInHand;
        cardTargeting.enabled = canPlay && !card.IsInHand;
        cardHoverAnimation.enabled = card.IsInHand && card.CardOwner.GetPlayerType() == PlayerType.Player;
    }

    public void ChangeBorderColor(Color borderColor)
    {
        borderImage.color = borderColor;
    }
}