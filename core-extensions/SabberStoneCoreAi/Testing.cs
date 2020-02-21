using System;
using Xunit;
using System.Collections.Generic;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCore.Config;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Enums;
using SabberStoneCore.Actions;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.HearthQuery;

namespace SabberStoneCoreAi
{
	internal class Testing
	{
		public Testing()
		{
			//var handTests = new HandTests();
			var deckTests = new DeckTests();
			//var predictionTests = new PredictionTests();
		}

		internal class DeckTests
		{
			MCTSAgent testAgent = new MCTSAgent();
			Deck testDeck = new Deck();

			public DeckTests()
			{
				//TestRemove();
				//TestAddToDB();
				TestFromDB();
				//TestGetOppDeck();
				//TestGetHeroClass();
				//TestDeckFromName();
			}

			void TestGetHeroClass()
			{
				testDeck = new Deck(CardClass.WARRIOR, Archetype.CONTROL, "TestDeck", 10, new List<Card>());
				CardClass test = testDeck.HeroClass;

				//test = testDeck.GetHeroClass("warrior");
				//test = testDeck.GetHeroClass("Warrior");
				//test = testDeck.GetHeroClass("WARRIOR");
			}

			void TestFromDB()
			{
				testDeck = testDeck.FromDB(CardClass.DRUID, deckName: "Malygos_Druid1");
			}

			void TestDeckFromName()
			{
				testDeck = new Deck(CardClass.WARRIOR, "Zoo_Warlock1");
			}

			void TestMostPopular()
			{
				testDeck = testDeck.FromDB(CardClass.DRUID, max: true);
			}

			//void TestAddToDB()
			//{
			//	testDeck = testDeck.FromDB(CardClass.WARRIOR, new WARRIORDecks().decks[0]);
			//}

			void TestGetOppDeck()
			{
				//testAgent.SetOpponentDeck(CardClass.WARLOCK);
			}

			void TestRemove()
			{
				testDeck = DeckQuery.DeckFromName("IceFireWarrior", CardClass.WARRIOR);

				testDeck.Remove(c => c.Name == "Fire Plume's Heart");

				var cards = new List<Card> { Cards.FromName("Goldshire Footman") };

				testDeck.Remove(c => cards.Contains(c));
			}
		};

		//internal class PredictionTests
		//{
		//	Deck testDeck = new Deck();
		//	Deck p1Deck = new DeckQuery(CardClass.WARRIOR).DeckFromName("IceFireWarrior");
		//	Deck p2Deck = new DeckQuery(CardClass.DRUID).DeckFromName("Malygos_Druid1");

		//	public PredictionTests()
		//	{
		//		TestPredictOpponentDeck();
		//	}

		//	void TestPredictOpponentDeck()
		//	{
		//		var rnd = new Random();
		//		var gameConfig = new GameConfig
		//		{
		//			StartPlayer = rnd.Next(1, 2),
		//			Player1HeroClass = CardClass.WARRIOR,
		//			Player1Name = "Garrosh Hellscream",
		//			Player1Deck = p1Deck,
		//			Player2HeroClass = CardClass.DRUID,
		//			Player2Name = "Malfurion Stormrage",
		//			Player2Deck = p2Deck,
		//			Shuffle = true,
		//			Logging = true
		//		};
		//		var player1 = new MCTSAgent();
		//		AbstractAgent player2 = new RandomAgent();

		//		player1.InitializeAgent();
		//		player2.InitializeAgent();

		//		var game = new Game(gameConfig);
		//		player1.InitializeGame();
		//		player2.InitializeGame();

		//		game.StartGame();
		//		var root = new RootNode(null, null, game, null);

		//		var testRoot = new RootNode(root, null, game, null);

		//		var test = new HearthTree(testRoot);

		//		HearthNode testSim = Simulate(test, 20);

		//		//testDeck = MCTSHelper.PredictOpponentDeck(test);
		//	}

		//	HearthNode Simulate(HearthTree tree, int endTurn)
		//	{
		//		HearthNode state = tree.Root.Clone();

		//		while (state.Game.State != State.COMPLETE && state.Game.State != State.INVALID && state.Game.Turn < endTurn)
		//		{
		//			if (state.PossibleActions.Count == 0)
		//				state = tree.SimPossibleActions(state);

		//			List<HearthNode> deathNodes = state.PossibleActions.FindAll(p => p.Wins == -100);
		//			if (deathNodes != null && state.PossibleActions.Count > 1)
		//				foreach (HearthNode deathNode in deathNodes)
		//					state.PossibleActions.Remove(deathNode);

		//			var rnd = new Random();
		//			int rndInd = rnd.Next(state.PossibleActions.Count);
		//			state = state.PossibleActions[rndInd];
		//		}

		//		return state;
		//	}
		//};

		public class HandTests
		{
			MCTSAgent testAgent = new MCTSAgent();

			public HandTests()
			{
				MoltenTest();
			}

			[Fact]
			public static void TransformationInHand()
			{
				var game = new Game(new GameConfig
				{
					StartPlayer = 1,
					FillDecks = true,
					FillDecksPredictably = true,
					Shuffle = false,
				});
				game.StartGame();

				IPlayable blade = Generic.DrawCard(game.Player1, Cards.FromName("Molten Blade"));
				IPlayable scroll = Generic.DrawCard(game.Player1, Cards.FromName("Shifting Scroll"));
				IPlayable zerus = Generic.DrawCard(game.Player1, Cards.FromName("Shifter Zerus"));

				game.Process(EndTurnTask.Any(game.CurrentPlayer));
				game.Process(EndTurnTask.Any(game.CurrentPlayer));

				// Next Turn
				Assert.Equal(blade.Cost, blade.Card.Cost);
				Assert.Equal(scroll.Cost, scroll.Card.Cost);
				if (zerus.AuraEffects != null)
					Assert.Equal(zerus.Cost, zerus.Card.Cost);

				game.Process(EndTurnTask.Any(game.CurrentPlayer));
				game.Process(EndTurnTask.Any(game.CurrentPlayer));

				// Next Turn
				Assert.Equal(blade.Cost, blade.Card.Cost);
				Assert.Equal(scroll.Cost, scroll.Card.Cost);
				if (zerus.AuraEffects != null)
					Assert.Equal(zerus.Cost, zerus.Card.Cost);
				Assert.Equal(zerus.Cost, zerus.Card.Cost);
			}

			[Fact]
			public static void MoltenTest()
			{
				var rnd = new Random();
				var p1Deck = new Deck(CardClass.WARRIOR, Archetype.QUEST, "IceFireWarrior", 100,
					new List<Card>
						{
							Cards.FromName("Fire Plume's Heart"),
							Cards.FromName("Goldshire Footman"),
							Cards.FromName("Wax Elemental"),
							Cards.FromName("Wax Elemental"),
							Cards.FromName("Cleave"),
							Cards.FromName("Cornered Sentry"),
							Cards.FromName("Drywhisker Armorer"),
							Cards.FromName("Drywhisker Armorer"),
							Cards.FromName("Execute"),
							Cards.FromName("Execute"),
							Cards.FromName("Plated Beetle"),
							Cards.FromName("Plated Beetle"),
							Cards.FromName("Warpath"),
							Cards.FromName("Warpath"),
							Cards.FromName("Phantom Militia"),
							Cards.FromName("Phantom Militia"),
							Cards.FromName("Shield Block"),
							Cards.FromName("Shield Block"),
							Cards.FromName("Stonehill Defender"),
							Cards.FromName("Stonehill Defender"),
							Cards.FromName("Brawl"),
							Cards.FromName("Direhorn Hatchling"),
							Cards.FromName("Direhorn Hatchling"),
							Cards.FromName("Rotten Applebaum"),
							Cards.FromName("Ornery Direhorn"),
							Cards.FromName("Unidentified Shield"),
							Cards.FromName("Unidentified Shield"),
							Cards.FromName("Geosculptor Yip"),
							Cards.FromName("Scourgelord Garrosh"),
							Cards.FromName("Molten Blade")
						});

				var p2Deck = new Deck();

				var gameConfig = new GameConfig
				{
					StartPlayer = rnd.Next(1, 2),
					Player1HeroClass = CardClass.WARRIOR,
					Player1Name = "Garrosh Hellscream",
					Player1Deck = p1Deck,
					Player2HeroClass = CardClass.DRUID,
					Player2Name = "Malfurion Stormrage",
					Player2Deck = p2Deck.FromDB(CardClass.DRUID, deckName: "Malygos_Druid1"),
					Shuffle = false,
					Logging = true,
					History = true,
					//SkipMulligan = false
				};

				var testGame = new Game(gameConfig);

				var player1 = new MCTSAgent();
				AbstractAgent player2 = new RandomAgent();

				player1.InitializeAgent();
				player2.InitializeAgent();

				player1.InitializeGame();
				player2.InitializeGame();

				testGame.StartGame();

				//bool check = true;
				//var molten = (IPlayable)testGame.CurrentPlayer.HandZone.Find(c => c.Type == CardType.WEAPON);
				//if (molten?.Cost != molten?.Card.Cost)
				//	check = false;
				var root = new HearthNode(null, testGame, null);

				//molten = (IPlayable)root.Game.CurrentPlayer.HandZone.Find(c => c.Type == CardType.WEAPON);
				//if (molten.Cost != molten.Card.Cost)
				//	check = false;

				HearthNode state = root.Frontier.Find(p => p.IsEndTurn);
				for (int i = 0; i < 5; ++i)
					state = state.Frontier.Find(p => p.IsEndTurn);
			}
		}
	}
}
