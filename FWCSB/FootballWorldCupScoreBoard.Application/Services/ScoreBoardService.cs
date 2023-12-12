using FootballWorldCupScoreBoard.Application.Exceptions;
using FootballWorldCupScoreBoard.Application.Interfaces;
using FootballWorldCupScoreBoard.Domain.Models;
using FootballWorldCupScoreBoard.Persistence.Interfaces;

namespace FootballWorldCupScoreBoard.Application.Services;

public class ScoreBoardService : IScoreBoardService
{
    private readonly IGameRepository _gameRepository;
    private readonly ITeamRepository _teamRepository;

    public ScoreBoardService(IGameRepository gameRepository, ITeamRepository teamRepository)
    {
        _gameRepository = gameRepository;
        _teamRepository = teamRepository;
    }

    public Game StartGame(string homeTeamName, string awayTeamName)
    {
        #region Validations
        if (string.IsNullOrEmpty(homeTeamName))
        {
            throw new InvalidTeamException($"The '{nameof(homeTeamName)}' must be non empty and not null.");
        }

        if (string.IsNullOrEmpty(awayTeamName))
        {
            throw new InvalidTeamException($"The '{nameof(awayTeamName)}' must be non empty and not null.");
        }

        var homeTeam = _teamRepository.GetByName(homeTeamName) ?? throw new EntityNotFoundException(nameof(Team), nameof(Team.Name), homeTeamName);
        var awayTeam = _teamRepository.GetByName(awayTeamName) ?? throw new EntityNotFoundException(nameof(Team), nameof(Team.Name), awayTeamName);

        // Check if there is already a game in progress in the board with any of the given teams
        CheckIfTeamIsInBoardGameOrThrow(homeTeam);
        CheckIfTeamIsInBoardGameOrThrow(awayTeam);
        #endregion

        // Add game to the board list
        var game = new Game
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            HomeTeamScore = 0,
            AwayTeamScore = 0
        };
        _gameRepository.Add(game);

        return game;
    }

    public void FinishGame(Game game)
    {
        #region Validations
        if (game is null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        if (!_gameRepository.Any())
        {
            throw new NoneGamesInBoardException();
        }

        var existingGame = _gameRepository.GetById(game.Id) ?? throw new EntityNotFoundException("The game could not be found in the board.");
        #endregion

        // Remove game from the board list
        _gameRepository.Remove(existingGame);
    }

    public Game UpdateScore(Game game, int homeTeamScore, int awayTeamScore)
    {
        #region Validations
        if (game is null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        if (homeTeamScore < 0)
        {
            throw new InvalidScoreException();
        }

        if (awayTeamScore < 0)
        {
            throw new InvalidScoreException();
        }

        var existingGame = _gameRepository.GetById(game.Id) ?? throw new EntityNotFoundException("The game could not be found in the board.");

        if (homeTeamScore < existingGame.HomeTeamScore)
        {
            throw new InvalidScoreException($"The new score for Home Team ({homeTeamScore}) cannot be less than the actual score ({existingGame.HomeTeamScore}).");
        }

        if (awayTeamScore < existingGame.AwayTeamScore)
        {
            throw new InvalidScoreException($"The new score for Home Team ({awayTeamScore}) cannot be less than the actual score ({existingGame.AwayTeamScore}).");
        }
        #endregion

        // Update game score
        return _gameRepository.UpdateScore(game.Id, homeTeamScore, awayTeamScore);
    }

    public List<Game> GetBoardGamesSummary()
    {
        var boardGames = _gameRepository.GetAll();
        if (!boardGames.Any())
        {
            throw new NoneGamesInBoardException();
        }

        return boardGames
            .OrderByDescending(x => x.TotalOverallScore)
            .ThenByDescending(x => x.CreatedAt)
            .ToList();
    }

    private void CheckIfTeamIsInBoardGameOrThrow(Team team)
    {
        if (_gameRepository.ExistsByTeam(team.Name))
        {
            throw new TeamAlreadyInBoardGameException(team);
        }
    }
}
