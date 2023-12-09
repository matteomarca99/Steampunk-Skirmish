using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce il terreno di gioco.
/// </summary>
public interface IBoard
{
    /// <summary>
    /// Metodo utilizzato per verificare se una carta puo' essere giocata in un determinato slot.
    /// </summary>
    /// <param name="visualCard">La carta da giocare</param>
    /// <param name="slot">Lo slot dove si vuole giocare la carta.</param>
    /// <returns></returns>
    bool CanPlaceCard(IVisualCard visualCard, IBoardSlot slot);

    /// <summary>
    /// Metodo utilizzato per posizionare una carta nello slot corrispondente.
    /// </summary>
    /// <param name="visualCard">La carta da giocare.</param>
    /// <param name="slot">Lo slot dove si vuole giocare la carta.</param>
    void PlaceCard(IVisualCard visualCard, IBoardSlot slot);

    /// <summary>
    /// Metodo utilizzato per selezionare una carta dallo slot corrispondente.
    /// </summary>
    /// <param name="slot">Lo slot che contiene la carta.</param>
    /// <returns>La carta contenuta nello slot se presente, Null altrimenti.</returns>
    IVisualCard SelectCard(IBoardSlot slot);

    /// <summary>
    /// Metodo utilizzato per ottenere la lista degli slot presenti sul terreno di gioco.
    /// </summary>
    /// <returns>La lista degli slot presenti all'interno del terreno di gioco.</returns>
    List<IBoardSlot> GetBoardSlots();

    /// <summary>
    /// Metodo utilizzato per ottenere la lista delle carte attualmente presenti sul terreno di gioco.
    /// </summary>
    /// <returns>La lista di carte presenti all'interno del terreno di gioco.</returns>
    List<IVisualCard> GetVisualCards();

    /// <summary>
    /// Metodo che restituisce gli slot idonei al posizionamento di una carta.
    /// </summary>
    /// <param name="visualCard">La carta di riferimento.</param>
    /// <returns>Gli slot idonei al posizionamento di una carta.</returns>
    List<IBoardSlot> GetEligibleSlots(IVisualCard visualCard);
}