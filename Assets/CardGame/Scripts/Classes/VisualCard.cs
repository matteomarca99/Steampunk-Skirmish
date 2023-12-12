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
    public TextMeshProUGUI actionPoints;

    private ICard card;

    private Draggable draggable;
    private CardTargeting cardTargeting;
    private CardHoverAnimation cardHoverAnimation;

    private bool markedForDestruction = false;
    private bool isAnimating = false;

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
            actionPoints.text = card.ActionPoints.ToString();

            if (card.ActionPoints <= 0)
            {
                actionPoints.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                actionPoints.transform.parent.gameObject.SetActive(true);
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

    public bool IsAnimating
    {
        get => isAnimating;
        set
        {
            isAnimating = value;
            SetBehaviour();
        }
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

    void SetTextObjectsActive(bool steamCostActive, bool healthActive, bool damageActive, bool actionPointsActive)
    {
        steamCost.transform.parent.gameObject.SetActive(steamCostActive);
        health.transform.parent.gameObject.SetActive(healthActive);
        damage.transform.parent.gameObject.SetActive(damageActive);
        actionPoints.transform.parent.gameObject.SetActive(actionPointsActive);
    }

    void SetBehaviour()
    {
        if (card.CardOwner.GetPlayerType() == PlayerType.Player && !IsMarkedForDestruction())
        {
            bool canPlay = card.CardOwner.CanPlay;

            draggable.enabled = canPlay && card.IsInHand && card.CardOwner.SteamPoints >= card.CardData.steamCost;
            cardTargeting.enabled = canPlay && !card.IsInHand && card.ActionPoints > 0 && !IsAnimating;
            cardHoverAnimation.enabled = card.IsInHand && card.CardOwner.GetPlayerType() == PlayerType.Player;
        } else if (!IsMarkedForDestruction())
        {
            draggable.enabled = false;
            cardTargeting.enabled = false;
            cardHoverAnimation.enabled = false;
        }
    }

    public void ChangeBorderColor(Color borderColor)
    {
        borderImage.color = borderColor;
    }
}