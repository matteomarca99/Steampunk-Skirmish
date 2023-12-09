using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    private Queue<QueuedAction> actionQueue = new Queue<QueuedAction>();
    private bool isProcessing;

    public void EnqueueAction(System.Action action, float initialDelay, float finalDelay)
    {
        QueuedAction queuedAction = new QueuedAction(action, initialDelay, finalDelay);
        actionQueue.Enqueue(queuedAction);
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
            QueuedAction nextAction = actionQueue.Dequeue();

            yield return new WaitForSeconds(nextAction.initialDelay);

            nextAction.action?.Invoke();

            yield return new WaitForSeconds(nextAction.finalDelay);
        }

        isProcessing = false;
    }
}