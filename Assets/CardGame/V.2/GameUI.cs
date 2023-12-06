using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameUI : MonoBehaviour, IGameUI
{
    public Transform playerCardsHolder;
    public Transform opponentCardsHolder;


    public GameObject playerHandCardPrefab;
    public GameObject opponentHandCardPrefab;
    public GameObject boardCardPrefab;

    public Color eligibleSlotColor;

    public Arrow arrow;

    public void RefreshHand(IPlayer player)
    {
        Debug.Log("Refresh hand del giocatore " + player.GetPlayerName());

        List<ICard> playerCards = player.GetCardsInHand();

        Transform cardsHolder = GetCardHolderTransform(player.GetPlayerType());

        // Elimina tutti i figli dell'handCardPrefab
        foreach (Transform child in cardsHolder)
        {
            Destroy(child.gameObject);
        }

        // Crea nuove carte per ciascuna carta del giocatore
        playerCards.ForEach(card =>
        {
            // Se non è il player che tiene la carta in mano, allora verra' coperta
            if (player.GetPlayerType() != PlayerType.Player)
            {
                card.CardDirectionType = CardDirectionType.FaceDown;
            }
            else
            {
                card.CardDirectionType = CardDirectionType.FaceUp;
            }
                
            GameObject newCardObject = Instantiate(GetCardPrefab(player.GetPlayerType()), cardsHolder);
            newCardObject.GetComponent<IVisualCard>().SetCardData(card);
        });

        // Infine effettuiamo anche il refresh della curvatura delle carte
        //cardsHolder.GetComponent<RadialLayoutManager>().Rotate();
    }

    public void RefreshHand(List<IPlayer> players)
    {
        players.ForEach(player => RefreshHand(player));
    }

    public void RefreshBoard(IBoard board)
    {
        Debug.Log("Refresh della board");

        List<BoardSlot> boardSlots = board.GetBoardSlots().Cast<BoardSlot>().ToList();

        // Aggiorna la UI degli slot
        boardSlots.ForEach(slot =>
        {
            if (slot.transform.childCount > 0)
                Destroy(slot.transform.GetChild(0).gameObject);

            if (slot.GetCardInSlot() != null)
            {
                GameObject newCardObject = Instantiate(boardCardPrefab, slot.transform);
                newCardObject.GetComponent<IVisualCard>().SetCardData(slot.GetCardInSlot());
            }

            // In caso ci fosse lo slot evidenziato (eligibleSlot), disattiviamo l'immagine
            slot.GetComponent<Image>().enabled = false;
        });
    }

    public void ShowEligibleslots(IBoard board, ICard card)
    {
        List<BoardSlot> boardSlots = board.GetEligibleSlots(card).Cast<BoardSlot>().ToList();

        // Mostriamo gli slot idonei
        boardSlots.ForEach(slot =>
        {
            Image image = slot.GetComponent<Image>();
            image.enabled = true;
            image.color = eligibleSlotColor;
        });
    }

    public void TogglePlayerControls(IPlayer player)
    {
        /*if(player.GetPlayerType() == PlayerType.Player)
            playerCardsHolder.gameObject.SetActive(!playerCardsHolder.gameObject.activeSelf);*/

        if (player.GetPlayerType() == PlayerType.Player)
        {
            if (playerCardsHolder.gameObject.activeInHierarchy == true)
                playerCardsHolder.gameObject.SetActive(false);
            else
                playerCardsHolder.gameObject.SetActive(true);
        }
    }

    public void BeginTargeting(Transform attacker)
    {
        arrow.SetupAndActivate(attacker);
    }

    public void EndTargeting()
    {
        arrow.Deactivate();
    }

    Transform GetCardHolderTransform(PlayerType playerType)
    {
        if (playerType == PlayerType.Player)
            return playerCardsHolder;
        else
            return opponentCardsHolder;
    }

    GameObject GetCardPrefab(PlayerType playerType)
    {
        if (playerType == PlayerType.Player)
            return playerHandCardPrefab;
        else
            return opponentHandCardPrefab;
    }
}