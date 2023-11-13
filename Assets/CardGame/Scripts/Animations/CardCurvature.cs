using UnityEngine;

public class CardCurvature : MonoBehaviour
{
    public float curvatureAngle = 45;

    private void OnEnable()
    {
        ArrangeCards();
    }

    void ArrangeCards()
    {
        int cardCount = transform.childCount;

        if (cardCount < 2)
            return;

        for (int i = 0; i < cardCount; i++)
        {
            Transform card = transform.GetChild(i);
            float t = i / (float)(cardCount - 1); // Percentuale tra le carte
            float angle = Mathf.Lerp(-curvatureAngle, curvatureAngle, t); // Angolo per la curvatura
            card.localRotation = Quaternion.Euler(0f, 0f, -angle); // Ruota la carta in base all'angolo
        }
    }
}
