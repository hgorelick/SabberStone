using System;
using System.Text;
using System.Diagnostics;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.Utils;
using SabberStoneCoreAi.Stats;

namespace SabberStoneCoreAi.POGamespace
{
	class POGameHandler
	{
		bool _debug;

		//public readonly int NumGames;
		//public readonly int GamesPlayed;

		public AbstractAgent _player1 { get; protected set; }
		public AbstractAgent _player2 { get; protected set; }

		private GameConfig _gameConfig;
		private bool _setupHeroes = true;

		private GameStats gameStats;
		private static readonly Random Rnd = new Random();

		public POGameHandler(GameConfig gameConfig, AbstractAgent player1, AbstractAgent player2, int option=0, bool setupHeroes=true, bool debug=false)
		{
			//NumGames = numGames;
			//GamesPlayed = gamesPlayed;
			_gameConfig = gameConfig;
			_setupHeroes = setupHeroes;
			_player1 = player1;
			_player1.InitializeAgent();

			_player2 = player2;
			_player2.InitializeAgent();

			gameStats = new GameStats(gameConfig.Player1Name, gameConfig.Player2Name);
			_debug = debug;
		}

		public bool PlayGame(int gameNumber, bool addToGameStats=true)
		{
			var _masterGame = new Game(_gameConfig, _setupHeroes);
			_player1.InitializeGame();
			_player2.InitializeGame();

			_masterGame.StartGame();

			var _masterRoot = new RootNode(null, null, _masterGame, null);

			AbstractAgent _currentAgent;

			Stopwatch currentStopwatch;
			Stopwatch[] watches = new[] { new Stopwatch(), new Stopwatch() };

			var state = new RootNode(_masterRoot, null, _masterGame, null);

			try
			{
				while (state.Game.State != State.COMPLETE && state.Game.State != State.INVALID)
				{
					if (_debug)
						Console.WriteLine(state.PrintBoard(false));

					//state.Write("Sabber", false, true);

					_currentAgent = state.Game.CurrentPlayer == state.Game.Player1 ? _player1 : _player2;
					currentStopwatch = state.Game.CurrentPlayer == state.Game.Player1 ? watches[0] : watches[1];

					currentStopwatch.Start();
					HearthNode moveNode = _currentAgent.PlayTurn(state);
					if (moveNode.Action.PlayerTaskType == PlayerTaskType.PLAY_CARD)
						gameStats.AddCard(state.Game.CurrentPlayer.PlayerId);

					if (_debug)
						Console.Write(moveNode.PrintAction());

					state = new RootNode(moveNode.Root, null, moveNode.Game, moveNode.Action);
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
				_masterRoot.Game.State = State.COMPLETE;
				_masterRoot.Game.CurrentPlayer.PlayState = PlayState.CONCEDED;
				_masterRoot.Game.CurrentOpponent.PlayState = PlayState.WON;

				if (addToGameStats && _masterRoot.Game.State != State.INVALID)
					gameStats.registerException(_masterRoot.Game, e);
			}

			if (state.Game.State == State.INVALID)
				return false;

			if (addToGameStats)
			{
				gameStats.addGame(state.Game, watches);
			}

			_player1.FinalizeGame();
			_player2.FinalizeGame();

			return true;
		}

		public void PlayGames(int NumGames, bool addToGameStats=true)
		{
			for (int i = 0; i < NumGames; i++)
			{
				if (!PlayGame(i, addToGameStats))
					i -= 1;		// invalid _game
			}
			gameStats.FinalizeResults("GameStats_MCTSvsTyche" + DateTime.Now.ToString().Replace('/', '_'));
		}

		public GameStats getGameStats() { return gameStats; }
	}
}
