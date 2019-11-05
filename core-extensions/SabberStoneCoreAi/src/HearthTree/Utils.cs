using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


using SabberStoneCore.Model;
using SabberStoneCore.Model.Zones;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.Enums;
using SabberStoneCore.Kettle;
using SabberStoneCoreAi.HearthNodes;

namespace SabberStoneCoreAi.Utils
{
	/// <summary>
	/// Static helper methods
	/// </summary>
	public static class MCTSHelper
	{
		/// <summary>
		/// Checks for and fixes a bug in which Molten Blade costs 1 mana after transformation.
		/// Returns true if the bug was fixed.
		/// </summary>
		/// <param name="state"></param>
		public static bool FixMolten(this HearthNode state)
		{
			var molten = (IPlayable)state.Game.CurrentPlayer.HandZone.Find(c => c.Type == CardType.WEAPON);
			if (molten != null)
			{
				if (molten.Cost != molten.Card.Cost)
				{
					int moltInd = state.Game.CurrentPlayer.HandZone.IndexOf(molten);
					//state.Game.CurrentPlayer.HandZone.Remove(molten);
					state.Game.CurrentPlayer.HandZone[moltInd].Cost = molten.Card.Cost;
					//state.Game.CurrentPlayer.HandZone.Insert(moltInd, molten);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns a list of the cards the opponent has played so far
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static List<Card> GetPlayedSoFar(this HearthNode state, Controller opponent)
		{
			var playedSoFar = new List<Card>();
			int currentPlayer = opponent.PlayerId;

			while (state.Parent != null)
			{
				if (state.Game.CurrentPlayer.PlayerId == currentPlayer)
				{
					if (state.Action.PlayerTaskType == PlayerTaskType.PLAY_CARD && state.Action.HasSource)
					{
						if (!state.Action.Source?.Card.IsSecret ?? false)
						{
							Card card = state.Action.Source.Card;
							playedSoFar.Add(card);
						}
					}
				}
				state = state.Parent;
			}
			return playedSoFar;
		}

		/// <summary>
		/// Returns the total number of minions in play
		/// </summary>
		/// <param name="game"></param>
		/// <returns></returns>
		public static int BoardCount(this Game game)
		{
			return game.CurrentPlayer.BoardZone.Count + game.CurrentOpponent.BoardZone.Count;
		}

		/// <summary>
		/// Returns a list of this HearthNode's children that will
		/// win the game if selected consecutively.
		/// </summary>
		/// <param name="h"></param>
		/// <returns></returns>
		public static List<HearthNode> GetLethalMoves(this HearthNode h)
		{
			//for (int i = 0; i < h.Children.Count; ++i)
			//{
			//	h.Children[i].Process();
			//}

			if (h.Children.Contains(c => c.Damage > 0))
			{
				int oppHealth = h.Game.CurrentOpponent.TotalHealth();

				var damageMoves = h.Children.Where(c => c.Damage > 0).ToList();
				HearthNode fatigue = damageMoves.Pop(c => c.IsEndTurn);

				int damageAvailable = damageMoves.DamageAvailable();

				if (damageAvailable > oppHealth)
					return damageMoves;

				else if (fatigue != null)
				{
					if (damageAvailable + fatigue.Damage > oppHealth)
					{
						damageMoves.Add(fatigue);
						return damageMoves;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Returns true if each of this HearthNode's children executes an Attack action.
		/// </summary>
		/// <param name="h"></param>
		/// <returns></returns>
		public static bool AttackOnly(this HearthNode h)
		{
			return h.Frontier.SkipWhile(p => p.IsEndTurn).ToList().All(p =>
			   (p.Action.PlayerTaskType == PlayerTaskType.MINION_ATTACK || p.Action.PlayerTaskType == PlayerTaskType.HERO_ATTACK)
				&& h.Game.CurrentOpponent.BoardZone.IsEmpty
				&& h.Game.CurrentOpponent.SecretZone.IsEmpty);
		}

		/// <summary>
		/// Returns the sum of this controller's health and armor
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int TotalHealth(this Controller c)
		{
			return c.Hero.Health + c.Hero.Armor;
		}

		/// <summary>
		/// Returns the sum of each HearthNode's damage in list h
		/// </summary>
		/// <param name="h"></param>
		/// <returns></returns>
		public static int DamageAvailable(this List<HearthNode> h)
		{
			return h.Sum(c => c.Damage);
		}

		public static List<string> HunterSecrets = new List<string>
		{
		"Explosive Trap",
		"Freezing Trap",
		"Snipe",
		"Misdirection",
		"Venomstrike Trap",
		"Wandering Monster",
		"Rat Trap",
		"Snake Trap"
		};

		public static List<string> MageSecrets = new List<string>
		{
			"Frozen Clone",
			"Ice Barrier",
			"Mirror Entity",
			"Counterspell",
			"Explosive Runes",
			"Mana Bind",
			"Vaporize",
			"Spellbender"
		};

		public static List<string> PaladinSecrets = new List<string>
		{
			"Autodefense Matrix",
			"Eye for an Eye",
			"Noble Sacrifice",
			"Redemption",
			"Repentance",
			"Hidden Wisdom"
		};

		public static List<string> RogueSecrets = new List<string>
		{
			"Cheat Death",
			"Sudden Betrayal",
			"Evasion"
		};
	}

	public static class GameWriter
	{
		/// <summary>
		/// Writes game information to a file including the game board and moves
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="gameInfo"></param>
		public static void Write(this HearthNode state, string filename, bool create = false, bool board = false, bool move = false)
		{
			string newfile = filename;
			string dir = @"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\";
			string path = dir + newfile + ".json";
			int count = 1;

			while (File.Exists(path) && File.GetLastWriteTime(path) + TimeSpan.FromMinutes(3) < DateTime.Now)
			{
				newfile += count++.ToString();
				path = dir + newfile + ".json";
			}

			using (var sw = new StreamWriter(File.Open(path, FileMode.Append)))
			{
				if (create)
					sw.Write(state.PrintCreateGame());

				else if (board)
					sw.Write(state.PrintBoard());

				else if (move)
					sw.Write(state.PrintAction(true));

				if (state.Game.State == State.COMPLETE)
					sw.WriteLine("}");
			}
		}

		/// <summary>
		/// Prints this games overall info
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static string PrintCreateGame(this HearthNode state)
		{
			var str = new StringBuilder();
			str.AppendLine("{");
			str.AppendLine("\t\"game\": {");
			str.AppendLine($"\t\t\"ts\": \"{DateTime.Now.ToString("yyyy - MM - ddTHH:mm: ss.ffffffK")}\",");
			str.AppendLine("\t\t\"setup\": {");
			str.Append(state.Game.Player1.Hero.FullPrint(true));
			str.Append(state.Game.Player2.Hero.FullPrint(true));
			str.AppendLine("\t},");
			return str.ToString();
		}

		/// <summary>
		/// Prints the current game board
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static string PrintBoard(this HearthNode state, string mode = "")
        {
			var str = new StringBuilder();

			if (mode == "json")
			{
				str.AppendLine("\t\t\"play\": {");
				str.AppendLine($"\t\t\t\"turn\": \"{state.Game.Turn}\",");
				str.Append(state.Game.PrintJson(false));
				str.AppendLine("\t\t\t}");
				str.AppendLine("\t\t},");
				return str.ToString();
			}

			else
			{
				str.AppendLine("------------");
				str.AppendLine($"| Turn {state.Game.Turn} |");
				str.AppendLine("------------");
				str.AppendLine("-----------------------------------------------------------------------------------------------------");
				str.AppendLine(state.Game.FullPrint() + "-----------------------------------------------------------------------------------------------------");
				str.AppendLine($"{(state.Game.CurrentPlayer == state.Game.Player1 ? $"{state.Game.Player1.Hero.Card.Name}" : $"{state.Game.Player2.Hero.Card.Name}")} is thinking...");
			}
			
			return str.ToString();
		}

		//public static string PrintBoard(this HearthNode state, XmlWriter x)
		//{
		//	var str = new StringBuilder();

		//	x.WriteTurn(state.Game);
			
		//	str.AppendLine("------------");
		//	str.AppendLine($"| Turn {state.Game.Turn} |");
		//	str.AppendLine("------------");
		//	str.AppendLine("-----------------------------------------------------------------------------------------------------");
		//	str.AppendLine(state.Game.FullPrint() + "-----------------------------------------------------------------------------------------------------");
		//	str.AppendLine($"{(state.Game.CurrentPlayer == state.Game.Player1 ? $"{state.Game.Player1.Hero.Card.Name}" : $"{state.Game.Player2.Hero.Card.Name}")} is thinking...");

		//	return str.ToString();
		//}

		/// <summary>
		/// Sorts this PowerHistory to match the HSReplay xml structure
		/// </summary>
		/// <param name="powerHistory"></param>
		public static void HSReplaySort(this PowerHistory powerHistory)
		{
			var first = new PowerHistory();
			var second = new PowerHistory();

			foreach (IPowerHistoryEntry e in powerHistory.Full)
			{
				if (e.PowerType == PowerType.CREATE_GAME)
					first.Add(e);

				else if (e.PowerType == PowerType.FULL_ENTITY)
				{
					var fe = (PowerHistoryFullEntity)e;
					if (fe.Entity.Tags[GameTag.CONTROLLER] == 1)
						first.Add(e);

					else if (fe.Entity.Tags[GameTag.CONTROLLER] == 2)
						second.Add(e);
				}
			}

			powerHistory.Remove(e => first.Full.Contains(e) || second.Full.Contains(e));

			first.AddRange(second);
			second.Clear();
			second.AddRange(powerHistory);

			powerHistory.Clear();
			powerHistory.AddRange(first);
			powerHistory.AddRange(second);
		}

		/// <summary>
		/// Writes this PowerHistory to the specified file
		/// </summary>
		/// <param name="powerHistory"></param>
		/// <param name="fileName"></param>
		/// <param name="root"></param>
		/// <param name="end"></param>
		public static void Dump(this PowerHistory powerHistory, string fileName, bool root = false, bool end = false)
		{
			var str = new StringBuilder();
			var b = new BinaryWriter(File.Open(fileName, mode: root ? FileMode.Create : FileMode.Append), Encoding.UTF8);

			powerHistory.Full.ForEach(e => str.Append(e.Print(true)));
			if (end)
			{
				str.AppendLine("  </Game>");
				str.AppendLine("</HSReplay>");
			}
			b.Write(str.ToString());
			b.Close();
		}
	}

	/// <summary>
	/// Extends Game.FullPrint() for Json fromatting
	/// </summary>
	internal static class JsonPrinter
	{
		static string oneTab = "\t";
		static string twoTabs = oneTab + oneTab;
		static string threeTabs = twoTabs + oneTab;
		static string fourTabs = threeTabs + oneTab;
		static string fiveTabs = fourTabs + oneTab;
		static string sixTabs = fiveTabs + oneTab;
		static string sevenTabs = sixTabs + oneTab;
		static string eightTabs = sevenTabs + oneTab;

		/// <summary>
		/// Prints the game board in a json format
		/// </summary>
		/// <param name="game"></param>
		/// <param name="json"></param>
		/// <returns></returns>
		internal static string PrintJson(this Game game, bool create = false)
		{
			var str = new StringBuilder();
			str.Append(game.Player1.Hero.FullPrint(create));
			str.Append(game.Player2.Hero.FullPrint(create));
			return str.ToString();
		}

		internal static string FullPrint(this Hero h, bool create = false)
		{
			var str = new StringBuilder();

			str.AppendLine($"{threeTabs}\"player{h.Controller.PlayerId}\": {{");
			str.AppendLine($"{fourTabs}\"hero\": {{");
			str.AppendLine($"{fiveTabs}\"class\": \"{h.Controller.HeroClass.ToString().ToLower()}\",");
			str.AppendLine($"{fiveTabs}\"name\": \"{h.Card.Name}\",");
			str.AppendLine($"{fiveTabs}\"power\": \"{h.HeroPower.Card.Name}\",");

			if (h.Controller.Deck.Contains("Genn Greymane") && h.HeroPower.Card.Cost < 2)
				str.AppendLine($"{fiveTabs}\"greymane\": \"true\",");

			if (h.HeroPower.Card.Name.Contains("132_AT"))
				str.AppendLine($"{fiveTabs}\"baku\": \"true\",");

			if (create)
				str.Append(h.Controller.Deck.FullPrint());

			else
			{
				str.AppendLine($"{fiveTabs}\"power-usable\": \"{h.HeroPower.IsPlayableByPlayer.ToString().ToLower()}\",");
				str.AppendLine($"{fiveTabs}\"remaining-mana\" {h.Controller.RemainingMana},");
				str.AppendLine($"{fiveTabs}\"full-mana\": {Controller.MaxResources},");
				str.AppendLine($"{fiveTabs}\"hp\": {h.Health},");
				str.AppendLine($"{fiveTabs}\"armor\": {h.Armor},");

				if (h.Weapon != null)
					str.Append(h.Weapon.FullPrint());

				if (h.AttackDamage > 0)
					str.AppendLine($"{fiveTabs}\"atk\": {h.AttackDamage},");

				if (h.Controller.CurrentSpellPower > 0)
					str.AppendLine($"{fiveTabs}\"spell-dmg\": {h.Controller.CurrentSpellPower},");

				str.Append(h.Controller.HandZone.FullPrint(hand: true));
				str.Append(h.Controller.BoardZone.FullPrint(true));
				str.Append(h.Controller.DeckZone.FullPrint(deck: true));
			}

			str.AppendLine($"{fourTabs}}}");
			str.AppendLine($"{threeTabs}}},");
			return str.ToString();
		}

		static string FullPrint(this Zone<IPlayable> z, bool hand = false, bool deck = false)
		{
			var str = new StringBuilder();

			if (hand)
				str.AppendLine($"{fiveTabs}\"hand\": {{");

			else if (deck)
				str.AppendLine($"{fiveTabs}\"deck\": {{");

			str.AppendLine($"{sixTabs}\"count\": {z.Count},");

			int pos = 0;
			foreach (IPlayable p in z)
			{
				str.AppendLine($"{sevenTabs}\"{pos}\": {{");
				str.AppendLine($"{eightTabs}\"name\": \"{p.Card.Name}\",");
				str.AppendLine($"{eightTabs}\"cost\": \"{p.Card.Cost}\",");
				
				if (p is Minion)
				{
					var m = p as Minion;
					str.AppendLine($"{eightTabs}\"atk\": \"{m.AttackDamage}\",");
					str.AppendLine($"{eightTabs}\"health\": \"{m.Health}\"");
				}

				else if (p is Weapon)
				{
					var w = p as Weapon;
					str.AppendLine($"{eightTabs}\"atk\": \"{w.AttackDamage}\",");
					str.AppendLine($"{eightTabs}\"dur\": \"{w.Durability}\"");
				}
				str.AppendLine($"{sevenTabs}}},");
				++pos;
			}
			str.AppendLine($"{fiveTabs}}},");
			return str.ToString();
		}

		static string FullPrint(this BoardZone b, bool json)
		{
			var str = new StringBuilder();
			str.AppendLine($"{fiveTabs}\"in-play\": {{");
			str.AppendLine($"{sixTabs}\"count\": {b.Count},");

			int pos = 0;
			foreach (Minion m in b)
			{
				str.AppendLine($"{sixTabs}\"{pos}\": {{");
				str.AppendLine($"{sevenTabs}\"name\": \"{m.Card.Name}\",");
				str.AppendLine($"{sevenTabs}\"atk\": \"{m.AttackDamage}\",");
				str.AppendLine($"{sevenTabs}\"health\": \"{m.Health}\"");

				if (m.HasCharge)
					str.AppendLine($"{sevenTabs}\"charge\": \"true\",");

				if (m.HasDivineShield)
					str.AppendLine($"{sevenTabs}\"divine-shield\": \"true\",");

				if (m.HasInspire)
					str.AppendLine($"{sevenTabs}\"inspire\": \"true\",");

				if (m.HasLifeSteal)
					str.AppendLine($"{sevenTabs}\"lifesteal\": \"true\",");

				if (m.Poisonous)
					str.AppendLine($"{sevenTabs}\"poisonous\": \"true\",");

				if (m.IsRush)
					str.AppendLine($"{sevenTabs}\"rush\": \"true\",");

				if (m.HasStealth)
					str.AppendLine($"{sevenTabs}\"stealth\": \"true\",");

				if (m.HasTaunt)
					str.AppendLine($"{sevenTabs}\"taunt\": \"true\",");

				if (m.HasWindfury)
					str.AppendLine($"{sevenTabs}\"windfury\": \"true\",");

				str.AppendLine($"{sixTabs}}},");
				++pos;
			}
			str.AppendLine($"{fiveTabs}}},");
			return str.ToString();
		}

		static string FullPrint(this Weapon w)
		{
			var str = new StringBuilder();

			bool hasExtra = w.HasLifeSteal || w.IsWindfury || w.Poisonous;

			str.AppendLine($"{fiveTabs}\"weapon\": {{");
			str.AppendLine($"{sixTabs}\"name\": \"{w.Card.Name}\",");
			str.AppendLine($"{sixTabs}\"atk\": {w.AttackDamage},");
			str.AppendLine($"{sixTabs}\"dur\": {w.Durability}{(hasExtra ? "," : "")}");

			if (hasExtra)
			{
				int extras = 0;
				if (w.HasLifeSteal)
				{
					extras += w.IsWindfury ? 1 : 0;
					str.AppendLine($"{sixTabs}\"lifesteal\": \"true\"{(extras > 0 ? "," : "")}");
				}

				if (w.Poisonous)
					str.AppendLine($"{sixTabs}\"poisonous\": \"true\"{(extras == 1 ? "," : "")}");

				if (w.IsWindfury)
					str.AppendLine($"{sixTabs}\"windfury\": \"true\"");
			}

			str.AppendLine($"{fiveTabs}}},");
			return str.ToString();
		}

		static string FullPrint(this Deck d)
		{
			var str = new StringBuilder();
			str.AppendLine($"{fiveTabs}\"deck\": {{");

			str.AppendLine($"{sixTabs}\"deck-class\": \"{d.HeroClass.ToString()}\",");
			str.AppendLine($"{sixTabs}\"archetype\": \"{d.Archetype.ToString()}\",");
			str.AppendLine($"{sixTabs}\"deck-name\": \"{d.DeckName}\",");
			str.AppendLine($"{sixTabs}\"num-games\": {d.NumGames},");
			str.AppendLine($"{sixTabs}\"cards\": {{");

			int pos = 0;
			foreach (Card c in d)
			{
				str.AppendLine($"{sevenTabs}\"{pos}\": {{");
				str.AppendLine($"{eightTabs}\"name\": \"{c.Name}\",");
				str.AppendLine($"{eightTabs}\"id\": \"{c.Id}\",");
				str.AppendLine($"{eightTabs}\"cost\": {c.Cost},");

				if (c.Type == CardType.MINION)
				{
					str.AppendLine($"{eightTabs}\"atk\": {c[GameTag.ATK]},");
					str.AppendLine($"{eightTabs}\"health\": {c[GameTag.HEALTH]}");
				}

				else if (c.Type ==  CardType.WEAPON)
				{
					str.AppendLine($"{eightTabs}\"atk\": {c[GameTag.ATK]},");
					str.AppendLine($"{eightTabs}\"dur\": {c[GameTag.DURABILITY]}");
				}
				//str.AppendLine($"{sevenTabs}}}]");
				str.AppendLine($"{sevenTabs}}},");
				++pos;
			}
			str.AppendLine($"{sixTabs}}}");
			str.AppendLine($"{fiveTabs}}}");
			return str.ToString();
		}
	}
}
