using FootballWorldCupScoreBoard.Domain.Models;
using FootballWorldCupScoreBoard.Persistence.Interfaces;

namespace FootballWorldCupScoreBoard.Persistence.Repositories;

public class TeamRepository : ITeamRepository
{
    public List<Team> Teams { get; set; } = new List<Team>();

    public List<Team> GetAll() => Teams;
    
    public Team? GetByName(string name) => Teams.FirstOrDefault(t => t.Name == name);
    
    public void Add(Team team) => Teams.Add(team);
}
