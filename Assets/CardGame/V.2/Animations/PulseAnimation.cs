using UnityEngine;
using DG.Tweening;

public class PulseAnimation : MonoBehaviour
{
    [SerializeField] private float pulseScale = 0.2f; // La scala aggiunta durante la pulsazione
    [SerializeField] private float duration = 0.5f; // Durata della pulsazione

    private Vector3 originalScale; // Scala originale dell'oggetto

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        StartPulseAnimation();
    }

    private void StartPulseAnimation()
    {
        float targetScaleX = originalScale.x + pulseScale;
        float targetScaleY = originalScale.y + pulseScale;
        float targetScaleZ = originalScale.z + pulseScale;

        Vector3 targetScale = new Vector3(targetScaleX, targetScaleY, targetScaleZ);

        transform.DOScale(targetScale, duration / 2).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        transform.DOKill(); // Assicura che l'animazione venga interrotta quando l'oggetto viene distrutto
        transform.localScale = originalScale; // E resetta la posizione iniziale
    }
}
