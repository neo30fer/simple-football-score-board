using FootballWorldCupScoreBoard.Application.Services;
using FootballWorldCupScoreBoard.Domain.Models;
using FootballWorldCupScoreBoard.Persistence.Repositories;
using System.Text;

Console.WriteLine("Football World Cup Score Board Test Client");

TeamRepository teamRepository = new();
var team1 = new Team { Name = "Mexico" };
var team2 = new Team { Name = "Canada" };
var team3 = new Team { Name = "Spain" };
var team4 = new Team { Name = "Brazil" };
var team5 = new Team { Name = "Germany" };
var team6 = new Team { Name = "France" };
var team7 = new Team { Name = "Uruguay" };
var team8 = new Team { Name = "Italy" };
var team9 = new Team { Name = "Argentina" };
var team10 = new Team { Name = "Australia" };
teamRepository.Add(team1);
teamRepository.Add(team2);
teamRepository.Add(team3);
teamRepository.Add(team4);
teamRepository.Add(team5);
teamRepository.Add(team6);
teamRepository.Add(team7);
teamRepository.Add(team8);
teamRepository.Add(team9);
teamRepository.Add(team10);

ScoreBoardService scoreBoardService = new(new GameRepository(), teamRepository);

var game1 = scoreBoardService.StartGame(team1.Name, team2.Name);
var game2 = scoreBoardService.StartGame(team3.Name, team4.Name);
var game3 = scoreBoardService.StartGame(team5.Name, team6.Name);
var game4 = scoreBoardService.StartGame(team7.Name, team8.Name);
var game5 = scoreBoardService.StartGame(team9.Name, team10.Name);

scoreBoardService.UpdateScore(game1, 0, 5);
scoreBoardService.UpdateScore(game2, 10, 2);
scoreBoardService.UpdateScore(game3, 2, 2);
scoreBoardService.UpdateScore(game4, 6, 6);
scoreBoardService.UpdateScore(game5, 3, 1);

var gamesSummary = scoreBoardService.GetBoardGamesSummary();
StringBuilder stringBuilder = new();
for(int i = 0; i < gamesSummary.Count; i++)
{
    var game = gamesSummary[i];
    stringBuilder.Append($"{i + 1}. {game.HomeTeam.Name} {game.HomeTeamScore} - {game.AwayTeam.Name} {game.AwayTeamScore}\n");
}
Console.WriteLine(stringBuilder.ToString());