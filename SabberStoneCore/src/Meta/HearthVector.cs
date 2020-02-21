using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace SabberStoneCore.HearthVector
{
	public interface IHearthVector
	{
		OrderedDictionary Vector();
		string Prefix();

	}

	//public abstract class HearthVector : IHearthVector
	//{
	//	public bool IsNull { get; set; } = false;
	//	public OrderedDictionary Vector { get; set; } = new OrderedDictionary();
	//	public string Prefix { get; } = "HearthVector.";

	//	public HearthVector()
	//	{
	//		IsNull = true;
	//	}

	//	public PropertyInfo[] GetProperties()
	//	{
	//		return GetType().GetProperties();
	//	}

	//	public List<string> GetPropertyNames()
	//	{
	//		var propNames = new List<string>();
	//		PropertyInfo[] props = GetType().GetProperties();

	//		foreach (PropertyInfo p in props)
	//			propNames.Add(p.Name);

	//		return propNames;
	//	}

	//	//public virtual bool SetSpecialProperty(Entity e, string propName)
	//	//{
	//	//	throw new NotImplementedException();
	//	//}
	//}

	public static class Utils
	{
		public static void AddRange(this IHearthVector hv, OrderedDictionary src)
		{
			hv.Vector().AddRange(src, hv.Prefix());
		}

		public static OrderedDictionary AddRange(this OrderedDictionary dest, OrderedDictionary src, string destPref)
		{
			foreach (DictionaryEntry kv in src)
			{
				try { dest.Add(destPref + kv.Key, kv.Value); }
				catch
				{
					int i = 1;
					string kvKey = kv.Key.ToString();
					if (dest.Contains(destPref + kvKey))
					{
						string[] kvKeyParts = kvKey.ToString().Split('.');
						kvKeyParts[0] = kvKeyParts[0] + i.ToString();
						kvKey = String.Join(".", kvKeyParts);

						while (dest.Contains(destPref + kvKey))
						{
							kvKeyParts = kvKey.ToString().Split('.');
							kvKeyParts[0] = kvKeyParts[0].Remove(kvKeyParts[0].Length - 1, 1) + i.ToString();
							kvKey = String.Join(".", kvKeyParts);
							i++;
						}
					}
					dest.Add(destPref + kvKey, kv.Value);
				}
			}
			return dest;
		}

		public static bool Contains(this OrderedDictionary src, Predicate<string> p)
		{
			foreach (DictionaryEntry kv in src)
				if (p(kv.Key.ToString()))
					return true;
			return false;
		}

		public static void WriteCSV(this Game game, string gameNumber, CardClass[] heroClasses, string filename,
			string dir = @"C:\Users\hgore\OneDrive - Fordham University\Documents\Fordham\Thesis\SabberStone\SabberStatsListener\HearthVectors\",
			int winner = 0)
		{
			dir += $"{Enum.GetName(typeof(CardClass), heroClasses[0])}_vs_{Enum.GetName(typeof(CardClass), heroClasses[1])}\\Tyche\\";

			string path = $"{dir}Game_{gameNumber}";//\\Turn{turn.ToString()}";
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			path += $"\\{filename}";

			if (winner > 0)
				WriteWinner(winner, path);

			else if (!File.Exists(path))
			{
				using var csv = new StreamWriter(File.Open(path, FileMode.CreateNew));
				string headers = "Turn,CurrentPlayer,BoardCountAdvantage,BoardManaAdvantage," +
					"DeckManaEfficiency,DrawAdvantage,HandManaEfficiency,HandSizeAdvantage," +
					"HealthAdvantage,ManaAdvantage,Winner";
				csv.WriteLine(headers);
			}

			else
			{
				using var csv = new StreamWriter(File.Open(path, FileMode.Append));
				var str = new StringBuilder();
				//csv.Write($"{game.Turn},{game.CurrentPlayer.PlayerId}");
				foreach (double val in game.CurrentPlayer.Vector)
					str.Append($"{val.ToString()},");
				csv.WriteLine(str.ToString());
			}
		}

		private static void WriteWinner(int winner, string path)
        {
			var file = new StreamReader(File.OpenRead(path));
			var lines = new List<string>();
			string line;
			int lineNumber = 0;
			while ((line = file.ReadLine()) != null)
			{
				if (lineNumber > 0)
					line += $"{winner.ToString()}";

				lineNumber++;
				lines.Add(line);
			}

			file.Close();

			var csv = new StreamWriter(File.Open(path, FileMode.Create));
			foreach (string l in lines)
				csv.WriteLine(l);
			csv.Close();
		}
	}
}
