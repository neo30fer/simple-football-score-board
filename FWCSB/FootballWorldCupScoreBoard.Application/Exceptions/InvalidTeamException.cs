namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class InvalidTeamException : Exception
{
    public InvalidTeamException(string message) : base(message) { }
}
