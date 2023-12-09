/// <summary>
/// Interfaccia che definisce uno slot che serve per posizionare le carte sul terreno di gioco.
/// </summary>
public interface IBoardSlot
{
    /// <summary>
    /// Metodo utilizzato per verificare se una carta puo' essere giocata nello slot.
    /// </summary>
    /// <param name="visualCard">La carta da giocare.</param>
    /// <returns>True se la carta puo' essere posizionata nello slot, False altrimenti.</returns>
    bool CanDropCard(IVisualCard visualCard);

    /// <summary>
    /// Metodo utilizzato per posizionare una carta nello slot.
    /// </summary>
    /// <param name="visualCard">La carta da posizionare nello slot.</param>
    void DropCard(IVisualCard visualCard);

    /// <summary>
    /// Metodo utilizzato per rimuovere la carta dallo slot (se presente).
    /// </summary>
    void RemoveCard();

    /// <summary>
    /// Metodo che ritorna la carta presente nello slot.
    /// </summary>
    /// <returns>La carta presente nello slot, Null se lo slot e' libero.</returns>
    IVisualCard GetCardInSlot();

    /// <summary>
    /// Metodo che ritorna il tipo di proprietario dello slot.
    /// </summary>
    /// <returns>Il tipo di proprietario dello slot.</returns>
    PlayerType GetSlotOwner();

    /// <summary>
    /// Metodo che ritorna il tipo di piazzamento dello slot.
    /// </summary>
    /// <returns>Il tipo di piazzamento dello slot</returns>
    PlacementType GetPlacementType();
}
