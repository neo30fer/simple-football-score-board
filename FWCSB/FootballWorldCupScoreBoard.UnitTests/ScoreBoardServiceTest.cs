using FootballWorldCupScoreBoard.Application.Exceptions;
using FootballWorldCupScoreBoard.Application.Services;
using FootballWorldCupScoreBoard.Domain.Models;
using FootballWorldCupScoreBoard.Persistence.Interfaces;
using FootballWorldCupScoreBoard.Persistence.Repositories;

namespace FootballWorldCupScoreBoard.UnitTests;

public class ScoreBoardServiceTest
{
    private readonly IGameRepository _gameRepository;
    private readonly ITeamRepository _teamRepository;

    private readonly ScoreBoardService _scoreBoardService;

    public ScoreBoardServiceTest()
    {
        _gameRepository = new GameRepository();
        _teamRepository = new TeamRepository();
        _scoreBoardService = new ScoreBoardService(_gameRepository, _teamRepository);
    }

    [Fact]
    public void StartGame_WhenSuccessfulPath_ShouldAddNewGameToBoard()
    {
        // Arrange
        _teamRepository.Add(GetMockedHomeTeam());
        _teamRepository.Add(GetMockedAwayTeam());

        // Act
        var game = _scoreBoardService.StartGame("Brazil", "England");

        // Assert
        var boardGame = _gameRepository.GetAll().FirstOrDefault(g => g.Id == game.Id); ;
        Assert.NotNull(boardGame);
        Assert.Equal("Brazil", boardGame.HomeTeam.Name);
        Assert.Equal("England", boardGame.AwayTeam.Name);
        Assert.Equal(0, boardGame.HomeTeamScore);
        Assert.Equal(0, boardGame.AwayTeamScore);
    }

    [Fact]
    public void StartGame_WhenHomeTeamParameterIsEmpty_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Add(GetMockedAwayTeam());

        // Act
        var exception = Assert.Throws<InvalidTeamException>(() => _scoreBoardService.StartGame(string.Empty, "England"));

        // Assert
        Assert.Equal("The 'homeTeamName' must be non empty and not null.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenAwayTeamParameterIsEmpty_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Add(GetMockedHomeTeam());

        // Act
        var exception = Assert.Throws<InvalidTeamException>(() => _scoreBoardService.StartGame("Brazil", string.Empty));

        // Assert
        Assert.Equal("The 'awayTeamName' must be non empty and not null.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenHomeTeamIsAlreadyInBoardGame_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Add(GetMockedHomeTeam());
        _teamRepository.Add(GetMockedAwayTeam());
        _gameRepository.Add(GetMockedGame());

        // Act
        var exception = Assert.Throws<TeamAlreadyInBoardGameException>(() => _scoreBoardService.StartGame("Brazil", "England"));

        // Assert
        Assert.Equal("The team 'Brazil' is already playing in a current game.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenAwayTeamIsAlreadyInBoardGame_ShouldReturnAnException()
    {
        // Arrange
        var homeTeam = new Team { Name = "Mexico" };
        var awayTeam = GetMockedAwayTeam();

        _teamRepository.Add(homeTeam);
        _teamRepository.Add(GetMockedAwayTeam());
        _gameRepository.Add(GetMockedGame());

        // Act
        var exception = Assert.Throws<TeamAlreadyInBoardGameException>(() => _scoreBoardService.StartGame("Mexico", "England"));

        // Assert
        Assert.Equal("The team 'England' is already playing in a current game.", exception.Message);
    }

    [Fact]
    public void FinishGame_WhenSuccessfulPath_ShouldRemoveGameFromBoard()
    {
        // Arrange
        var game = GetMockedGame();
        _gameRepository.Add(game);

        // Act
        _scoreBoardService.FinishGame(game);

        // Assert
        Assert.Empty(_gameRepository.GetAll());
    }

    [Fact]
    public void FinishGame_WhenGameParameterIsNull_ShouldReturnAnException()
    {
        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => _scoreBoardService.FinishGame(null));

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'game')", exception.Message);
    }

    [Fact]
    public void FinishGame_WhenThereAreNoGamesInBoard_ShouldReturnAnException()
    {
        // Arrange
        var game = GetMockedGame();

        // Act
        var exception = Assert.Throws<NoneGamesInBoardException>(() => _scoreBoardService.FinishGame(game));

        // Assert
        Assert.Equal("Currently there are not any games in the board.", exception.Message);
    }

    [Fact]
    public void FinishGame_WhenGamesDoesNotExistsInBoard_ShouldReturnAnException()
    {
        // Arrange
        var game = new Game
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = new Team { Name = "Mexico" },
            AwayTeam = new Team { Name = "Spain" },
            HomeTeamScore = 0,
            AwayTeamScore = 0
        };
        _gameRepository.Add(GetMockedGame());

        // Act
        var exception = Assert.Throws<EntityNotFoundException>(() => _scoreBoardService.FinishGame(game));

        // Assert
        Assert.Equal("The game could not be found in the board.", exception.Message);
    }

    [Fact]
    public void UpdateScore_WhenSuccessfulPath_ShouldUpdateGameScore()
    {
        // Arrange
        var game = GetMockedGame();
        _gameRepository.Add(game);

        // Act
        _scoreBoardService.UpdateScore(game, 1, 0);

        // Assert
        var updatedGame = _gameRepository.GetById(game.Id);
        Assert.NotNull(updatedGame);
        Assert.Equal(1, updatedGame.HomeTeamScore);
        Assert.Equal(0, updatedGame.AwayTeamScore);
    }

    [Fact]
    public void UpdateScore_WhenGameParameterIsNull_ShouldReturnAnException()
    {
        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => _scoreBoardService.UpdateScore(null, 0, 1));

        // Assert
        Assert.Equal("Value cannot be null. (Parameter 'game')", exception.Message);
    }

    [Theory]
    [MemberData(nameof(InvalidTeamScores))]
    public void UpdateScore_WhenHomeTeamScoreIsInvalid_ShouldReturnAnException(int homeTeamScore, int awayTeamScore, string expectedMessage)
    {
        // Arrange
        var game = GetMockedGame(1, 2);
        _gameRepository.Add(game);

        // Act
        var exception = Assert.Throws<InvalidScoreException>(() => _scoreBoardService.UpdateScore(game, homeTeamScore, awayTeamScore));

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void UpdateScore_WhenGamesDoesNotExistsInBoard_ShouldReturnAnException()
    {
        // Arrange
        var game = new Game
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = new Team { Name = "Mexico" },
            AwayTeam = new Team { Name = "Spain" },
            HomeTeamScore = 0,
            AwayTeamScore = 0
        };
        _gameRepository.Add(GetMockedGame());

        // Act
        var exception = Assert.Throws<EntityNotFoundException>(() => _scoreBoardService.UpdateScore(game, 1, 0));

        // Assert
        Assert.Equal("The game could not be found in the board.", exception.Message);
    }

    [Fact]
    public void GetBoardGamesSummary_WhenSuccessfulPath_ShouldReturnAListOfGames()
    {
        // Arrange
        _gameRepository.Add(GetMockedGame(1, 2));
        _gameRepository.Add(new Game
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = new Team { Name = "Spain" },
            AwayTeam = new Team { Name = "Mexico" },
            HomeTeamScore = 0,
            AwayTeamScore = 0
        });
        _gameRepository.Add(new Game
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = new Team { Name = "Germany" },
            AwayTeam = new Team { Name = "Italy" },
            HomeTeamScore = 2,
            AwayTeamScore = 0
        });

        // Act
        var boardGamesSummary = _scoreBoardService.GetBoardGamesSummary();

        // Assert
        Assert.NotEmpty(boardGamesSummary);
        Assert.Equal(3, boardGamesSummary.Count);
        Assert.All(boardGamesSummary, x => Assert.NotNull(x.Id));
        Assert.All(boardGamesSummary, x => Assert.NotNull(x.HomeTeam));
        Assert.All(boardGamesSummary, x => Assert.NotNull(x.AwayTeam));
        Assert.All(boardGamesSummary, x => Assert.True(x.HomeTeamScore >= 0));
        Assert.All(boardGamesSummary, x => Assert.True(x.AwayTeamScore >= 0));
        Assert.Equal("Brazil", boardGamesSummary.First().HomeTeam.Name);
        Assert.Equal("Mexico", boardGamesSummary.Last().AwayTeam.Name);
    }

    public static List<object[]> InvalidTeamScores =>
        new()
        {
            new object[] { -1, 0, "The score must be greater or equal than 0 (zero)." },
            new object[] { 0, -1, "The score must be greater or equal than 0 (zero)." },
            new object[] { 0, 2, "The new score for Home Team (0) cannot be less than the actual score (1)." },
            new object[] { 1, 1, "The new score for Home Team (1) cannot be less than the actual score (2)." },
        };

    private static Team GetMockedHomeTeam() => new() { Name = "Brazil" };

    private static Team GetMockedAwayTeam() => new() { Name = "England" };

    private static Game GetMockedGame(int homeTeamScore = 0, int awayTeamScore = 0) =>
        new()
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = GetMockedHomeTeam(),
            AwayTeam = GetMockedAwayTeam(),
            HomeTeamScore = homeTeamScore,
            AwayTeamScore = awayTeamScore
        };
}