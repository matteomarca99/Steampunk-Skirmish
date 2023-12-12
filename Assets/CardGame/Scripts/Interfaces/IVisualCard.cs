using UnityEngine;

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
    /// Metodo utilizzato per effettuare il refresh visivo della VisualCard. Utilizza la Card all'interno della VisualCard.
    /// </summary>
    /// <param name="holder">Il Transform del contenitore attuale della VisualCard.</param>
    void RefreshVisualCard(Transform holder = null);

    /// <summary>
    /// Metodo utilizzato per cambiare il colore del bordo della carta.
    /// </summary>
    /// <param name="borderColor">Il colore del boardo da impostare.</param>
    void ChangeBorderColor(Color borderColor);

    /// <summary>
    /// Metodo utilizzato per marchiare la carta da distruggere al prossimo refresh.
    /// </summary>
    /// <param name="value">Il valore se marchiare la carta come da distruggere al prossimo refresh oppure no.</param>
    void SetMarkedForDestruction(bool value);

    /// <summary>
    /// Metodo utilizzato per ottenere il marchio di distruzione della carta.
    /// </summary>
    /// <returns>Il marchio di distruzione della carta.</returns>
    bool IsMarkedForDestruction();

    /// <summary>
    /// Metodo che definisce se una carta sta eseguendo un'animazione oppure no.
    /// </summary>
    bool IsAnimating { get; set; }

    /// <summary>
    /// Metodo utilizzato per distruggere la VisualCard
    /// </summary>
    void DestroyVisualCard();

    /// <summary>
    /// Metodo che ritorna il Transform della carta nella scena.
    /// </summary>
    /// <returns>Il Transform della carta nella scena.</returns>
    Transform GetTransform();
}