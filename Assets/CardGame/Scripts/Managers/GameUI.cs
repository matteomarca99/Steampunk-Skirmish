using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;
using Shapes2D;
using System;

public class GameUI : MonoBehaviour, IGameUI
{
    public ColorData uiColors;
    public Canvas canvas;
    public Board board;
    public Transform playerCardsHolder;
    public Transform opponentCardsHolder;
    public Transform playerCardSpawn;
    public Transform opponentCardSpawn;

    public GameObject floatingTextPrefab;

    public Button endTurnBtn;

    public Shape zonePanel;
    public TextMeshProUGUI zonetext;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI opponentScoreText;
    public TextMeshProUGUI playerSteamPointsText;
    public TextMeshProUGUI opponentSteamPointsText;

    public GameObject endSplash;
    public Text endSplashText;

    public Arrow arrow;

    public void RefreshHand(IPlayer player)
    {
        Debug.Log("Refresh hand del giocatore " + player.GetPlayerName());

        List<IVisualCard> playerCards = player.GetCardsInHand();

        Transform cardsHolder = GetCardTransform(player.GetPlayerType(), true);

        // Aggiorna la UI delle carte in mano
        playerCards.ForEach(visualCard =>
        {
            // Se non è il player che tiene la carta in mano, allora verra' coperta
            if (player.GetPlayerType() != PlayerType.Player)
            {
                visualCard.GetCard().CardDirectionType = CardDirectionType.FaceDown;
            }
            else
            {
                visualCard.GetCard().CardDirectionType = CardDirectionType.FaceUp;
            }

            // Infine effettuiamo il refresh della VisualCard
            visualCard.RefreshVisualCard(cardsHolder);

            // Il refresh del contenitore delle carte
            LayoutRebuilder.ForceRebuildLayoutImmediate(cardsHolder.GetComponent<RectTransform>());

            // E il refresh degli SteamPoints
            RefreshPlayerSteamPoints(player);
        });

        // Infine effettuiamo anche il refresh della curvatura delle carte
        cardsHolder.GetComponent<RadialLayoutManager>().Rotate();
    }

    public void RefreshHand(List<IPlayer> players)
    {
        players.ForEach(player => RefreshHand(player));
    }

    public void RefreshBoard()
    {
        Debug.Log("Refresh della board");

        List<BoardSlot> boardSlots = board.GetBoardSlots().Cast<BoardSlot>().ToList();

        // Aggiorna la UI delle carte negli slot
        boardSlots.ForEach(slot =>
        {
            if (slot.GetCardInSlot() != null && slot.GetCardInSlot().IsAnimating == false && !slot.GetCardInSlot().IsMarkedForDestruction())
            {
                IVisualCard visualCardInSlot = slot.GetCardInSlot();
                visualCardInSlot.RefreshVisualCard(slot.transform);
            }

            // In caso ci fosse lo slot evidenziato (eligibleSlot), disattiviamo l'immagine
            slot.GetComponent<Image>().enabled = false;
        });

        // Alla fine Effettuiamo anche il refresh della zona
        RefreshZone();
    }

    public void ShowEligibleslots(IVisualCard visualCard)
    {
        List<BoardSlot> boardSlots = board.GetEligibleSlots(visualCard).Cast<BoardSlot>().ToList();

        // Mostriamo gli slot idonei
        boardSlots.ForEach(slot =>
        {
            Image image = slot.GetComponent<Image>();
            image.enabled = true;
            image.color = uiColors.eligibleSlotColor;
        });
    }

    public void StartTurn(IPlayer player)
    {
        if (player.GetPlayerType() == PlayerType.Player)
        {
            endTurnBtn.interactable = true;
        } else
        {
            endTurnBtn.interactable = false;
        }

        RefreshBoard();
        RefreshPlayerSteamPoints(player);
    }

    public void EndTurn(IScoreManager scoreManager)
    {
        RefreshBoard();
        RefreshScore(scoreManager);
    }

    public void EndGame(IPlayer winner)
    {
        endSplash.SetActive(true);
        
        if(winner.GetPlayerType() == PlayerType.Player)
        {
            endSplashText.text = "HAI VINTO!";
        } else
        {
            endSplashText.text = "HAI PERSO!";
        }
    }

    void RefreshZone()
    {
        ZoneStatus zoneStatus = board.GetZoneStatus();

        switch (zoneStatus)
        {
            case ZoneStatus.PlayerControlled:
                zonetext.text = "ZONA CONTROLLATA";
                zonePanel.settings.fillColor = uiColors.playerZonePanelColor;
                zonetext.color = uiColors.playerZoneTextColor;
                break;

            case ZoneStatus.OpponentControlled:
                zonetext.text = "ZONA CONTROLLATA DALL'AVVERSARIO";
                zonePanel.settings.fillColor = uiColors.opponentZonePanelColor;
                zonetext.color = uiColors.opponentZoneTextColor;
                break;

            case ZoneStatus.Contested:
                zonetext.text = "ZONA CONTESA";
                zonePanel.settings.fillColor = uiColors.contestedZonePanelColor;
                zonetext.color = uiColors.contestedZoneTextColor;
                break;

            case ZoneStatus.Neutral:
                zonetext.text = "ZONA NEUTRALE";
                zonePanel.settings.fillColor = uiColors.neutralZonePanelColor;
                zonetext.color = uiColors.neutralZoneTextColor;
                break;

            default:
                zonetext.text = "STATO SCONOSCIUTO";
                zonePanel.settings.fillColor = Color.black;
                zonetext.color = Color.white;
                break;
        }
    }

    void RefreshScore(IScoreManager scoreManager)
    {
        playerScoreText.text = "PUNTI: " + scoreManager.PlayerScore;
        opponentScoreText.text = "PUNTI: " + scoreManager.OpponentScore;

        if (scoreManager.LastPlayerScoreAdded > 0)
        {
            StartCoroutine(ShowFloatingText(playerScoreText.transform.position + Vector3.up, "+" + scoreManager.LastPlayerScoreAdded.ToString(), uiColors.playerScoreAddColor));
            scoreManager.LastPlayerScoreAdded = 0;
        }
        if (scoreManager.LastOpponentScoreAdded > 0)
        {
            StartCoroutine(ShowFloatingText(opponentScoreText.transform.position - Vector3.up, "+" + scoreManager.LastOpponentScoreAdded.ToString(), uiColors.opponentScoreAddColor));
            scoreManager.LastOpponentScoreAdded = 0;
        }
    }

    void RefreshPlayerSteamPoints(IPlayer player)
    {
        if (player.GetPlayerType() == PlayerType.Player)
        {
            playerSteamPointsText.text = "STEAM: " + player.SteamPoints;
        }
        else
        {
            opponentSteamPointsText.text = "STEAM: " + player.SteamPoints;
        }
    }

    public void DoAttackAnim(IVisualCard attacker, IVisualCard target)
    {
        StartCoroutine(AttackAnimation(attacker, target));
    }

    private IEnumerator AttackAnimation(IVisualCard attacker, IVisualCard target)
    {
        attacker.IsAnimating = true;

        Transform attackerTransform = attacker.GetTransform();
        Transform targetTransform = target.GetTransform();
        Vector3 startPos = attackerTransform.position;

        Vector3 distance = target.GetTransform().position - startPos;
        distance = Vector3.MoveTowards(distance, distance * 0.001f, 1f);

        Vector3 cross = Vector3.Cross(attackerTransform.up, distance);
        float angle = Vector3.SignedAngle(attackerTransform.up, distance, cross);

        if (Vector3.Dot(cross, attackerTransform.forward) < 0)
        {
            angle = -angle;
        }

        // Prendiamo il danno da mostrare
        int damage = attacker.GetCard().CardData.baseDamage;

        // Prendiamo i delay
        float rotationDuration = attacker.GetCard().CardData.attackAnimationData.rotationDuration;  // Delay rotazione verso il target
        float backwardMoveDuration = attacker.GetCard().CardData.attackAnimationData.backwardMoveDuration; // Delay movimento all'indietro
        float forwardMoveDuration = attacker.GetCard().CardData.attackAnimationData.forwardMoveDuration; // Delay movimento verso il target
        float returnMoveDuration = attacker.GetCard().CardData.attackAnimationData.returnMoveDuration; // Delay movimento verso la posizione iniziale
        float returnRotationDuration = attacker.GetCard().CardData.attackAnimationData.returnRotationDuration; // Delay rotazione iniziale

        // Animazione di attacco

        // 1) Ruotiamo la carta verso il target
        yield return attackerTransform.DORotate(new Vector3(0, 0, angle), rotationDuration).WaitForCompletion();

        // 2) Muoviamo la carta indietro (come per prendere la rincorsa)
        Vector3 backwardPosition = attackerTransform.position - attackerTransform.up * 0.8f;
        yield return attackerTransform.DOMove(backwardPosition, backwardMoveDuration).WaitForCompletion();

        // 3) Muoviamo la carta verso il target
        yield return attackerTransform.DOMove(startPos + distance, forwardMoveDuration).WaitForCompletion();

        // Crea e mostra il numero di danno come UI temporanea
        Vector3 damageTextPosition = targetTransform.position;
        StartCoroutine(ShowFloatingText(damageTextPosition, "-" + damage.ToString(), Color.red));

        // Attacco effettuato, aggiorniamo la UI delle carte
        attacker.RefreshVisualCard();
        target.RefreshVisualCard();

        // Aggiorniamo anche la UI della zona
        RefreshZone();

        // 4) Muoviamo la carta verso la sua posizione iniziale
        yield return attackerTransform.DOMove(startPos, returnMoveDuration).WaitForCompletion();

        // 5) Ruotiamo la carta nella sua rotazione originaria
        yield return attackerTransform.DORotate(Vector3.zero, returnRotationDuration).WaitForCompletion();

        if(attacker != null)
            attacker.IsAnimating = false;
    }

    public void DoDrawAnim(IPlayer player)
    {
        StartCoroutine(DrawAnimation(player));
    }

    private IEnumerator DrawAnimation(IPlayer player)
    {
        List<IVisualCard> cardsInHand = player.GetCardsInHand();

        // Prendiamo l'ultima carta pescata
        IVisualCard lastDrawnCard = cardsInHand[cardsInHand.Count - 1];

        // Prendiamo l'holder delle carte del giocatore
        Transform cardsHolder = GetCardTransform(player.GetPlayerType(), true);

        // Prendiamo lo spawn delle carte del giocatore (la posizione del deck)
        Transform spawnDeckTransform = GetCardTransform(player.GetPlayerType(), false);

        // Ora dobbiamo effettuare l'animazione dell'ultima carta pescata

        // 1) Mettiamo la carta sopra al deck
        lastDrawnCard.RefreshVisualCard(spawnDeckTransform);

        // 2) Ruotiamo la carta verso l'holder
        Vector3 distance = cardsHolder.transform.position - lastDrawnCard.GetTransform().position;
        distance = Vector3.MoveTowards(distance, distance * 0.001f, 1f);
        lastDrawnCard.GetTransform().up = distance;

        // 2) Muoviamo la carta verso la mano del giocatore
        Vector3 offset = player.GetPlayerType() == PlayerType.Player ? new Vector3(-1, -1, 0) : new Vector3(1, 1, 0);
        yield return lastDrawnCard.GetTransform().DOMove(cardsHolder.position + offset, 1f).WaitForCompletion();

        // 3) Fine animazione, aggiorniamo la mano del giocatore così la carta viene aggiunta correttamente al relativo holder
        RefreshHand(player);
    }

    private IEnumerator ShowFloatingText(Vector3 position, string text, Color textColor)
    {
        // Crea un'etichetta temporanea per mostrare il danno inflitto
        GameObject damageTextObject = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        damageTextObject.transform.SetParent(canvas.transform);
        damageTextObject.transform.localScale = Vector3.one;

        // Impostiamo il valore del danno
        TextMeshProUGUI damageText = damageTextObject.GetComponent<TextMeshProUGUI>();
        damageText.text = text;
        damageText.color = textColor;

        // E lo muoviamo verso l'alto
        yield return damageTextObject.transform.DOMove(damageTextObject.transform.position + Vector3.up, 1f).WaitForCompletion();

        // Aggiungi un'animazione se necessario (es. fading out)
        Destroy(damageTextObject);
    }

    public void BeginTargeting(IVisualCard attacker, Transform position, List<IVisualCard> eligibleTargets)
    {
        arrow.SetupAndActivate(position, eligibleTargets);

        // Evidenziamo i possibili targets
        eligibleTargets.ForEach(target =>
        {
            target.ChangeBorderColor(uiColors.eligibleTargetBorderColor);
        });
    }

    public void EndTargeting(List<IVisualCard> eligibleTargets)
    {
        arrow.Deactivate();

        // Disattiviamo l'evidenziazione dei possibili target
        eligibleTargets.ForEach(target =>
        {
            target.ChangeBorderColor(uiColors.defaultTargetBorderColor);
        });

        RefreshBoard();
    }

    Transform GetCardTransform(PlayerType playerType, bool useHolder)
    {
        if (playerType == PlayerType.Player)
        {
            return useHolder ? playerCardsHolder : playerCardSpawn;
        }
        else
        {
            return useHolder ? opponentCardsHolder : opponentCardSpawn;
        }
    }
}