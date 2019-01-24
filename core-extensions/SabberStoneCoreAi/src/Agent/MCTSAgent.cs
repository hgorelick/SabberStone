using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Zones;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Enums;
using SabberStoneCore.Kettle;
using SabberStoneCore.HearthQuery;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.Utils;

namespace SabberStoneCoreAi.Agent
{
	public class MCTSAgent : AbstractAgent
	{
		Game BlindGame { get; set; }
		bool OpponentSet { get; set; } = false;

		Controller Opponent { get; set; }
		Deck OpponentDeck { get; set; }
		HandZone OpponentHandZone { get; set; }
		DeckZone OpponentDeckZone { get; set; }
		SecretZone OpponentSecretZone { get; set; }
		GraveyardZone OpponentGraveyardZone { get; set; }

		List<Card> PlayedSoFar = new List<Card>();
		List<string> PossibleOpponentDecks = new List<string>();

		HearthTree SimTree;

		/// <summary>
		/// Performs a monte carlo tree search on the current game-state,
		/// and then selects the move with the highest score
		/// </summary>
		/// <param name="state"></param>
		/// <returns>The best move determined by <see cref="MCTSHelper.DoMCTS(MCTSAgent, HearthNode)"/></returns>
		public override HearthNode PlayTurn(HearthNode state)
		{
			if (!OpponentSet)
				SetupOpponent(state.Game);

			HearthNode move = PrepAndProcessMCTS(state);

			//move.Game.PowerHistory.Dump(@"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\SabberStone.log", end: move.Game.State == State.COMPLETE ? true : false);
			//move.Game.PowerHistory = new PowerHistory();

			//move.Write(filename, false, false, true);
			return move;
		}

		/// <summary>
		/// Performs a MCTS to select the best action at this current game state
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		HearthNode PrepAndProcessMCTS(HearthNode state)
		{
			PredictOpponentDeck(state);

			HearthNode MCTSNode = DontCheat(state);
			MCTSNode.FixMolten();

			SimTree = new HearthTree(MCTSNode);
			HearthNode move = SimTree.MCTS();

			state.BirthPossibility(move);
			return move;
		}

		/// <summary>
		/// Makes BlindGame a clone of the current game, but replaces all of the
		/// blind information with this agent's best predictions.
		/// </summary>
		/// <param name="realGame"></param>
		/// <param name="firstTurn"></param>
		HearthNode DontCheat(HearthNode state)
		{
			if (PlayedSoFar.Count < 1 || PossibleOpponentDecks.Count == 1)
				return state.Clone();

			UpdateOpponent(BlindGame.CurrentOpponent.HandZone.Count);

			return new RootNode(state.Root, state.Parent, BlindGame, state.Action);
		}

		/// <summary>
		/// Initializes this agent's estimation of the opponent
		/// </summary>
		/// <param name="opponent"></param>
		/// <returns></returns>
		void SetupOpponent(Game game)
		{
			BlindGame = game.Clone();
			Opponent = BlindGame.CurrentOpponent;
			Opponent.Game = BlindGame;

			OpponentDeck = new DeckQuery(Opponent.HeroClass).GetMostPopular();
			OpponentDeckZone = new DeckZone(Opponent)
			{
				Controller = Opponent
			};

			Opponent.Deck = OpponentDeck.Clone();
			Opponent.DeckZone = OpponentDeckZone;

			OpponentGraveyardZone = new GraveyardZone(Opponent);
			OpponentSecretZone = new SecretZone(Opponent);

			OpponentSet = true;
		}

		/// <summary>
		/// Updates this agent's estimation of the opponent
		/// </summary>
		void UpdateOpponent(int handSize)
		{
			Opponent.Game = BlindGame;
			Opponent.Deck = OpponentDeck.Clone();
			Opponent.DeckZone = OpponentDeckZone.Clone(Opponent);
			//OpponentDeckZone.Controller = Opponent;

			OpponentHandZone = new HandZone(Opponent);
			//{
			//	Controller = Opponent
			//};

			foreach (Card card in OpponentDeck)
				Entity.FromCard(Opponent, card, null, OpponentDeckZone);

			foreach (Card card in PlayedSoFar)
				if (OpponentDeckZone.Contains(card))
					OpponentDeckZone.Remove(card);

			for (int i = 0; i < handSize; ++i)
			{
				IPlayable rnd = OpponentDeckZone.Random;
				OpponentDeckZone.Remove(rnd);
				rnd.Controller = OpponentHandZone.Controller;
				OpponentHandZone.Add(rnd);
			}

			Opponent.HandZone = OpponentHandZone.Clone(Opponent);
			OpponentGraveyardZone = BlindGame.CurrentOpponent.GraveyardZone.Clone(Opponent);

			if (BlindGame.CurrentOpponent.SecretZone.Count == 0)
				OpponentSecretZone = new SecretZone(Opponent);

			else if	(BlindGame.CurrentOpponent.SecretZone.Count == 1 && BlindGame.CurrentOpponent.SecretZone.Contains(s => s.IsQuest))
				OpponentSecretZone = BlindGame.CurrentOpponent.SecretZone.Clone(Opponent);

			else
			{
				OpponentSecretZone = new SecretZone(Opponent);
				if (!OpponentDeck.Contains(c => c.IsSecret))
					AddBlindSecrets();

				else
				{
					var secretsPlayed = OpponentGraveyardZone.Where(p => p.Card.IsSecret).ToList();
					if (secretsPlayed != null)
					{
						List<string> secrets = GetSecrets(Opponent.HeroClass);
						foreach (Spell s in secretsPlayed)
						{
							if (secretsPlayed.Count(s) > 2)
								secrets.Remove(s.Card.Name);
						}

						if (secrets.Count != 0)
							AddBlindSecrets(secrets);

						else
							AddBlindSecrets(GetSecrets(Opponent.HeroClass));
					}
				}
			}

			BlindGame.Player2 = Opponent;
			BlindGame.CurrentPlayer.Game = BlindGame;
			BlindGame.CurrentOpponent.Game = BlindGame;
		}

		/// <summary>
		/// Predicts the opponents deck based on the cards they've played so far.
		/// And, initializes this agent's BlindGame object.
		/// </summary>
		/// <returns></returns>
		void PredictOpponentDeck(HearthNode state)
		{
			if (state.Game.Turn > 1 && PossibleOpponentDecks.Count != 1)
			{
				PlayedSoFar = state.GetPlayedSoFar(state.Game.CurrentOpponent);

				BlindGame = state.Game.Clone();
				Opponent = BlindGame.CurrentOpponent;
				var deckQuery = new DeckQuery(Opponent.HeroClass);

				if (PlayedSoFar.Any(c => c.Name == "The Coin"))
					PlayedSoFar.RemoveAll(c => c.Name == "The Coin");

				if (PlayedSoFar.Count == 0)
					OpponentDeck = deckQuery.GetMostPopular();

				var bestMatches = new List<string>();

				if (PossibleOpponentDecks.Count == 0)
					bestMatches = deckQuery.GetBestMatch(PlayedSoFar);

				else if (PossibleOpponentDecks.Count == 1)
				{
					if (PossibleOpponentDecks[0] == OpponentDeck.DeckName)
						return;

					OpponentDeck = deckQuery.DeckFromName(PossibleOpponentDecks[0]);
				}

				else
					bestMatches = deckQuery.GetBestMatch(PlayedSoFar, PossibleOpponentDecks);

				if (bestMatches.Count == 1)
				{
					PossibleOpponentDecks.Clear();
					PossibleOpponentDecks.Add(bestMatches[0]);
				}

				else if (bestMatches.Count > 1)
					for (int i = 1; i < bestMatches.Count; ++i)
						PossibleOpponentDecks.Add(bestMatches[i]);

				OpponentDeck = deckQuery.DeckFromName(bestMatches[0]);
			}
		}

		/// <summary>
		/// Retuns a list of names of the secrets in the opponent's class
		/// </summary>
		/// <returns></returns>
		List<string> GetSecrets(CardClass heroClass)
		{
			switch (heroClass)
			{
				case CardClass.HUNTER:
					return MCTSHelper.HunterSecrets;

				case CardClass.MAGE:
					return MCTSHelper.MageSecrets;

				case CardClass.PALADIN:
					return MCTSHelper.PaladinSecrets;

				case CardClass.ROGUE:
					return MCTSHelper.RogueSecrets;

				default:
					return null;
			}
		}

		/// <summary>
		/// Fills the opponent's secret zone with possible secrets from the proper classes
		/// based on the real cards in the opponent's secret zone.
		/// </summary>
		/// <returns></returns>
		void AddBlindSecrets()
		{
			foreach (Spell s in BlindGame.CurrentOpponent.SecretZone)
			{
				List<string> secrets = GetSecrets(s.Card.Class);
				var rnd = new Random();
				int rndInd = 0;
				Card secret;

				do
				{
					rndInd = rnd.Next(secrets.Count);
					secret = Cards.FromName(secrets.PopAt(rndInd));
				}
				while (OpponentSecretZone.Contains(secret));

				OpponentSecretZone.Add(secret as IPlayable);
			}
		}

		/// <summary>
		/// Adds the same number of secret's to OpponentSecretZone, as there
		/// are in reality, from the given list of secrets.
		/// </summary>
		/// <param name="secrets"></param>
		void AddBlindSecrets(List<string> secrets)
		{
			for (int i = 0; i < BlindGame.CurrentOpponent.SecretZone.Count; ++i)
			{
				var rnd = new Random();
				int rndInd = 0;
				Card secret;

				do
				{
					rndInd = rnd.Next(secrets.Count);
					secret = Cards.FromName(secrets.PopAt(rndInd));
				}
				while (OpponentSecretZone.Contains(secret));

				OpponentSecretZone.Add(secret as IPlayable);
			}
		}

		public override void InitializeAgent() { }
		public override void InitializeGame() { }
		public override void FinalizeGame() { }
		public override void FinalizeAgent() { }

		#region OLD STUFF
		/// <summary>
		/// Writes information about the games in Experiences so that they
		/// can be loaded later and used for learning
		/// </summary>
		/// <param name="numGames"></param>
		/// <param name="oppClass"></param>
		//public override void WriteGames(CardClass oppClass)
		//{
		//	Encoding encoding = Encoding.Unicode;
		//	try
		//	{
		//		var fs = new FileStream($"C:\\Users\\hgore\\SabberStone\\core-extensions" +
		//			$"\\SabberStoneCoreAi\\src\\Meta\\Train_vs_{oppClass}.data", FileMode.CreateNew);

		//		ExpWriter = new BinaryWriter(fs);

		//		try
		//		{
		//			foreach (KeyValuePair<EndTurnNode, string> exp in Experiences)
		//			{
		//				ExpWriter.Write(Hash(oppClass, exp.Key));
		//				HearthNode parent = exp.Key.Parent;
		//				while (parent != null)
		//				{
		//					ExpWriter.Write($"{{EXP:[{parent.Hash}]}}");
		//					parent = parent.Parent;
		//				}
		//			}
		//		}
		//		catch (Exception e)
		//		{
		//			Console.WriteLine(e.Message + "\n Cannot create file.");
		//			throw e;
		//		}

		//		fs.Close();
		//	}
		//	catch (Exception e)
		//	{
		//		var fs = new FileStream($"C:\\Users\\hgore\\SabberStone\\core-extensions" +
		//			$"\\SabberStoneCoreAi\\src\\Meta\\Train_vs_{oppClass}.data", FileMode.Append);

		//		ExpWriter = new BinaryWriter(fs);

		//		try
		//		{
		//			foreach (KeyValuePair<EndTurnNode, string> exp in Experiences)
		//			{
		//				ExpWriter.Write(Hash(oppClass, exp.Key));
		//				HearthNode parent = exp.Key.Parent;
		//				while (parent != null)
		//				{
		//					ExpWriter.Write($"{{EXP:[{parent.Hash}]}}");
		//					parent = parent.Parent;
		//				}
		//			}
		//		}
		//		catch (Exception)
		//		{
		//			Console.WriteLine(e.Message + "\n Cannot create file.");
		//			throw e;
		//		}

		//		fs.Close();
		//	}
		//	Experiences.Clear();
		//}

		/// <summary>
		/// Reads game information from a file and loads the information
		/// into Experiences
		/// </summary>
		/// <param name="oppClass"></param>
		//public override void ReadGames(int numGames, CardClass oppClass)
		//{
		//	FileStream fs = File.Open($"C:\\Users\\hgore\\SabberStone\\core-extensions" +
		//			$"\\SabberStoneCoreAi\\src\\Meta\\Train_{numGames + 1}_Games_vs_{oppClass}.data", FileMode.Open);

		//	ExpReader = new BinaryReader(fs);

		//	int pos = 0;
		//	int length = (int)ExpReader.BaseStream.Length;

		//	while (pos < length)
		//	{

		//	}
		//}


		/// <summary>
		/// Builds game information to be read by future versions of the MCTS 
		/// </summary>
		/// <returns></returns>
		//private string Hash(CardClass oppHero, EndTurnNode end)
		//{
		//	var str = new StringBuilder();
		//	str.Append($"{{OHC:{oppHero}}}");
		//	str.Append($"{{ODN:{(end.Game.CurrentOpponent == end.Game.Player2 ? end.Game.CurrentOpponent.Deck._deckName : end.Game.CurrentPlayer.Deck._deckName)}}}");
		//	str.Append($"{{W:{(end.Wins)}}}");

		//	return str.ToString();
		//}
		#endregion

	}
}
