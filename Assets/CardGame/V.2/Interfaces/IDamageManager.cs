using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce il DamageManager, colui che si occupa della gestione di ogni tipo di danno.
/// </summary>
public interface IDamageManager
{
    /// <summary>
    /// Metodo utilizzato per effettuare un attacco con una VisualCard.
    /// </summary>
    /// <param name="attacker">La carta attaccante.</param>
    /// <param name="target">Il bersaglio dell'attacco.</param>
    /// <returns>True se l'attacco ha avuto successo, False altrimenti.</returns>
    bool Attack(IVisualCard attacker, IVisualCard target);

    /// <summary>
    /// Metodo utilizzato per stabilire se una carta puo' attaccare un bersaglio.
    /// </summary>
    /// <param name="attacker">L'attaccante.</param>
    /// <param name="target">Il bersaglio.</param>
    bool CanAttack(IVisualCard attacker, IVisualCard target);

    /// <summary>
    /// Metodo che ritorna una lista di possibili bersagli idonei per l'attacco di una carta.
    /// </summary>
    /// <param name="attacker">La carta attaccante.</param>
    /// <param name="visualCards">Le carte da verificare.</param>
    /// <returns>La lista dei possibili bersagli idonei di una carta attaccante, Null se non ci sono bersagli idonei.</returns>
    List<IVisualCard> GetEligibleTargets(IVisualCard attacker, List<IVisualCard> visualCards);
}