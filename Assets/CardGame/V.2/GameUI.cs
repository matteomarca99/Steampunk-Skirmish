using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

public class GameUI : MonoBehaviour, IGameUI
{
    public Canvas canvas;
    public Board board;
    public Transform playerCardsHolder;
    public Transform opponentCardsHolder;
    public Transform playerCardSpawn;
    public Transform opponentCardSpawn;

    public GameObject damageTextPrefab;

    public Color eligibleSlotColor;
    public Color defaultTargetBorderColor;
    public Color eligibleTargetBorderColor;

    public Button endTurnBtn;

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

            // E del contenitore delle carte
            LayoutRebuilder.ForceRebuildLayoutImmediate(cardsHolder.GetComponent<RectTransform>());
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

            if (slot.GetCardInSlot() != null)
            {
                IVisualCard visualCardInSlot = slot.GetCardInSlot();
                visualCardInSlot.RefreshVisualCard(slot.transform);
            }

            // In caso ci fosse lo slot evidenziato (eligibleSlot), disattiviamo l'immagine
            slot.GetComponent<Image>().enabled = false;
        });
    }

    public void ShowEligibleslots(IVisualCard visualCard)
    {
        List<BoardSlot> boardSlots = board.GetEligibleSlots(visualCard).Cast<BoardSlot>().ToList();

        // Mostriamo gli slot idonei
        boardSlots.ForEach(slot =>
        {
            Image image = slot.GetComponent<Image>();
            image.enabled = true;
            image.color = eligibleSlotColor;
        });
    }

    public void StartTurn(IPlayer player)
    {
        if (player.GetPlayerType() == PlayerType.Player)
        {
            Debug.Log("ATTIVO!");
            endTurnBtn.interactable = true;
        } else
        {
            Debug.Log("DISATTIVO!");
            endTurnBtn.interactable = false;
        }

        RefreshBoard();
    }

    public void EndTurn()
    {
        
    }

    public void DoAttackAnim(IVisualCard attacker, IVisualCard target)
    {
        StartCoroutine(AttackAnimation(attacker, target));
    }

    private IEnumerator AttackAnimation(IVisualCard attacker, IVisualCard target)
    {
        Transform attackerTransform = attacker.GetTransform();
        Vector3 startPos = attackerTransform.position;

        Vector3 distance = target.GetTransform().position - startPos;
        distance = Vector3.MoveTowards(distance, distance * 0.001f, 1f);

        Vector3 cross = Vector3.Cross(attackerTransform.up, distance);
        float angle = Vector3.SignedAngle(attackerTransform.up, distance, cross);

        if (Vector3.Dot(cross, attackerTransform.forward) < 0)
        {
            angle = -angle;
        }

        // Animazione di attacco

        // 1) Ruotiamo la carta verso il target
        float rotationDuration = attacker.GetCard().CardData.attackAnimationData.rotationDuration;
        yield return attackerTransform.DORotate(new Vector3(0, 0, angle), rotationDuration).WaitForCompletion();

        // 2) Muoviamo la carta indietro (come per prendere la rincorsa)
        float backwardMoveDuration = attacker.GetCard().CardData.attackAnimationData.backwardMoveDuration;
        Vector3 backwardPosition = attackerTransform.position - attackerTransform.up * 0.8f;
        yield return attackerTransform.DOMove(backwardPosition, backwardMoveDuration).WaitForCompletion();

        // 3) Muoviamo la carta verso il target
        float forwardMoveDuration = attacker.GetCard().CardData.attackAnimationData.forwardMoveDuration;
        yield return attackerTransform.DOMove(startPos + distance, forwardMoveDuration).WaitForCompletion();

        // Attacco effettuato, aggiorniamo la UI delle carte
        attacker.RefreshVisualCard();
        target.RefreshVisualCard();

        // Calcola il danno e posizione per i numeri floating
        int damage = attacker.GetCard().CardData.baseDamage;
        Vector3 damageTextPosition = target.GetTransform().position;

        // Crea e mostra il numero di danno come UI temporanea
        StartCoroutine(ShowDamageFloatingText(damageTextPosition, damage));

        // 4) Muoviamo la carta verso la sua posizione iniziale
        float returnMoveDuration = attacker.GetCard().CardData.attackAnimationData.returnMoveDuration;
        yield return attackerTransform.DOMove(startPos, returnMoveDuration).WaitForCompletion();

        // 5) Ruotiamo la carta nella sua rotazione originaria
        float returnRotationDuration = attacker.GetCard().CardData.attackAnimationData.returnRotationDuration;
        yield return attackerTransform.DORotate(Vector3.zero, returnRotationDuration).WaitForCompletion();
    }

    private IEnumerator ShowDamageFloatingText(Vector3 position, int damage)
    {
        // Crea un'etichetta temporanea per mostrare il danno inflitto
        GameObject damageTextObject = Instantiate(damageTextPrefab, position, Quaternion.identity);
        damageTextObject.transform.SetParent(canvas.transform);
        damageTextObject.transform.localScale = Vector3.one;

        // Impostiamo il valore del danno
        TextMeshProUGUI damageText = damageTextObject.GetComponent<TextMeshProUGUI>();
        damageText.text = "-" + damage.ToString();

        // E lo muoviamo verso l'alto
        yield return damageTextObject.transform.DOMove(damageTextObject.transform.position + Vector3.up, 1f).WaitForCompletion();

        // Aggiungi un'animazione se necessario (es. fading out)
        Destroy(damageTextObject);
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

    public void BeginTargeting(IVisualCard attacker, Transform position, List<IVisualCard> eligibleTargets)
    {
        arrow.SetupAndActivate(position);

        // Evidenziamo i possibili targets
        eligibleTargets.ForEach(target =>
        {
            target.ChangeBorderColor(eligibleTargetBorderColor);
        });
    }

    public void EndTargeting(List<IVisualCard> eligibleTargets)
    {
        arrow.Deactivate();

        // Disattiviamo l'evidenziazione dei possibili target
        eligibleTargets.ForEach(target =>
        {
            target.ChangeBorderColor(defaultTargetBorderColor);
        });
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