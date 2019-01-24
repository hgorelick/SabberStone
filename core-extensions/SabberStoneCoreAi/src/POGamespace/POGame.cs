using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Enums;
using SabberStoneCore.Exceptions;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;
using SabberStoneCore.Tasks;

namespace SabberStoneCoreAi.POGamespace
{

	public class POGame
	{
		public Game _game;
		public Game _origGame;
		private bool debug;

		public POGame(Game rootGame, bool debug)
		{
			_origGame = rootGame;
			_game = rootGame.Clone();
			_game.Player1.Game = _game;
			_game.Player2.Game = _game;
			//prepareOpponent();
			this.debug = debug;

			if (debug)
			{
				Console.WriteLine("Game Board");
				Console.WriteLine(rootGame.FullPrint());
			}
		}

		private void prepareOpponent()
		{
			int nr_deck_cards = _game.CurrentOpponent.DeckZone.Count;
			int nr_hand_cards = _game.CurrentOpponent.HandZone.Count;

			//_game.CurrentOpponent.Deck. = Decks.DebugDeck;

			//DebugCardsGen.AddAll(_game.CurrentOpponent.Deck.);
			_game.CurrentOpponent.HandZone = new HandZone(_game.CurrentOpponent);
			_game.CurrentOpponent.DeckZone = new DeckZone(_game.CurrentOpponent);

			for (int i = 0; i < nr_hand_cards; i++)
			{
				addCardToZone(_game.CurrentOpponent.HandZone, _game.CurrentOpponent.Deck[i], _game.CurrentOpponent);
			}

			for (int i = 0; i < nr_deck_cards; i++)
			{
				addCardToZone(_game.CurrentOpponent.DeckZone, _game.CurrentOpponent.Deck[i], _game.CurrentOpponent);
			}
		}

		private void addCardToZone(IZone zone, Card card, Controller player)
		{
			var tags = new Dictionary<GameTag, int>
			{
				[GameTag.ENTITY_ID] = _game.NextId,
				[GameTag.CONTROLLER] = player.PlayerId,
				[GameTag.ZONE] = (int)zone.Type
			};
			IPlayable playable = null;


			switch (card.Type)
			{
				case CardType.MINION:
					playable = new Minion(player, card, tags);
					break;

				case CardType.SPELL:
					playable = new Spell(player, card, tags);
					break;

				case CardType.WEAPON:
					playable = new Weapon(player, card, tags);
					break;

				case CardType.HERO:
					tags[GameTag.ZONE] = (int)SabberStoneCore.Enums.Zone.PLAY;
					tags[GameTag.CARDTYPE] = card[GameTag.CARDTYPE];
					playable = new Hero(player, card, tags);
					break;

				case CardType.HERO_POWER:
					tags[GameTag.COST] = card[GameTag.COST];
					tags[GameTag.ZONE] = (int)SabberStoneCore.Enums.Zone.PLAY;
					tags[GameTag.CARDTYPE] = card[GameTag.CARDTYPE];
					playable = new HeroPower(player, card, tags);
					break;

				default:
					throw new EntityException($"Couldn't create entity, because of an unknown cardType {card.Type}.");
			}

			zone?.Add(playable);
		}

		//private void CreateFullInformationGame(List<Card> deck_player1, DeckZone deckzone_player1, HandZone handzone_player1, List<Card> deck_player2, DeckZone deckzone_player2, HandZone handzone_player2)
		//{
		//	_game.Player1.Deck = deck_player1;
		//	_game.Player1.DeckZone = deckzone_player1;
		//	_game.Player1.HandZone = handzone_player1;

		//	_game.Player2.Deck. = deck_player2;
		//	_game.Player2.DeckZone = deckzone_player2;
		//	_game.Player2.HandZone = handzone_player2;
		//}

		public void Process(PlayerTask task)
		{
			_game.Process(task);
		}

		/**
		 * Simulates the tasks against the current _game and
		 * returns a Dictionary with the following POGame-Object
		 * for each task (or null if an exception happened
		 * during that _game)
		 */
		public Dictionary<PlayerTask, POGame> Simulate(List<PlayerTask> tasksToSimulate)
		{
			var simulated = new Dictionary<PlayerTask, POGame>();
			foreach (PlayerTask task in tasksToSimulate)
			{
				Game clone = _origGame.Clone();
				try
				{
					clone.Process(task);
					simulated.Add(task, new POGame(clone, debug));
				}

				catch (Exception)
				{
					simulated.Add(task, null);
				}
			}
			return simulated;
		}

		public POGame getCopy(bool? debug = null)
		{
			return new POGame(_origGame, debug ?? this.debug);
		}


		public string FullPrint()
		{
			return _game.FullPrint();
		}

		public string PartialPrint()
		{
			var str = new StringBuilder();
			if (_game.CurrentPlayer == _game.Player1)
			{
				str.AppendLine(_game.Player1.HandZone.FullPrint());
				str.AppendLine(_game.Player1.Hero.FullPrint());
				str.AppendLine(_game.Player1.BoardZone.FullPrint());
				str.AppendLine(_game.Player2.BoardZone.FullPrint());
				str.AppendLine(_game.Player2.Hero.FullPrint());
				str.AppendLine(String.Format("Opponent Hand Cards: {0}", _game.Player2.HandZone.Count));
			}
			if (_game.CurrentPlayer == _game.Player2)
			{
				str.AppendLine(String.Format("Opponent Hand Cards: {0}", _game.Player1.HandZone.Count));
				str.AppendLine(_game.Player1.Hero.FullPrint());
				str.AppendLine(_game.Player1.BoardZone.FullPrint());
				str.AppendLine(_game.Player2.BoardZone.FullPrint());
				str.AppendLine(_game.Player2.Hero.FullPrint());
				str.AppendLine(_game.Player2.HandZone.FullPrint());
			}

			return str.ToString();
		}


	}
}
