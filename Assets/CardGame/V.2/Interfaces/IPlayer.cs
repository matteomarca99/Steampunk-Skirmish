using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce un giocatore.
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Metodo per aggiungere una carta nella mano del giocatore.
    /// </summary>
    /// <param name="card">La carta da aggiungere.</param>
    void AddCardToHand(ICard card);

    /// <summary>
    /// Metodo per rimuovere una carta dalla mano del giocatore.
    /// </summary>
    /// <param name="card">La carta da rimuovere.</param>
    void RemoveCardFromHand(ICard card);

    /// <summary>
    /// Metodo che restituisce la lista di tutte le carte nella mano del giocatore.
    /// </summary>
    /// <returns>La lista delle carte nella mano del giocatore.</returns>
    List<ICard> GetCardsInHand();

    /// <summary>
    /// Metodo utilizzato per pescare una carta dal deck del giocatore ed aggiungerla alla mano.
    /// </summary>
    void DrawCardFromDeck();

    /// <summary>
    /// Metodo utilizzato per pescare N carte dal deck del giocatore ed aggiungerla alla mano.
    /// </summary>
    void DrawCardsFromDeck(int n);

    /// <summary>
    /// Metodo che restituisce la lista di tutte le carte nel deck del giocatore.
    /// </summary>
    /// <returns>La lista delle carte nel deck del giocatore.</returns>
    List<ICard> GetCardsInDeck();

    /// <summary>
    /// Metodo che restituisce il nome del giocatore.
    /// </summary>
    /// <returns>Il nome del giocatore.</returns>
    string GetPlayerName();

    /// <summary>
    /// Metodo che restituisce il tipo di giocatore.
    /// </summary>
    /// <returns>Il tipo di giocatore.</returns>
    PlayerType GetPlayerType();
}
