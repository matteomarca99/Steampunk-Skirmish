using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce la mano di carte di un giocatore.
/// </summary>
public interface IPlayerHand
{
    /// <summary>
    /// Metodo utilizzato per verificare se e' possibile aggiungere una carta alla mano.
    /// </summary>
    /// <returns>True se e' possibile aggiungere una carta alla mano, False altrimenti.</returns>
    bool CanAddCartToHand();

    /// <summary>
    /// Metodo utilizzato per aggiungere una carta alla mano di carte del giocatore.
    /// </summary>
    /// <param name="card">La carta da aggiungere.</param>
    void AddCardToHand(IVisualCard visualCard);

    /// <summary>
    /// Metodo utilizzato per rimuovere una carta dalla mano di carte del giocatore.
    /// </summary>
    /// <param name="card">La carta da rimuovere.</param>
    void RemoveCardFromHand(IVisualCard visualCard);

    /// <summary>
    /// Metodo che restituisce la lista di tutte le carte nella mano del giocatore.
    /// </summary>
    /// <returns>La lista delle carte nella mano del giocatore.</returns>
    List<IVisualCard> GetCardsInHand();
}