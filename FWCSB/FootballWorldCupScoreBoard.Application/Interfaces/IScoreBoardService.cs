﻿using FootballWorldCupScoreBoard.Domain.Models;

namespace FootballWorldCupScoreBoard.Application.Interfaces;

public interface IScoreBoardService
{
    Game StartGame(string homeTeamName, string awayTeamName);
    void FinishGame(Game game);
    Game UpdateScore(Game game, int homeTeamScore, int awayTeamScore);
    List<Game> GetBoardGamesSummary();
}
