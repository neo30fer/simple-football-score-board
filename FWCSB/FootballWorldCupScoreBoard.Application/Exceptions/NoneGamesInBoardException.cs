namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class NoneGamesInBoardException : Exception
{
    public NoneGamesInBoardException() : base("Currently there are not any games in the board.") { }
}
