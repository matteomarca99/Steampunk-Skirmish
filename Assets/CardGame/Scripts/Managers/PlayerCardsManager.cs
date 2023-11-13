using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsManager : MonoBehaviour
{
    public List<CardData> playerCardsInHand = new();
    public List<CardDataInstance> playerCardsInHandInstances = new();

    public GameObject cardPrefab;

    public void SpawnCardsInHand()
    {
        // Prima rimuoviamo le carte di test
        DeleteAllCardsInHand();

        // Poi istanziamo ogni carta come figlio dell'oggetto
        foreach (CardData card in playerCardsInHand)
        {
            GameObject newCardObject = Instantiate(cardPrefab, transform);
            newCardObject.GetComponent<CardDataInstance>().Initialize(card);
            newCardObject.GetComponent<CardDisplayManager>().RefreshCardInfo();

            playerCardsInHandInstances.Add(newCardObject.GetComponent<CardDataInstance>());
        }

        RefreshCardsInHandUI();
    }

    public void DeleteAllCardsInHand()
    {
        // Svuoto la lista delle istanze delle carte
        playerCardsInHandInstances.Clear();

        // Rimuovo tutti i figli dell'oggetto
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshCardsInHandUI()
    {
        // Disattivando e riattivando l'HorizontalLayoutGroup si resetta la posizione delle carte

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void DestroyPlayerCard(CardDataInstance card)
    {
        // Rimuovo la carta dalla lista delle carte del giocatore
        playerCardsInHandInstances.Remove(card);

        // Distruggo l'oggetto
        Destroy(card.gameObject);

        // Aggiorno la visualizzazione delle carte in mano
        RefreshCardsInHandUI();
    }
}
