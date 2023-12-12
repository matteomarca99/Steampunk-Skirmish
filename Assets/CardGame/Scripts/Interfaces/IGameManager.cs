/// <summary>
/// Interfaccia che definisce il GameManager.
/// </summary>
public interface IGameManager
{
    /// <summary>
    /// Metodo utlilizzato per far iniziare una partita.
    /// </summary>
    void StartGame();

    /// <summary>
    /// Metodo utilizzato per mettere in pausa la partita.
    /// </summary>
    void PauseGame();

    /// <summary>
    /// Metodo utilizzato per concludere la partita.
    /// </summary>
    void EndGame();
}