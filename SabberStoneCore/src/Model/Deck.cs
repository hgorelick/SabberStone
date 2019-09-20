using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SabberStoneCore.Enums;
using SabberStoneCore.HearthQuery;

namespace SabberStoneCore.Model
{
	public enum Archetype
	{
		INVALID = -1,
		ZOO = 0,
		ODD = 1,
		EVEN = 2,
		DEATHRATTLE = 3,
		TAUNT = 4,
		SECRET = 5,
		QUEST = 6,
		TOKEN = 7,
		MILL = 8,
		MIDRANGE = 9,
		TEMPO = 10,
		CONTROL = 11,
		BIG = 12,
		COMBO = 13,
		CUBE = 14,
		ELEMENTAL = 15,
		FREEZE = 16,
		KINGSBANE = 17,
		MALYGOS = 18,
		MECH = 19,
		MECHATHUN = 20,
		MURLOC = 21,
		OTK = 22,
		OVERLOAD = 23,
		POGO = 24,
		RECRUIT = 25,
		RESURRECT = 26,
		SHUDDERWOCK = 27,
		SPELL = 28,
		SPITEFUL = 29,
		TURVY = 30,
		CUSTOM = 31
	}

	/// <summary>
	/// Defining members and constructors
	/// </summary>
	public partial class Deck : Collection<Card>
	{
		CardClass _heroClass { get; set; }
		public CardClass HeroClass => _heroClass;

		Archetype _archetype { get; set; }
		public Archetype Archetype => _archetype;

		string _deckName { get; set; }
		public string DeckName => _deckName;

		int _numGames { get; set; }
		public int NumGames => _numGames;

		/// <summary>
		/// Default constructor
		/// </summary>
		public Deck()
		{
			_heroClass = CardClass.INVALID;
			_archetype = Archetype.INVALID;
			_deckName = "new deck";
			_numGames = 0;
			Clear();
		}

		/// <summary>
		/// Constructor that takes all deck information
		/// </summary>
		/// <param name="heroClass"></param>
		/// <param name="archetype"></param>
		/// <param name="deckName"></param>
		/// <param name="num_games"></param>
		/// <param name="cards"></param>
		public Deck(CardClass heroClass = CardClass.INVALID, Archetype archetype = Archetype.INVALID, string deckName = "new deck", int num_games = 0, List<Card> cards = null)
		{
			_heroClass = heroClass;
			_archetype = archetype;
			_deckName = deckName;
			_numGames = num_games;

			if (cards != null)
			{
				Clear();
				this.AddRange(cards);
			}
		}
		bool OpponentSet { get; set; } = false;

		/// <summary>
		/// Overloaded constructor that extracts the archetype from the deck's name
		/// </summary>
		/// <param name="heroClass"></param>
		/// <param name="archetype"></param>
		/// <param name="deckName"></param>
		/// <param name="num_games"></param>
		/// <param name="cards"></param>
		public Deck(CardClass heroClass = CardClass.INVALID, string deckName = "new deck", int num_games = 0, List<Card> cards = null)
		{
			_heroClass = heroClass;

			try { _archetype = DeckHelper.Map<Archetype>(deckName.Split('_')[0]); }

			catch
			{
				if (deckName == "IceFireWarrior")
					_archetype = Archetype.QUEST;
				else
					_archetype = Archetype.CUSTOM;
			}

			_deckName = deckName;
			_numGames = num_games;

			if (cards != null)
			{
				Clear();
				this.AddRange(cards);
			}
		}

		/// <summary>
		/// Creates the most popular deck of the given hero class
		/// </summary>
		/// <param name="heroClass"></param>
		public Deck(CardClass heroClass)
		{
			_heroClass = heroClass;

			var deck = new Deck(DeckQuery.GetMostPopular(heroClass));

			_archetype = deck.Archetype;
			_deckName = deck.DeckName;
			_numGames = deck.NumGames;

			Clear();
			this.AddRange(deck.ToList());
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="other"></param>
		Deck(Deck other)
		{
			_heroClass = other.HeroClass;
			_archetype = other.Archetype;
			_deckName = other.DeckName;
			_numGames = other.NumGames;

			Clear();
			this.AddRange(other.ToList());
		}

		/// <summary>
		/// Calls the copy constructor
		/// </summary>
		/// <returns></returns>
		public Deck Clone() { return new Deck(this); }
	}

	/// <summary>
	/// Getters, helper methods, and Collection<T> overrides
	/// </summary>
	public partial class Deck : Collection<Card>
	{
		/// <summary>
		/// Returns a deck from a sqlite database
		/// </summary>
		/// <param name="deckName"></param>
		public Deck FromDB(CardClass heroClass, Deck deck = null, bool max = false, string deckName = "none")
		{
			if (deck != null)
			{
				//try { return deckQuery.AddToDB(deck); }

				//catch (Exception)
				//{
				return deck;
				//}
			}

			else if (max)
				return new Deck(DeckQuery.GetMostPopular(heroClass));

			else if (deckName != "none")
				return DeckQuery.DeckFromName(deckName, heroClass);

			throw new Exception($"There was a problem retrieving the deck from the database, or the arguments are incorrect");
		}
	}

	public static class DeckHelper
	{
		/// <summary>
		/// Returns the archetype that matches the given string
		/// </summary>
		/// <param name="archetype"></param>
		/// <returns></returns>
		public static T Map<T>(this string value)
		{
			if ((String.IsNullOrEmpty(value) == false) && typeof(T).IsEnum)
			{
				try { return (T)(Enum.Parse(typeof(T), value, true)); }
				catch { }
			}
			throw new InvalidCastException();
		}

		/// <summary>
		/// Collection<Card> override
		/// </summary>
		/// <typeparam name="Card"></typeparam>
		/// <param name="dest"></param>
		/// <param name="source"></param>
		public static void AddRange(this Collection<Card> dest, List<Card> source)
		{
			foreach (Card card in source)
				dest.Add(card);
		}

		/// <summary>
		/// Remove<> override
		/// </summary>
		/// <param name="p"></param>
		public static void Remove(this Deck d, Func<Card, bool> p)
		{
			foreach (Card c in d)
				if (p(c))
				{
					d.Remove(c);
					return;
				}
		}

		public static void ForEach(this Deck d, Action<Card> a)
		{
			foreach (Card c in d)
				a(c);
		}
	}
};
