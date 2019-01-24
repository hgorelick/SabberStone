using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.Stats
{
	public class GameStats
	{
		public int NumGames { get; private set; } = 0;
		public int TotalTurns { get; private set; } = 0;

		private int[] wins = new[] { 0, 0 };
		private double[] time_per_player = new[] { 0D, 0D };
		private int[] exception_count = new[] { 0, 0 };
		private Dictionary<int, string> exceptions = new Dictionary<int, string>();

		public PlayerStats Hero1;
		public PlayerStats Hero2;

		public int PlayerA_Wins => wins[0];
		public int PlayerB_Wins => wins[1];

		public int PlayerA_Exceptions => exception_count[0];
		public int PlayerB_Exceptions => exception_count[1];

		//public double PlayerA_Time => time_per_player[0];
		//public double PlayerB_Time => time_per_player[1];

		//Todo add getter for each private variable

		public GameStats()
		{
		}

		public GameStats(string h1, string h2)
		{
			Hero1 = new PlayerStats(h1, NumGames);
			Hero2 = new PlayerStats(h2, NumGames);
		}

		public void addGame(Game game, Stopwatch[] playerWatches)
		{
			TotalTurns += game.Turn;

			Hero1.SaveResults(TotalTurns, game.Player1.Hero.Health, game.Player1.Hero.Armor,
				playerWatches[0].Elapsed.TotalMilliseconds);

			Hero2.SaveResults(TotalTurns, game.Player2.Hero.Health, game.Player2.Hero.Armor,
				playerWatches[1].Elapsed.TotalMilliseconds);

			if (game.Player1.PlayState == PlayState.WON)
				Hero1.Wins++;
			if (game.Player2.PlayState == PlayState.WON)
				Hero2.Wins++;

			time_per_player[0] += playerWatches[0].Elapsed.TotalMilliseconds;
			time_per_player[1] += playerWatches[1].Elapsed.TotalMilliseconds;
		}

		public void registerException(Game game, Exception e)
		{
			if (game.Player1.PlayState == PlayState.CONCEDED)
			{
				exception_count[0] += 1;
			}
			else if (game.Player2.PlayState == PlayState.CONCEDED)
			{
				exception_count[1] += 1;
			}
			exceptions.Add(NumGames, e.Message);
		}

		public void printResults()
		{
			if (NumGames > 0)
			{
				Console.WriteLine($"{NumGames} games with {TotalTurns} turns took {(time_per_player[0] + time_per_player[1]).ToString("F4")} ms => " +
							  $"Avg. {((time_per_player[0] + time_per_player[1]) / NumGames).ToString("F4")} per game " +
							  $"and {((time_per_player[0] + time_per_player[1]) / (NumGames * TotalTurns)).ToString("F8")} per turn!");
				Console.WriteLine($"IceFireWarrior {wins[0] * 100 / NumGames}% vs. playerB {wins[1] * 100 / NumGames}%!");
				if (exceptions.Count > 0)
				{
					Console.WriteLine($"Games lost due to exceptions: playerA - {exception_count[0]}; playerB - {exception_count[1]}");
					Console.WriteLine("Exception messages:");
					foreach (KeyValuePair<int, string> e in exceptions)
					{
						Console.WriteLine($"\tGame {e.Key}: {e.Value}");
					}
					Console.WriteLine();
				}
			}
			else
			{
				Console.WriteLine("No games played yet. Use Gamehandler.PlayGame() to add games.");
			}

		}

		public void AddCard(int playerId)
		{
			if (!Hero1.Stats.ContainsKey(NumGames))
			{
				Hero1.Stats.Add(NumGames, new Dictionary<string, dynamic>());
				Hero2.Stats.Add(NumGames, new Dictionary<string, dynamic>());
			}

			if (!Hero1.Stats[NumGames].ContainsKey("NumCardsPlayed"))
			{
				Hero1.Stats[NumGames].Add("NumCardsPlayed", 0);
				Hero2.Stats[NumGames].Add("NumCardsPlayed", 0);
			}

			if (playerId == 1)
				Hero1.Stats[NumGames]["NumCardsPlayed"] += 1;

			else
				Hero2.Stats[NumGames]["NumCardsPlayed"] += 1;
		}

		public void FinalizeResults(string filename)
		{
			Hero1.FinalizeResults();
			Hero2.FinalizeResults();

			Write(filename);
		}

		public void Write(string filename)
		{
			string newfile = filename;
			string dir = @"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\";
			string path = dir + newfile + ".stats";
			int count = 1;

			while (File.Exists(path))// && File.GetLastWriteTime(path) + TimeSpan.FromMinutes(10) < DateTime.Now)
			{
				newfile += count++.ToString();
				path = dir + newfile + ".stats";
			}

			using (var sw = new StreamWriter(File.Open(path, FileMode.Append)))
			{
				var serializer = new XmlSerializer(typeof(PlayerStats));
				serializer.Serialize(sw, Hero1);
				serializer.Serialize(sw, Hero2);
			}
		}
	}
}
