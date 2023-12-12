namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class InvalidTeamException : Exception
{
    public InvalidTeamException(string message) : base(message) { }

    public InvalidTeamException() : base("The team name must be non empty and not null.") { }
}
