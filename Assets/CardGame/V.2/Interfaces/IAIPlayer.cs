using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce un giocatore controllato dall'IA.
/// </summary>
public interface IAIPlayer
{
    /// <summary>
    /// Metodo utilizzato dalla IA per giocare il turno attuale.
    /// </summary>
    /// <param name="board">Il terreno di gioco che contiene gli slot.</param>
    void PlayTurn(IBoard board);
}