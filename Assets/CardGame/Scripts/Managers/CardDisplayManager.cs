using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplayManager : MonoBehaviour
{
    public CardDataInstance cardDataInstance;
    public Image image;
    public TextMeshProUGUI steamCost;
    public TextMeshProUGUI health;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI armor;

    public bool isInHand;

    public void RefreshCardInfo()
    {
        image.sprite = cardDataInstance.sprite;

        if (isInHand)
        {
            steamCost.text = cardDataInstance.steamCost.ToString();
        } else
        {
            health.text = cardDataInstance.health.ToString();
            damage.text = cardDataInstance.damage.ToString();
            armor.text = cardDataInstance.armor.ToString();

            if (cardDataInstance.armor < 1)
                armor.transform.parent.gameObject.SetActive(false);
        }
    }
}
