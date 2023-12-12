namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class NoneGamesInBoardException : Exception
{
    public NoneGamesInBoardException(string message) : base(message) { }

    public NoneGamesInBoardException() : base("Currently there are not any games in the board.") { }
}
