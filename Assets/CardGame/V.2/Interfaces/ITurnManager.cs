using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce un TurnManager, cioe' il componente che gestisce i turni del gioco.
/// </summary>
public interface ITurnManager
{
    /// <summary>
    /// Metodo utilizzato per iniziare un turno di gioco.
    /// </summary>
    void StartTurn();

    /// <summary>
    /// Metodo utilizzato per concludere un turno di gioco.
    /// </summary>
    void EndTurn();

    /// <summary>
    /// Metodo che restituisce il numero del turno attuale.
    /// </summary>
    /// <returns>Il numero del turno attuale.</returns>
    int GetCurrentTurn();

    /// <summary>
    /// Metodo che restituisce il giocatore del turno attuale.
    /// </summary>
    /// <returns>Il giocatore del turno attuale.</returns>
    IPlayer GetCurrentTurnPlayer();
}
