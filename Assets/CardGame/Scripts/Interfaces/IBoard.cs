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
    /// Metodo utilizzato per restituire lo slot dove e' posizionata una VisualCard.
    /// </summary>
    /// <param name="visualCard">La VisualCard da verificare.</param>
    /// <returns>Lo slot se trovato, Null altrimenti.</returns>
    IBoardSlot FindSlotByCard(IVisualCard visualCard);

    /// <summary>
    /// Metodo utilizzato per ottenere la lista degli slot presenti sul terreno di gioco.
    /// </summary>
    /// <returns>La lista degli slot presenti all'interno del terreno di gioco.</returns>
    List<IBoardSlot> GetBoardSlots();

    /// <summary>
    /// Metodo utilizzato per ottenere la lista degli slot di un certo tipo, presenti sul terreno di gioco.
    /// </summary>
    /// <param name="slotType">Il tipo degli slot.</param>
    /// <returns>La lista degli slot di un certo tipo, presenti sul terreno di gioco.</returns>
    List<IBoardSlot> GetSlotsOfType(SlotType slotType);

    /// <summary>
    /// Metodo utilizzato per ottenere la lista delle carte attualmente presenti sul terreno di gioco.
    /// </summary>
    /// <returns>La lista di carte presenti all'interno del terreno di gioco.</returns>
    List<IVisualCard> GetVisualCards();

    /// <summary>
    /// Metodo utilizzato per ottenere le carte di un certo giocatore attualmente presenti sul terreno di gioco.
    /// </summary>
    /// <param name="owner">Il proprietario delle carte.</param>
    /// <returns>La lista di carte di un certo giocatore attualmente presenti sul terreno di gioco.</returns>
    List<IVisualCard> GetVisualCardsOfPlayer(IPlayer owner);

    /// <summary>
    /// Metodo che restituisce gli slot idonei al posizionamento di una carta.
    /// </summary>
    /// <param name="visualCard">La carta di riferimento.</param>
    /// <returns>Gli slot idonei al posizionamento di una carta.</returns>
    List<IBoardSlot> GetEligibleSlots(IVisualCard visualCard);

    /// <summary>
    /// Metodo che ritorna lo stato della zona.
    /// </summary>
    /// <returns>Lo stato della zona.</returns>
    ZoneStatus GetZoneStatus();
}