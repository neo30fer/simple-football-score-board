using FootballWorldCupScoreBoard.Application.Interfaces;
using FootballWorldCupScoreBoard.Domain.Models;

namespace FootballWorldCupScoreBoard.Application.Services;

public class ScoreBoardService : IScoreBoardService
{
    public Game StartGame(string homeTeamName, string awayTeamName)
    {
        return new Game
        {
            HomeTeam = new Team { Name = homeTeamName },
            AwayTeam = new Team { Name = awayTeamName },
            HomeTeamScore = 0,
            AwayTeamScore = 0
        };
    }

    public void FinishGame(Game game)
    {
        throw new NotImplementedException();
    }

    public void UpdateScore(Game game, int homeTeamScore, int awayTeamScore)
    {
        game.HomeTeamScore = homeTeamScore;
        game.AwayTeamScore = awayTeamScore;
    }

    public List<Game> GetBoardGamesSummary()
    {
        throw new NotImplementedException();
    }
}
