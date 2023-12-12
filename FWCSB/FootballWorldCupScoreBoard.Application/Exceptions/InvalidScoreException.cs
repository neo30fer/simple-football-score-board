namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class InvalidScoreException : Exception
{
    public InvalidScoreException(string message) : base(message) { }

    public InvalidScoreException() : base("The score must be greater or equal than 0 (zero).") { }
}
