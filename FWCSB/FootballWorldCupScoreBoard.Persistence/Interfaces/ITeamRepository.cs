using FootballWorldCupScoreBoard.Domain.Models;

namespace FootballWorldCupScoreBoard.Persistence.Interfaces;

public interface ITeamRepository
{
    List<Team> GetAll();
    Team? GetByName(string name);
    void Add(Team team);
}
