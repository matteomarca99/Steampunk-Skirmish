using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager : ITurnManager
{
    private List<IPlayer> players;
    private IPlayer currentTurnPlayer;
    private int currentTurn = 0; // Contatore dei turni
    private int currentRound = 1; // Contatore dei round

    public TurnManager(List<IPlayer> players)
    {
        this.players = players;
        currentTurnPlayer = SelectRandomPlayer();
    }

    public void StartTurn()
    {
        Debug.Log("Inizio turno " + currentTurn + " del giocatore " + currentTurnPlayer.GetPlayerName() + " (Round: " + currentRound + ")");

        // Notifichiamo l'inizio del turno
        EventManager.TriggerEvent(EventType.OnStartTurn);
    }

    public void EndTurn()
    {
        Debug.Log("Fine turno " + " del giocatore " + currentTurnPlayer.GetPlayerName());

        currentTurn++;

        // Se abbiamo completato i due turni (un round), incrementiamo il round
        if (currentTurn % players.Count == 0)
        {
            currentRound++;
        }

        // Passa il turno al prossimo giocatore
        NextTurnPlayer();

        // E iniziamo un nuovo turno
        StartTurn();
    }


    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }

    public IPlayer GetCurrentTurnPlayer()
    {
        return currentTurnPlayer;
    }

    IPlayer SelectRandomPlayer()
    {
        int randomIndex = Random.Range(0, players.Count); // Genera un indice casuale
        return players[randomIndex]; // Restituisce il giocatore casuale
    }

    void NextTurnPlayer()
    {
        int currentIndex = players.IndexOf(currentTurnPlayer);
        int nextIndex = (currentIndex + 1) % players.Count;
        currentTurnPlayer = players[nextIndex];
    }
}
