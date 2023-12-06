using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce un deck di carte.
/// </summary>
public interface IDeck
{
    /// <summary>
    /// Metodo utilizzato per mischiare il deck di carte.
    /// </summary>
    void Shuffle();

    /// <summary>
    /// Metodo utilizzato per verificare se e' possibile pescare una carta dal deck.
    /// </summary>
    /// <returns>True se e' possibile pescare una carta dal deck, False altrimenti.</returns>
    bool CanDrawCard();

    /// <summary>
    /// Metodo utilizzato per pescare una carta dal deck. Se la carta e' valida allora viene rimossa dal deck.
    /// </summary>
    /// <returns>La carta pescata, Null altrimenti.</returns>
    ICard DrawCard();

    /// <summary>
    /// Metodo che restituisce la lista delle carte presenti nel deck.
    /// </summary>
    /// <returns>La lista delle carte presenti nel deck.</returns>
    List<ICard> GetCards();

    /// <summary>
    /// Metodo utilizzato per impostare il IPlayer proprietario delle carte del deck.
    /// </summary>
    /// <param name="owner">IPlayer proprietario delle carte del deck.</param>
    void SetCardsOwner(IPlayer owner);
}