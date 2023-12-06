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
    /// <param name="board">La board a cui effettuare il refresh.</param>
    void RefreshBoard(IBoard board);

    /// <summary>
    /// Metodo utilizzato per mostrare gli slot idonei al posizionamento della carta.
    /// </summary>
    /// <param name="board">La board che contiene gli slot.</param>
    /// <param name="card">La carta da verificare.</param>
    void ShowEligibleslots(IBoard board, ICard card);

    /// <summary>
    /// Metodo utilizzato per attivare/disattivare la possibilita' del giocatore di interagire con la UI.
    /// </summary>
    /// <param name="player">Il giocatore di riferimento.</param>
    void TogglePlayerControls(IPlayer player);
}