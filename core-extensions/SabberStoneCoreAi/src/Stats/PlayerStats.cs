using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.Stats
{
	using stats = Dictionary<string, dynamic>;

	[Serializable]
	public class PlayerStats
	{
		public string Name { get; set; }
		public int Wins { get; set; } = 0;
		public int GameNumber { get; set; }
		public Dictionary<int, stats> Stats = new Dictionary<int, stats>();
		public KeyValuePair<string, List<dynamic>> FullStats;

		public PlayerStats(string heroName, int gameNumber)
		{
			Name = heroName;
			GameNumber = gameNumber;
			FullStats = new KeyValuePair<string, List<dynamic>>(Name, new List<dynamic>());
		}

		internal void SaveResults(int turns, int health, int armor, double gameTime)
		{
			Stats[GameNumber].Add("NumTurns", turns);
			Stats[GameNumber].Add("FinalScore", new Tuple<int, int>(health, armor));
			Stats[GameNumber].Add("GameTime", gameTime);
			GameNumber++;
		}

		internal void FinalizeResults()
		{
			FullStats.Value.Add(new KeyValuePair<string, int>("Wins", Wins));
			FullStats.Value.Add(Stats);
		}
	}
}
