using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class RadialLayoutManager : MonoBehaviour
{
    public float startRotation;

    public void Rotate()
    {
        StartCoroutine(AdjustRotationAfterFrame());
    }

    private IEnumerator AdjustRotationAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        int cardCount = transform.childCount;

        float totalRotation = startRotation * 2; // Somma tra la rotazione positiva e quella negativa

        for (int i = 0; i < cardCount; i++)
        {
            RectTransform card = transform.GetChild(i) as RectTransform;
            if (card != null)
            {
                float normalizedIndex = i / (float)(cardCount - 1); // Indice normalizzato tra 0 e 1
                float rotation = Mathf.Lerp(startRotation, -startRotation, normalizedIndex);
                card.localRotation = Quaternion.Euler(0f, 0f, rotation);
            }
        }
    }
}
