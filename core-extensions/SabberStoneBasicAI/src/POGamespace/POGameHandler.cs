using System;
using System.Diagnostics;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.Utils;
using SabberStoneCoreAi.Stats;
using System.Collections.Generic;
using System.Collections.Specialized;
using SabberStoneCore.HearthVector;

namespace SabberStoneCoreAi.POGamespace
{
	class POGameHandler
	{
		readonly string _debugMode;

		//public readonly int NumGames;
		//public readonly int GamesPlayed;

		public AbstractAgent _player1 { get; protected set; }
		public AbstractAgent _player2 { get; protected set; }

		private GameConfig _gameConfig;
		private bool _setupHeroes = true;

		//private GameStats gameStats;
		private static readonly Random Rnd = new Random();

		public POGameHandler(GameConfig gameConfig, AbstractAgent player1, AbstractAgent player2, bool setupHeroes = true, string debugMode = "")
        {
			//NumGames = numGames;
			//GamesPlayed = gamesPlayed;
			_gameConfig = gameConfig;
			_setupHeroes = setupHeroes;
			_player1 = player1;
			_player1.InitializeAgent();

			_player2 = player2;
			_player2.InitializeAgent();

			//gameStats = new GameStats(gameConfig.Player1Name, gameConfig.Player2Name);
			_debugMode = debugMode;
		}

		public bool PlayGame(int gameNumber, bool addToGameStats=true)
		{
			var _masterGame = new Game(_gameConfig, _setupHeroes);
			_player1.InitializeGame();
			_player2.InitializeGame();

			_masterGame.StartGame();

			var state = HearthNode.CreateRoot(_masterGame);

			AbstractAgent _currentAgent;

			Stopwatch currentStopwatch;
			Stopwatch[] watches = new[] { new Stopwatch(), new Stopwatch() };

			//var state = new HearthNode(_masterRoot, null, _masterGame, null);

			//List<int> gameStateVector = state.Vector();
			//string dt = DateTime.Now.ToString("MM_dd_yyyy HH_mm");
			//int turn = 1;
			//int actionNum = 1;
			var heroClasses = new CardClass[2] { state.Game.Player1.HeroClass, state.Game.Player2.HeroClass };
			string filename = $"{state.Game.Player1.Deck.Name}_vs_{state.Game.Player2.Deck.Name}.csv";

			try
			{
				while (state.Game.State != State.COMPLETE && state.Game.State != State.INVALID)
				{
					state.Game.WriteCSV((gameNumber + 1).ToString(), heroClasses, filename);

					if (_debugMode == "")
						Console.WriteLine(state.PrintBoard());

					//else if (_debugMode == "python")
					//	Console.WriteLine(state.PrintBoard());


					_currentAgent = state.Game.CurrentPlayer == state.Game.Player1 ? _player1 : _player2;
					//perspective = state.Game.CurrentPlayer == state.Game.Player1 ? 1 : 2;
					currentStopwatch = state.Game.CurrentPlayer == state.Game.Player1 ? watches[0] : watches[1];

					currentStopwatch.Start();
					HearthNode moveNode = _currentAgent.PlayTurn(state);
					//moveNode.Game.WriteCSV(dt, heroClasses, filename);

					//actionNum = moveNode.Game.Turn == turn ? actionNum + 1 : 1;

					//if (moveNode.Action.PlayerTaskType == PlayerTaskType.PLAY_CARD)
					//	gameStats.AddCard(state.Game.CurrentPlayer.PlayerId);

					if (_debugMode != null)
						Console.Write(moveNode.PrintAction());

					state = new HearthNode(null, moveNode.Game, moveNode.Action);
					//turn = state.Game.Turn;
					//gameStateVector = state.Vector();
					currentStopwatch.Stop();

					state.Game.CurrentPlayer.Game = state.Game;
					state.Game.CurrentOpponent.Game = state.Game;

					///if (_debug)
					///{
					///	Console.WriteLine(playerMove);
					///	if (playerMove.PlayerTaskType == PlayerTaskType.PLAY_CARD)
					///		Console.WriteLine($"\n{currentPlayer.Hero.Card.Name} plays {playerMove.Source.Card.Name}\n");
					///
					///	if (playerMove.PlayerTaskType == PlayerTaskType.HERO_ATTACK)
					///		Console.WriteLine($"\n{currentPlayer.Hero.Card.Name} attacks {currentPlayer.Opponent.Hero.Card.Name}\n");
					///
					///	if (playerMove.PlayerTaskType == PlayerTaskType.MINION_ATTACK)
					///		Console.WriteLine($"\n{playerMove.Source.Card.Name} attacks {playerMove.Target.Card.Name}\n");
					///
					///	if (playerMove.PlayerTaskType == PlayerTaskType.HERO_POWER)
					///		Console.WriteLine($"\n{currentPlayer.Hero.Card.Name} used {currentPlayer.Hero.HeroPower.Card.Name}");
					///}
					///state.Process(playerMove);
					///state = state.Children.Find(s => s._action == playerMove).GetChild(playerMove);
				}
			}

			catch (Exception e)
			//Current Player loses if he throws an exception
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				state.Game.State = State.COMPLETE;
				state.Game.CurrentPlayer.PlayState = PlayState.CONCEDED;
				state.Game.CurrentOpponent.PlayState = PlayState.WON;

				//if (addToGameStats && _masterRoot.Game.State != State.INVALID)
				//	gameStats.registerException(_masterRoot.Game, e);
			}

			if (state.Game.State == State.INVALID)
				return false;

			//if (addToGameStats)
			//{
			//	gameStats.addGame(state.Game, watches);
			//}

			string winner = state.Game.Player1.PlayState == PlayState.WON ?
				state.Game.Player1.Hero.Card.Name : state.Game.Player2.Hero.Card.Name;

			Console.WriteLine($"{winner} won!");

			int winnerId = state.Game.Player1.PlayState == PlayState.WON ?
				state.Game.Player1.PlayerId : state.Game.Player2.PlayerId;

			state.Game.WriteCSV((gameNumber + 1).ToString(), heroClasses, filename, winner: winnerId);

			_player1.FinalizeGame();
			_player2.FinalizeGame();

			return true;
		}

		public void PlayGames(int NumGames, bool addToGameStats=false)
		{
			for (int i = 0; i < NumGames; i++)
			{
				if (!PlayGame(i, addToGameStats))
					i -= 1;		// invalid _game
			}

			//if (addToGameStats)
			//	gameStats.FinalizeResults("GameStats_MCTSvsTyche" + DateTime.Now.ToString().Replace('/', '_').Replace(':', '_'));
		}

		//public GameStats getGameStats() { return gameStats; }
	}
}
