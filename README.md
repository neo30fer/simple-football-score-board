# Football World Cup Score Board Simple Library
## Operations
The board supports the following operations:
1. Start a game. When a game starts, it should capture (being initial score 0 – 0):
  * Home team
  * Away team
3. Finish game. It will remove a match from the scoreboard.
4. Update score. Receiving the pair score; home team score and away team score updates a
game score.
5. Get a summary of games by total score. Those games with the same total score will be
returned ordered by the most recently added to our system.
As an example, being the current data in the system:
  * Mexico - Canada: 0 - 5
  * Spain - Brazil: 10 – 2
  * Germany - France: 2 – 2
  * Uruguay - Italy: 6 – 6
  * Argentina - Australia: 3 - 1

The summary would provide with the following information:
  1. Uruguay 6 - Italy 6
  2. Spain 10 - Brazil 2
  3. Mexico 0 - Canada 5
  4. Argentina 3 - Australia 1
  5. Germany 2 - France

## Technology stack
1. Base: .NET 6, C# 10
2. Testing: xUnit
3. Mocking: Moq

## Console Client Application
There is a Console Client Application which shows how to use the library (can be tested by starting the FootballWorldCupScoreBoard.ConsoleClient project).

## Notes
1. Created the solution considering: clean code, TDD, and tried to follow SOLID principles.
2. Implemented clean architecture, and separated the layers with the following folders and projects:
  * Core: Domain and Application projects
  * Infrastructure: Persistence project
  * Client: ConsoleClient project
  * Test: UnitTests project
4. Implemented repository pattern for data access.
