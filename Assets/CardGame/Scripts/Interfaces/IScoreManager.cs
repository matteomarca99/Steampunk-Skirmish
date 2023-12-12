/// <summary>
/// Interfaccia che definisce due punteggi.
/// </summary>
public interface IScoreManager
{
    /// <summary>
    /// Metodo che definisce il punteggio del giocatore.
    /// </summary>
    public int PlayerScore { get; set; }

    /// <summary>
    /// Metodo che definisce il punteggio dell'avversario.
    /// </summary>
    public int OpponentScore { get; set; }

    /// <summary>
    /// Metodo che definisce l'ultimo punteggio aggiunto al giocatore.
    /// </summary>
    public int LastPlayerScoreAdded { get; set; }

    /// <summary>
    /// Metodo che definisce l'ultimo punteggio aggiunto all'avversario.
    /// </summary>
    public int LastOpponentScoreAdded { get; set; }
}
