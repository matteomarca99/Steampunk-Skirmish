using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interfaccia che definisce un giocatore controllato dall'IA.
/// </summary>
public interface IAIBehaviour
{
    /// <summary>
    /// Metodo utilizzato dalla IA per giocare il turno attuale.
    /// </summary>
    Task PlayTurn();
}