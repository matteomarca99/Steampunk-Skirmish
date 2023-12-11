using System;
using System.Collections.Generic;

/// <summary>
/// Interfaccia che definisce l'interfaccia di gioco (UI).
/// </summary>
public interface IGameUI
{
    /// <summary>
    /// Metodo utilizzato per effettuare il refresh delle carte di un giocatore (UI).
    /// </summary>
    /// <param name="player">Il giocatore alla quale verra' effettuato il refresh delle carte.</param>
    void RefreshHand(IPlayer player);

    /// <summary>
    /// Metodo utilizzato per effettuare il refresh delle carte di una lista di giocatori (UI).
    /// </summary>
    /// <param name="players">La lista di giocatori alla quale verra' effettuato il refresh delle carte.</param>
    void RefreshHand(List<IPlayer> players);

    /// <summary>
    /// Metodo utilizzato per effettuare il refresh degli slot sul terreno di gioco (UI).
    /// </summary>
    void RefreshBoard();

    /// <summary>
    /// Metodo utilizzato per mostrare gli slot idonei al posizionamento della carta.
    /// </summary>
    /// <param name="visualCard">La carta da verificare.</param>
    void ShowEligibleslots(IVisualCard visualCard);

    /// <summary>
    /// Metodo utilizzato per attivare la logica UI di quando inizia un turno (es: attivare i controlli del giocatore).
    /// </summary>
    /// <param name="player">Il giocatore del turno corrente.</param>
    void StartTurn(IPlayer player);

    /// <summary>
    /// Metodo utilizzato per attivare la logica UI di quando finisce un turno (es: disattivare i controlli del giocatore).
    /// </summary>
    /// <param name="scoreManager">Il gestore dei punteggi.</param>
    void EndTurn(IScoreManager scoreManager);
}