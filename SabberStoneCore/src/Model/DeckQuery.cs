using System;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Enums;

namespace SabberStoneCore.HearthQuery
{
	public static class DeckQuery
	{
		#region OLD -- Non-Static Class Members
		//const string connection = @"DataSource=C:\Users\hgore\SabberStone\SabberStoneCore\resources\Data\hearth_decks.db;";
		//DataTable Deck = new DataTable();
		//DataTable DeckNames = new DataTable();
		//#endregion

		//CardClass _heroClass { get; set; }
		//public CardClass HeroClass => _heroClass;

		//public DeckQuery(CardClass heroClass) { _heroClass = heroClass; }

		/// <summary>
		/// Returns the deck with the most number of games played from hearth_decks.db
		/// </summary>
		/// <returns></returns>
		//public static Deck GetMostPopular()
		//{
		//	string query = $"select * from AllInfo where num_games = (select max(num_games) from AllInfo where hero_class = '{HeroClass.ToString()}');";
		//	var Deck = new DataTable();

		//	using (var conn = new SQLiteConnection(connection))
		//	{
		//		var cmd = new SQLiteCommand(query, conn);
		//		using (var adapter = new SQLiteDataAdapter { SelectCommand = cmd })
		//		{
		//			adapter.Fill(Deck);
		//		}
		//	}

		//	var cards = new List<Card>();
		//	//var col = Deck.Columns[4];

		//	foreach (DataRow row in Deck.Rows)
		//		cards.Add(Cards.FromName(row.ItemArray[4].ToString()));

		//	Archetype archetype = DeckHelper.Map<Archetype>(Deck.Rows[0].ItemArray[1].ToString());
		//	string deckName = Deck.Rows[0].ItemArray[2].ToString();
		//	int numGames = (int)(long)Deck.Rows[0].ItemArray[3];

		//	return new Deck(HeroClass, archetype, deckName, numGames, cards);
		//}
		#endregion

		static readonly string connection = $"DataSource={Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName}" +
											$"\\SabberStoneCore\\resources\\Data\\hearth_decks.db;";

		public static Deck GetMostPopular(CardClass heroClass)
		{
			string query = $"select * from AllInfo where num_games = (select max(num_games) from AllInfo where hero_class = '{heroClass.ToString()}');";
			var Deck = new DataTable();

			using (var conn = new SQLiteConnection(connection))
			{
				//var cmd = new SQLiteCommand(query, conn);
				using (var adapter = new SQLiteDataAdapter { SelectCommand = new SQLiteCommand(query, conn) })
				{
					adapter.Fill(Deck);
				}
			}

			var cards = new List<Card>();
			//var col = Deck.Columns[4];

			foreach (DataRow row in Deck.Rows)
				cards.Add(Cards.FromName(row.ItemArray[4].ToString()));

			Archetype archetype = DeckHelper.Map<Archetype>(Deck.Rows[0].ItemArray[1].ToString());
			string deckName = Deck.Rows[0].ItemArray[2].ToString();
			int numGames = (int)(long)Deck.Rows[0].ItemArray[3];

			return new Deck(heroClass, archetype, deckName, numGames, cards);
		}

		/// <summary>
		/// Returns the specified deck from hearth_decks.db
		/// </summary>
		/// <param name="deckName"></param>
		/// <returns></returns>
		public static Deck DeckFromName(string deckName, CardClass heroClass)
		{
			string query = $"select archetype, num_games, card_name from AllInfo where deck_name = '{deckName}';";
			var Deck = new DataTable();

			using (var conn = new SQLiteConnection(connection))
			{
				conn.Open();
				//var cmd = new SQLiteCommand(query, conn);
				using (var adapter = new SQLiteDataAdapter { SelectCommand = new SQLiteCommand(query, conn) })
				{
					adapter.Fill(Deck);
				}
			}

			var cards = new List<Card>();
			//var col = Deck.Columns[4];

			foreach (DataRow row in Deck.Rows)
				cards.Add(Cards.FromName(row.ItemArray[2].ToString()));

			Archetype archetype = DeckHelper.Map<Archetype>(Deck.Rows[0].ItemArray[0].ToString());
			int numGames = (int)(long)Deck.Rows[0].ItemArray[1];

			return new Deck(heroClass, archetype, deckName, numGames, cards);
		}

		/// <summary>
		/// Returns the deck from hearth_decks.db with the most matching cards in psf.
		/// Can be filtered to only search through the decks with names in possible.
		/// </summary>
		/// <param name="psf"></param>
		/// <param name="possible"></param>
		/// <returns></returns>
		public static List<string> GetBestMatch(List<Card> psf, CardClass heroClass, List<string> possible = null)
		{
			string query = $"select distinct deck_name from AllInfo where hero_class = '{heroClass}';";
			var DeckNames = new DataTable();

			var matches = new List<string>();
			using (var conn = new SQLiteConnection(connection))
			{
				conn.Open();
				List<string> names = GetDeckNames(heroClass);

				int matchCount = 0;
				foreach (string name in names)
				{
					query = $"select card_name from deck_cards where deck_name = '{name}';";
					var Deck = new DataTable();

					//var cmd = new SQLiteCommand(query, conn);
					using (var adapter = new SQLiteDataAdapter { SelectCommand = new SQLiteCommand(query, conn) })
					{
						adapter.Fill(Deck);
					}

					var cards = new List<string>();
					foreach (DataRow row in Deck.Rows)
					{
						string card = row.ItemArray[0].ToString();
						if (psf.Contains(c => c.Name == card))
							cards.Add(card);
					}

					if (cards.Count >= matchCount)
					{
						if (cards.Count > matchCount)
							matches.Clear();

						matchCount = cards.Count;
						matches.Add(name);
					}
				}
			}
			return matches;
		}

		/// <summary>
		/// Returns a list of each deck's name in this HeroClass
		/// </summary>
		/// <returns></returns>
		static List<string> GetDeckNames(CardClass heroClass)
		{
			string query = $"select distinct deck_name from AllInfo where hero_class = '{heroClass}';";
			var DeckNames = new DataTable();

			using (var conn = new SQLiteConnection(connection))
			{
				conn.Open();
				//var cmd = new SQLiteCommand(query, conn);
				using (var adapter = new SQLiteDataAdapter { SelectCommand = new SQLiteCommand(query, conn) })
				{
					adapter.Fill(DeckNames);
				}
			}

			var names = new List<string>();
			foreach (DataRow row in DeckNames.Rows)
				names.Add(row.ItemArray[0].ToString());

			return names;
		}
	}

	internal static class QHelper
	{
		internal static bool Contains(this List<Card> l, Func<Card, bool> p)
		{
			foreach (Card c in l)
			{
				if (p(c))
					return true;
			}
			return false;
		}
	}
}
