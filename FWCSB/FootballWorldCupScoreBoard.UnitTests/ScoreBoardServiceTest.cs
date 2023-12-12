using FootballWorldCupScoreBoard.Application.Exceptions;
using FootballWorldCupScoreBoard.Application.Services;
using FootballWorldCupScoreBoard.Domain.Models;
using FootballWorldCupScoreBoard.Persistence.Interfaces;
using Moq;

namespace FootballWorldCupScoreBoard.UnitTests;

public class ScoreBoardServiceTest
{
    private readonly Mock<IGameRepository> _gameRepository;
    private readonly Mock<ITeamRepository> _teamRepository;

    private readonly ScoreBoardService _scoreBoardService;

    public ScoreBoardServiceTest()
    {
        _gameRepository = new();
        _teamRepository = new();
        _scoreBoardService = new ScoreBoardService(_gameRepository.Object, _teamRepository.Object);
    }

    [Fact]
    public void StartGame_WhenSuccessfulPath_ShouldAddNewGameToBoard()
    {
        // Arrange
        _teamRepository.Setup(x => x.GetByName(It.IsAny<string>()))
            .Returns((string teamName) => GetMockedTeam(teamName));
        //_gameRepository.Setup(x => x.Add(It.IsAny<Game>()));

        // Act
        var game = _scoreBoardService.StartGame("Brazil", "England");

        // Assert
        Assert.NotNull(game);
        Assert.Equal("Brazil", game.HomeTeam.Name);
        Assert.Equal("England", game.AwayTeam.Name);
        Assert.Equal(0, game.HomeTeamScore);
        Assert.Equal(0, game.AwayTeamScore);
        _gameRepository.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
    }

    [Fact]
    public void StartGame_WhenHomeTeamParameterIsEmpty_ShouldReturnAnException()
    {
        // Act
        var exception = Assert.Throws<InvalidTeamException>(() => _scoreBoardService.StartGame(string.Empty, "England"));

        // Assert
        Assert.Equal("The 'homeTeamName' must be non empty and not null.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenHomeTeamParameterIsNull_ShouldReturnAnException()
    {
        // Act
        var exception = Assert.Throws<InvalidTeamException>(() => _scoreBoardService.StartGame(null, "England"));

        // Assert
        Assert.Equal("The 'homeTeamName' must be non empty and not null.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenAwayTeamParameterIsEmpty_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Setup(x => x.GetByName(It.IsAny<string>()))
            .Returns((string teamName) => GetMockedTeam(teamName));

        // Act
        var exception = Assert.Throws<InvalidTeamException>(() => _scoreBoardService.StartGame("Brazil", string.Empty));

        // Assert
        Assert.Equal("The 'awayTeamName' must be non empty and not null.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenAwayTeamParameterIsNull_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Setup(x => x.GetByName(It.IsAny<string>()))
            .Returns((string teamName) => GetMockedTeam(teamName));

        // Act
        var exception = Assert.Throws<InvalidTeamException>(() => _scoreBoardService.StartGame("Brazil", null));

        // Assert
        Assert.Equal("The 'awayTeamName' must be non empty and not null.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenHomeTeamIsAlreadyInBoardGame_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Setup(x => x.GetByName(It.IsAny<string>()))
            .Returns((string teamName) => GetMockedTeam(teamName));
        _gameRepository.Setup(x => x.ExistsByTeam("Brazil"))
            .Returns(true);

        // Act
        var exception = Assert.Throws<TeamAlreadyInBoardGameException>(() => _scoreBoardService.StartGame("Brazil", "England"));

        // Assert
        Assert.Equal("The team 'Brazil' is already playing in a current game.", exception.Message);
    }

    [Fact]
    public void StartGame_WhenAwayTeamIsAlreadyInBoardGame_ShouldReturnAnException()
    {
        // Arrange
        _teamRepository.Setup(x => x.GetByName(It.IsAny<string>()))
            .Returns((string teamName) => GetMockedTeam(teamName));
        _gameRepository.Setup(x => x.ExistsByTeam("Mexico"))
            .Returns(false);
        _gameRepository.Setup(x => x.ExistsByTeam("England"))
            .Returns(true);

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
        _gameRepository.Setup(x => x.Any())
            .Returns(true);
        _gameRepository.Setup(x => x.GetById(It.IsAny<string>()))
            .Returns(game);

        // Act
        _scoreBoardService.FinishGame(game);

        // Assert
        _gameRepository.Verify(x => x.Remove(It.IsAny<Game>()), Times.Once);
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
        _gameRepository.Setup(x => x.Any())
            .Returns(false);

        // Act
        var exception = Assert.Throws<NoneGamesInBoardException>(() => _scoreBoardService.FinishGame(game));

        // Assert
        Assert.Equal("Currently there are not any games in the board.", exception.Message);
    }

    [Fact]
    public void FinishGame_WhenGamesDoesNotExistsInBoard_ShouldReturnAnException()
    {
        // Arrange
        var game = GetMockedGame();
        _gameRepository.Setup(x => x.Any())
            .Returns(true);
        _gameRepository.Setup(x => x.GetById(It.IsAny<string>()))
            .Returns((Game)null);

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
        _gameRepository.Setup(x => x.GetById(It.IsAny<string>()))
            .Returns(game);
        _gameRepository.Setup(x => x.UpdateScore(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(() =>
            {
                game.HomeTeamScore = 1;
                return game;
            });

        // Act
        var updatedGame = _scoreBoardService.UpdateScore(game, 1, 0);

        // Assert
        Assert.NotNull(updatedGame);
        Assert.Equal(1, updatedGame.HomeTeamScore);
        Assert.Equal(0, updatedGame.AwayTeamScore);
        _gameRepository.Verify(x => x.UpdateScore(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
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
        _gameRepository.Setup(x => x.GetById(It.IsAny<string>()))
            .Returns(game);

        // Act
        var exception = Assert.Throws<InvalidScoreException>(() => _scoreBoardService.UpdateScore(game, homeTeamScore, awayTeamScore));

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void UpdateScore_WhenGamesDoesNotExistsInBoard_ShouldReturnAnException()
    {
        // Arrange
        var game = GetMockedGame();
        _gameRepository.Setup(x => x.GetById(It.IsAny<string>()))
            .Returns((Game)null);

        // Act
        var exception = Assert.Throws<EntityNotFoundException>(() => _scoreBoardService.UpdateScore(game, 1, 0));

        // Assert
        Assert.Equal("The game could not be found in the board.", exception.Message);
    }

    [Fact]
    public void GetBoardGamesSummary_WhenSuccessfulPath_ShouldReturnAListOfGames()
    {
        // Arrange
        _gameRepository.Setup(x => x.GetAll())
            .Returns(new List<Game>
            {
                GetMockedGame(1, 2),
                GetMockedGame(0, 0, "Spain", "Mexico"),
                GetMockedGame(2, 0, "Germany", "Italy"),
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

    private static Team GetMockedTeam(string teamName) => new() { Name = teamName };

    private static Game GetMockedGame(int homeTeamScore = 0, int awayTeamScore = 0, string homeTeamName = "Brazil", string awayTeamName = "England") =>
        new()
        {
            Id = Guid.NewGuid().ToString(),
            HomeTeam = GetMockedTeam(homeTeamName),
            AwayTeam = GetMockedTeam(awayTeamName),
            HomeTeamScore = homeTeamScore,
            AwayTeamScore = awayTeamScore
        };
}