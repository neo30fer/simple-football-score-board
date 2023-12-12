using FootballWorldCupScoreBoard.Application.Services;
using FootballWorldCupScoreBoard.Domain.Models;

namespace FootballWorldCupScoreBoard.UnitTests;

public class ScoreBoardServiceTest
{
    private readonly ScoreBoardService _scoreBoardService;

    public ScoreBoardServiceTest()
    {
        _scoreBoardService = new ScoreBoardService();
    }

    [Fact]
    public void StartGame_WhenSuccessfulPath_ShouldAddNewGameToBoard()
    {
        // Arrange

        // Act
        var game = _scoreBoardService.StartGame("Brazil", "England");

        // Assert
        Assert.NotNull(game);
        Assert.Equal("Brazil", game.HomeTeam.Name);
        Assert.Equal("England", game.AwayTeam.Name);
        Assert.Equal(0, game.HomeTeamScore);
        Assert.Equal(0, game.AwayTeamScore);
    }

    [Fact]
    public void StartGame_WhenHomeTeamParameterIsEmpty_ShouldReturnAnException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void StartGame_WhenAwayTeamParameterIsEmpty_ShouldReturnAnException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void StartGame_WhenAwayTeamIsAlreadyInBoardGame_ShouldReturnAnException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void FinishGame_WhenSuccessfulPath_ShouldRemoveGameFromBoard()
    {
        // Arrange

        // Act

        // Assert
        // TODO: Check if game was removed from board games list
    }

    [Fact]
    public void FinishGame_WhenGameParameterIsNull_ShouldReturnAnException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void UpdateScore_WhenSuccessfulPath_ShouldUpdateGameScore()
    {
        // Arrange
        var game = GetMockedGame();

        // Act
        _scoreBoardService.UpdateScore(game, 1, 0);

        // Assert
        // TODO: Check if game score was succesfully updated
    }

    [Fact]
    public void UpdateScore_WhenGameParameterIsNull_ShouldReturnAnException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void GetBoardGamesSummary_WhenSuccessfulPath_ShouldReturnAListOfGames()
    {
        // Arrange

        // Act

        // Assert
    }

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