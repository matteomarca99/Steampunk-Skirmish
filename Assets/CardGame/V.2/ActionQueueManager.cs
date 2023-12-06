using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    private Queue<System.Action> actionQueue = new Queue<System.Action>();

    private bool isProcessing;

    public void EnqueueAction(System.Action action)
    {
        actionQueue.Enqueue(action);
        if (!isProcessing)
        {
            StartCoroutine(ProcessActionQueue());
        }
    }

    private IEnumerator ProcessActionQueue()
    {
        isProcessing = true;

        while (actionQueue.Count > 0)
        {
            System.Action nextAction = actionQueue.Dequeue();
            yield return new WaitForSeconds(1f); // Tempo di attesa tra un'azione e l'altra

            // Esegui l'azione
            nextAction?.Invoke();
        }

        isProcessing = false;
    }
}
