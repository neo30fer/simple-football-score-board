using FootballWorldCupScoreBoard.Domain.Models;

namespace FootballWorldCupScoreBoard.Application.Exceptions;

public class TeamAlreadyInBoardGameException : Exception
{
    public TeamAlreadyInBoardGameException(Team team)
        : base($"The team '{team.Name}' is already playing in a current game.") { }
}
