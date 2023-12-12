using FootballWorldCupScoreBoard.Domain.Models;

namespace FootballWorldCupScoreBoard.Persistence.Interfaces;

public interface IGameRepository
{
    List<Game> GetAll();
    bool Any();
    bool ExistsByTeam(string teamName);
    Game GetById(string id);
    void Add(Game game);
    void Remove(Game game);
    Game UpdateScore(string id, int homeTeamScore, int awayTeamScore);
}
