using System.Text;
using System.Linq;


using SabberStoneCore.Model;
using SabberStoneCore.Model.Zones;
using SabberStoneCore.Model.Entities;
using System.Numerics;
using System.Collections.Generic;

namespace SabberStoneCoreAi.StateVectorizer
{
	// TODO: Finish this...
	public class StateVector
	{
		public static readonly List<string> DefiniteGameKeys = new List<string>
		{
			"Turn",
			"CurrentPlayer",
			"Total Mana",
			"ManaRemaining",
			"Attack",
			"HP",
			"Armor",
			"Spell Damage",
			"Hand Count",
			"Deck Count",
			"Board Count",
			"Current Opponent",
			"Opponent Total Mana",
			"Opponent HP",
			"Opponent Armor",
			"Opponent Spell Damage",
			"Opponent Hand Count",
			"Opponent Deck Count",
			"Opponent Board Count",
		};

		public static readonly List<string> AbstractGameKeys = new List<string>
		{
			"Weapon",
			"Hand",
			"Board",
			"Deck",
			"Opponent Weapon",
			"Opponent Hand",	// for now, we'll cheat
			"Opponent Board",	//
			"Opponent Deck"		//
		};

		public StateVector(Game game)
		{
			var str = new StringBuilder();
			str.Append
			str.Append(game.CurrentPlayer.Hero.Vectorize());
			str.Append(game.CurrentOpponent.Hero.Vectorize());

		}

		public string Vectorize(Hero h)
		{
			var str = new StringBuilder();
			str.AppendLine($"Current:{h.Card.Name}");

			return str.ToString();
		}

		public string Vectorize(Zone<IPlayable> z)
		{
			var str = new StringBuilder();

			return str.ToString();
		}
	}

	public class CardVector
	{
		public static readonly List<string> CardKeys = new List<string>
		{
			"Cost"
		};
	}
}
