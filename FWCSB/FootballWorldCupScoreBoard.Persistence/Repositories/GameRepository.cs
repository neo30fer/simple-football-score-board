using FootballWorldCupScoreBoard.Domain.Models;
using FootballWorldCupScoreBoard.Persistence.Interfaces;

namespace FootballWorldCupScoreBoard.Persistence.Repositories;

public class GameRepository : IGameRepository
{
    public List<Game> BoardGames { get; set; } = new List<Game>();

    public List<Game> GetAll() => BoardGames;
    
    public bool Any() => BoardGames.Any();

    public bool ExistsByTeam(string teamName) => BoardGames.Any(g => g.HomeTeam.Name == teamName || g.AwayTeam.Name == teamName);
    
    public Game GetById(string id) => BoardGames.FirstOrDefault(g => g.Id == id);

    public void Add(Game game) => BoardGames.Add(game);

    public void Remove(Game game) => BoardGames.Remove(game);

    public Game UpdateScore(string id, int homeTeamScore, int awayTeamScore)
    {
        var game = GetById(id);
        if (game is not null)
        {
            game.HomeTeamScore = homeTeamScore;
            game.AwayTeamScore = awayTeamScore;
        }
        return game;
    }
}
