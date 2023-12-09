using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager : ITurnManager
{
    private List<IPlayer> players;
    private IPlayer currentTurnPlayer;
    private int currentTurn = 1; // Contatore dei turni

    public TurnManager(List<IPlayer> players)
    {
        this.players = players;
        currentTurnPlayer = SelectRandomPlayer();
    }

    public void StartTurn()
    {
        Debug.Log("Inizio turno " + currentTurn + " del giocatore " + currentTurnPlayer.GetPlayerName());

        // Notifichiamo l'inizio del turno
        EventManager.TriggerEvent(EventType.OnStartTurn);
    }

    public void EndTurn()
    {
        Debug.Log("Fine turno " + " del giocatore " + currentTurnPlayer.GetPlayerName());

        currentTurn++;

        // Passa il turno al prossimo giocatore
        NextTurnPlayer();

        // E iniziamo un nuovo turno
        StartTurn();
    }


    public int GetCurrentTurn()
    {
        return currentTurn;
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
