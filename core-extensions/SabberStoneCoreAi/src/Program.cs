using System;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGamespace;
using SabberStoneCoreAi.Agent;
//using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Tyche2;
using SabberStoneCoreAi.Stats;
using SabberStoneCore.Model;
using SabberStoneCore.Meta;

namespace SabberStoneCoreAi
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			//Console.WriteLine("Testing? If yes, enter y. Otherwise, enter anything else\n");
			//string test = Console.ReadLine();

			//if (test == "y")
			//{
			//	var tests = new Testing();
			//}

			CardClass p1 = CardClass.INVALID;
			Deck d1;

			CardClass p2 = CardClass.INVALID;
			Deck d2;

			args[0] = args[0].ToLower();
			switch (args[0])
			{
				case "druid":
					p1 = CardClass.DRUID;
					d1 = Decks.BasicDruid;
					break;
				case "hunter":
					p1 = CardClass.HUNTER;
					d1 = Decks.BasicHunter;
					break;
				case "mage":
					p1 = CardClass.MAGE;
					d1 = Decks.BasicMage;
					break;
				case "paladin":
					p1 = CardClass.PALADIN;
					d1 = Decks.BasicPaladin;
					break;
				case "priest":
					p1 = CardClass.PRIEST;
					d1 = Decks.BasicPriest;
					break;
				case "rogue":
					p1 = CardClass.ROGUE;
					break;
				case "shaman":
					p1 = CardClass.SHAMAN;
					break;
				case "warlock":
					p1 = CardClass.WARLOCK;
					break;
				case "warrior":
					p1 = CardClass.WARRIOR;
					break;
				default:
					throw new ArgumentException($"Player1 Hero Class is invalid, must be either:\n" +
						$"Druid\n" +
						$"Hunter\n" +
						$"Mage\n" +
						$"Paladin\n" +
						$"Priest\n" +
						$"Rogue\n" +
						$"Shaman\n" +
						$"Warlock\n" +
						$"Warrior");
			}

			Console.WriteLine("Setup gameConfig");

			//AbstractAgent player1 = new MCTSAgent();
			AbstractAgent player1 = new TycheAgentCompetition(p1, p2, Decks.BasicPaladin);
			//AbstractAgent player1 = new RandomAgent();
			AbstractAgent player2 = new TycheAgentCompetition(p2, p1, Decks.BasicMage);

			var rnd = new Random();
			var gameConfig = new GameConfig
			{
				StartPlayer = rnd.Next(1, 2),
				Player1HeroClass = CardClass.PALADIN,
				Player1Name = "Tyche Paladin",
				Player1Deck = Decks.BasicPaladin,
				Player1Health = 30,
				//Player1HeroClass = CardClass.HUNTER,
				//Player1Deck = Decks.MidrangeSecretHunter,
				Player2HeroClass = CardClass.MAGE,
				Player2Deck = Decks.BasicMage,
				Player2Name = "Tyche Mage",
				Player2Health = 30,
				Shuffle = true,
				Logging = true,
				History = false
				//SkipMulligan = false
			};

			Console.WriteLine("Setup POGameHandler");

			var gameHandler = new POGameHandler(gameConfig, player1, player2);

			Console.WriteLine("PlayGame");
			//gameHandler.PlayGame();
			gameHandler.PlayGames(10);
			//GameStats gameStats = gameHandler.getGameStats();

			//gameStats.printResults();

			Console.WriteLine("Test successful");
			Console.ReadLine();
		}

		//	public static void TrainAgent(int gamesPlayed, AbstractAgent player1, Deck p1Deck, CardClass oppClass)
		//	{
		//		var rnd = new Random();

		//		int deckCount = GetAllOppDecks(oppClass).Count;

		//		//todo: rename to Main
		//		var gameConfig = new GameConfig
		//		{
		//			StartPlayer = rnd.Next(1, 2),
		//			Player1HeroClass = CardClass.WARRIOR,
		//			Player1Deck = p1Deck,
		//			Player2HeroClass = oppClass,
		//			Player2Deck = GetAllOppDecks(oppClass)[rnd.Next(deckCount)],
		//			Shuffle = true,
		//			Logging = true
		//		};

		//		//Console.WriteLine("Setup POGameHandler");
		//		AbstractAgent player2 = new MCTSAgent();
		//		var gameHandler = new POGameHandler(1, gamesPlayed, gameConfig, player1, player2, debug: false);

		//		gameHandler.PlayGame(gamesPlayed);
		//		//gameHandler.PlayGames();
		//		GameStats gameStats = gameHandler.getGameStats();

		//		gameStats.printResults();

		//		//Console.ReadLine();
		//	}

		//	public static List<Deck> GetAllOppDecks(CardClass oppClass)
		//	{
		//		switch (oppClass)
		//		{
		//			case CardClass.DRUID:
		//				return new DRUIDDecks().decks;

		//			case CardClass.HUNTER:
		//				return new HUNTERDecks().decks;

		//			case CardClass.MAGE:
		//				return new MAGEDecks().decks;

		//			case CardClass.PALADIN:
		//				return new PALADINDecks().decks;

		//			case CardClass.PRIEST:
		//				return new PRIESTDecks().decks;

		//			case CardClass.ROGUE:
		//				return new ROGUEDecks().decks;

		//			case CardClass.SHAMAN:
		//				return new SHAMANDecks().decks;

		//			case CardClass.WARLOCK:
		//				return new WARLOCKDecks().decks;

		//			case CardClass.WARRIOR:
		//				return new WARRIORDecks().decks;

		//			default:
		//				return new List<Deck>();
		//		}
		//	}
	}
}
