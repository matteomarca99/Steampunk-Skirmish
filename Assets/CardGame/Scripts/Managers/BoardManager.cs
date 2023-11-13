using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{

    public GameObject boardCardPrefab;
    public List<Image> meleeRowPanels = new();
    public List<Image> rangedRowPanels = new();
    public List<Image> buildingRowPanels = new();

    public Color eligibleSlotColor;

    public void SpawnCard(CardData card, BoardSlot slot)
    {
        // Istanziare una nuova carta come figlio dello slot
        GameObject newCardObject = Instantiate(boardCardPrefab, slot.transform);
        newCardObject.GetComponent<CardDataInstance>().Initialize(card);
        newCardObject.GetComponent<CardDisplayManager>().RefreshCardInfo();
    }

    public void DestroyCard(BoardSlot slot)
    {
        // Distruggi la carta associata a questo slot se presente
        GameObject card = transform.GetChild(0).gameObject;

        if (card != null)
            Destroy(card);
    }

    public void ShowEligibleSlots(CardData card)
    {
        CardPlacement placement = card.cardPlacement;

        switch (placement)
        {
            case CardPlacement.MeleeAttackRow:
                ActivatePanels(meleeRowPanels);
                break;
            case CardPlacement.RangedAttackRow:
                ActivatePanels(rangedRowPanels);
                break;
            case CardPlacement.HybridAttackRow:
                ActivatePanels(meleeRowPanels);
                ActivatePanels(rangedRowPanels);
                break;
            case CardPlacement.BuildingRow:
                ActivatePanels(buildingRowPanels);
                break;
        }
    }

    private void ActivatePanels(List<Image> panels)
    {
        foreach (Image panelImage in panels)
        {
            panelImage.enabled = true;
            panelImage.color = eligibleSlotColor;
        }
    }

    public void DisableAllPanels()
    {
        DisablePanelsInList(meleeRowPanels);
        DisablePanelsInList(rangedRowPanels);
        DisablePanelsInList(buildingRowPanels);
    }

    private void DisablePanelsInList(List<Image> panels)
    {
        foreach (Image panelImage in panels)
        {
            panelImage.enabled = false;
        }
    }

}
