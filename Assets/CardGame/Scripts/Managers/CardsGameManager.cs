using System.Collections.Generic;
using UnityEngine;

public class CardsGameManager : MonoBehaviour
{
    // Singleton pattern
    private static CardsGameManager _instance;
    public static CardsGameManager Instance => _instance;

    public int playerCurrentSteam;

    public PlayerCardsManager playerCardsManager;

    public BoardManager boardManager;

    void Awake()
    {
        // Garantisce che ci sia una sola istanza del GameManager
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            // Se esiste già un'istanza, distruggo questa istanza duplicata
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnPlayerCards();
    }

    public void TryPlayCard(CardDataInstance card, BoardSlot slot)
    {
        // Controlli di costo, validità, ecc...

        if (slot == null || playerCurrentSteam < card.steamCost || slot.isBusy)
        {
            playerCardsManager.RefreshCardsInHandUI();
            return;
        }

        // Se i controlli vengono superati, giochiamo la carta
        PlayCard(card, slot);
    }

    // Esempio di funzione per giocare una carta
    public void PlayCard(CardDataInstance card, BoardSlot slot)
    {
        // Detraggo il costo in steam points
        playerCurrentSteam -= card.steamCost;

        // Occupo lo slot
        slot.isBusy = true;

        // Spawn della carta sullo slot corrispondente
        SpawnCardOnBoard(card.cardData, slot);

        // Rimozione della carta dalla mano
        DestroyPlayerCard(card);

        // Carta giocata con successo!
        Debug.Log("Carta giocata: " + card.cardData.name + " nello slot: " + slot.slotName);
    }

    public void DestroyPlayerCard(CardDataInstance card)
    {
        playerCardsManager.DestroyPlayerCard(card);
    }

    void SpawnPlayerCards()
    {
        playerCardsManager.SpawnCardsInHand();
    }

    void SpawnCardOnBoard(CardData card, BoardSlot slot)
    {
        boardManager.SpawnCard(card, slot);
    }

    public void ShowEligibleSlots(CardData card)
    {
        boardManager.ShowEligibleSlots(card);
    }

    public void DisableAllSlotImages()
    {
        boardManager.DisableAllPanels();
    }
}
