namespace FootballWorldCupScoreBoard.Domain.Models;

public class Game
{
    public string Id { get; set; }
    public Team HomeTeam { get; set; }
    public Team AwayTeam { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int TotalOverallScore
    {
        get
        {
            return HomeTeamScore + AwayTeamScore;
        }
    }
}
