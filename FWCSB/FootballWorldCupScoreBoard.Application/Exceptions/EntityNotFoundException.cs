namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string entityName, string parameter, dynamic value)
        : base($"Cannot find {entityName} from '{parameter}' = '{value}'.") { }
}
