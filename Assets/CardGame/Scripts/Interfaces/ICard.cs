using UnityEngine;

/// <summary>
/// Interfaccia che definisce una carta (card).
/// </summary>
public interface ICard
{
    /// <summary>
    /// Restituisce lo ScriptableObject CardData associato alla carta, dove sono contenute tutte le informazioni.
    /// </summary>
    CardData CardData { get; }

    /// <summary>
    /// Restituisce gli attuali punti vita della carta.
    /// </summary>
    int CurHealth { get; }

    /// <summary>
    /// Metodo che definisce il proprietario della carta.
    /// </summary>
    IPlayer CardOwner { get; set; }

    /// <summary>
    /// L'attuale direzione della carta.
    /// </summary>
    CardDirectionType CardDirectionType { get; set; }

    /// <summary>
    /// Metodo che definisce se la carta e' in mano al giocatore oppure no.
    /// </summary>
    bool IsInHand { get; set; }

    /// <summary>
    /// Metodo che definisce i punti azione disponibili per la carta.
    /// </summary>
    int ActionPoints { get; set; }
}
