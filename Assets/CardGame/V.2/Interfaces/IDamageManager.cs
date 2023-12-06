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
    void Attack(IVisualCard attacker, IVisualCard target);
}