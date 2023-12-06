using UnityEngine;
using System.Collections.Generic;

public class TestGameManager : MonoBehaviour
{
    public List<CardData> cardsData;

    private IGameUI gameUI;
    private IPlayer player;
    private IDeck deck;

    public Board board;

    void Start()
    {
        //gameUI.OnCardSelected += HandleCardSelected;
        //gameUI.OnBoardCellClicked += HandleBoardCellClicked;

        PrintBoardSlots(board.GetBoardSlots());

        //player = new Player();

        //deck = new Deck(cardsData);

        //deck.Shuffle();

        /*player.AddCardToHand(deck.DrawCard());
        player.AddCardToHand(deck.DrawCard());
        player.AddCardToHand(deck.DrawCard());
        player.AddCardToHand(deck.DrawCard());*/

        //PrintCardsInHand(player);

        //PrintCardsInDeck(deck);


    }

    // Metodi che gestiscono le interazioni dell'utente e aggiornano la logica di gioco
    private void HandleCardSelected(ICard card)
    {
        // Logica per selezionare una carta e giocarla sul terreno
        // ...

        // Aggiornare l'UI dopo l'azione del giocatore
        //gameUI.RefreshHand(player.GetCardsInHand());
        //gameUI.RefreshBoard(board.GetBoardState());
    }

    private void HandleBoardCellClicked(int x, int y)
    {
        // Logica per gestire il click su una cella del terreno di gioco
        // ...

        // Aggiornare l'UI dopo l'azione del giocatore
        //gameUI.RefreshBoard(board.GetBoardState());
    }

    private void PrintBoardSlots(List<IBoardSlot> slots)
    {
        // Ottenere gli slots
        foreach (BoardSlot slot in slots)
        {
            Debug.Log("SLOT => Nome slot: " + slot.gameObject.name);
        }
    }

    private void PrintCardsInHand(IPlayer player)
    {
        // Ottenere le carte in mano del giocatore
        List<ICard> cardsInHand = player.GetCardsInHand();
        foreach (ICard card in cardsInHand)
        {
            Debug.Log("MANO => Nome carta: " + card.CardData.name + "   Costo: " + card.CardData.steamCost);
        }
    }

    private void PrintCardsInDeck(IDeck deck)
    {
        // Ottenere le carte nel deck del giocatore
        List<ICard> cardsInDeck = deck.GetCards();
        foreach (ICard card in cardsInDeck)
        {
            Debug.Log("DECK => Nome carta: " + card.CardData.name + "   Costo: " + card.CardData.steamCost);
        }
    }
}