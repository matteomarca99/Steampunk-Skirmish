/// <summary>
/// Interfaccia che definisce una carta da gioco in formato visivo.
/// </summary>
public interface IVisualCard
{
    /// <summary>
    /// Metodo utilizzato per impostare i parametri visivi di una carta.
    /// </summary>
    /// <param name="card">La carta da rappresentare visivamente.</param>
    void SetCardData(ICard card);

    /// <summary>
    /// Metodo che ritorna l'effettiva carta che viene rappresentata dalla VisualCard in maniera visiva.
    /// </summary>
    /// <returns>La carta che viene rappresentata dalla VisualCard in maniera visiva. </returns>
    ICard GetCard();

    /// <summary>
    /// Metodo utilizzato per distruggere la VisualCard
    /// </summary>
    void DestroyVisualCard();
}