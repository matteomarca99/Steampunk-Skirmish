public class ScoreManager : IScoreManager
{
    // Punteggi dei giocatori
    private int playerScore;
    private int opponentScore;
    private int lastPlayerScoreAdded;
    private int lastOpponentScoreAdded;

    public int PlayerScore { get => playerScore; set => playerScore = value; }
    public int OpponentScore { get => opponentScore; set => opponentScore = value; }
    public int LastPlayerScoreAdded { get => lastPlayerScoreAdded; set => lastPlayerScoreAdded = value; }
    public int LastOpponentScoreAdded { get => lastOpponentScoreAdded; set => lastOpponentScoreAdded = value; }
}