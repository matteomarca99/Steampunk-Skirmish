/// <summary>
/// Interfaccia che definisce un oggetto che puo' ricevere danni.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Metodo utilizzato per ricevere danni.
    /// </summary>
    /// <param name="damageAmount">Il numero di danni da ricevere.</param>
    void TakeDamage(int damageAmount);
}
