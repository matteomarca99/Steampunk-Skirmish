using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce il terreno di gioco.
/// </summary>
public interface IBoard
{
    /// <summary>
    /// Metodo utilizzato per verificare se una carta puo' essere giocata in un determinato slot.
    /// </summary>
    /// <param name="card">La carta da giocare</param>
    /// <param name="slot">Lo slot dove si vuole giocare la carta.</param>
    /// <returns></returns>
    bool CanPlaceCard(ICard card, IBoardSlot slot);

    /// <summary>
    /// Metodo utilizzato per posizionare una carta nello slot corrispondente.
    /// </summary>
    /// <param name="card">La carta da giocare.</param>
    /// <param name="slot">Lo slot dove si vuole giocare la carta.</param>
    void PlaceCard(ICard card, IBoardSlot slot);

    /// <summary>
    /// Metodo utilizzato per selezionare una carta dallo slot corrispondente.
    /// </summary>
    /// <param name="slot">Lo slot che contiene la carta.</param>
    /// <returns>La carta contenuta nello slot se presente, Null altrimenti.</returns>
    ICard SelectCard(IBoardSlot slot);

    /// <summary>
    /// Metodo utilizzato per ottenere la lista degli slot presenti all'interno del terreno di gioco.
    /// </summary>
    /// <returns>La lista degli slot presenti all'interno del terreno di gioco.</returns>
    List<IBoardSlot> GetBoardSlots();

    /// <summary>
    /// Metodo che restituisce gli slot idonei al posizionamento di una carta.
    /// </summary>
    /// <param name="card">La carta di riferimento.</param>
    /// <returns>Gli slot idonei al posizionamento di una carta.</returns>
    List<IBoardSlot> GetEligibleSlots(ICard card);
}