/// <summary>
/// Classe che definisce una QueuedAction.
/// </summary>
public class QueuedAction
{
    public System.Action action;
    public float initialDelay; // Ritardo prima dell'esecuzione dell'azione
    public float finalDelay;   // Ritardo dopo l'esecuzione dell'azione

    public QueuedAction(System.Action action, float initialDelay, float finalDelay)
    {
        this.action = action;
        this.initialDelay = initialDelay;
        this.finalDelay = finalDelay;
    }
}