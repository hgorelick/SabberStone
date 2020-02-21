using System;
using System.Collections.Generic;
using System.Linq;


using SabberStoneCore.Model;
using SabberStoneCore.Model.Zones;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.Enums;
using SabberStoneCore.Kettle;
using SabberStoneCoreAi.HearthNodes;

namespace SabberStoneCoreAi.Utils
{
	public static class CollectionExtensions
	{ 
		public static bool Contains(this Zone<IPlayable> zone, Func<Card, bool> p)
		{
			for (int i = 0; i < zone.Count; ++i)
			{
				if (p(zone[i].Card))
					return true;
			}
			return false;
		}

		public static bool Contains(this Zone<IPlayable> zone, Card c)
		{
			for (int i = 0; i < zone.Count; ++i)
			{
				if (zone[i].Card.Name == c.Name)
					return true;
			}
			return false;
		}

		public static bool Contains(this SecretZone s, Func<Spell, bool> p)
		{
			for (int i = 0; i < s.Count; ++i)
			{
				if (p(s[i]))
					return true;
			}
			return false;
		}

		public static bool Contains(this SecretZone s, Card c)
		{
			for (int i = 0; i < s.Count; ++i)
			{
				if (s[i].Card.Name == c.Name)
					return true;
			}
			return false;
		}

		public static bool Contains(this Zone<Minion> zone, Func<Card, bool> p)
		{
			for (int i = 0; i < zone.Count; ++i)
			{
				if (p(zone[i].Card))
					return true;
			}
			return false;
		}

		public static bool Contains(this List<HearthNode> children, Func<HearthNode, bool> p)
		{
			for (int i = 0; i < children.Count; ++i)
			{
				if (p(children[i]))
					return true;
			}
			return false;
		}

		public static bool Contains(this IDictionary<GameTag, int> pairs, Func<KeyValuePair<GameTag, int>, bool> p)
		{
			for (int i = 0; i < pairs.Count; ++i)
			{
				if (p(pairs.ElementAt(i)))
					return true;
			}
			return false;
		}

		public static bool Contains(this Deck d, Func<Card, bool> p)
		{
			for (int i = 0; i < d.Count; ++i)
			{
				if (p(d[i]))
					return true;
			}
			return false;
		}

		public static bool Contains(this Deck d, string card)
		{
			for (int i = 0; i < d.Count; ++i)
			{
				if (d[i].Name == card)
					return true;
			}
			return false;
		}

		public static bool Contains<T>(this T[] arr, Func<T, bool> func)
		{
			for (int i = 0; i < arr.Length; ++i)
				if (func(arr[i]))
					return true;
			return false;
		}

		public static int IndexOf(this Zone<IPlayable> zone, IPlayable p)
		{
			for (int i = 0; i < zone.Count; ++i)
				if (p.Card.Name == zone[i].Card.Name)
					return i;

			return -1;
		}

		public static void Insert(this Zone<IPlayable> zone, int ind, IPlayable p)
		{
			var popped = new List<IPlayable>();
			for (int i = zone.Count - 1; i >= ind; --i)
			{
				popped.Add(zone.Remove(zone[i]));
			}
			zone.Add(p);
			zone.AddRange(popped);
		}

		public static int Count(this PowerHistory p)
		{
			int count = 0;
			for (int i = 0; i < p.Full.Count; ++i)
				++count;

			return count;
		}

		public static int Count(this List<IPlayable> l, Spell s)
		{
			int count = 0;
			for (int i = 0; i < l.Count; ++i)
			{
				if (l[i].Card.Name == s.Card.Name)
					++count;
			}
			return count;
		}

		public static void Reverse(this PowerHistory p)
		{
			p.Full.Reverse();
		}

		public static void AddRangeFirst(this PowerHistory dest, PowerHistory src)
		{
			var tmp = new PowerHistory();
			tmp.AddRange(src);

			for (int i = 0; i < dest.Full.Count; ++i)
				tmp.Add(dest.Full[i]);

			dest.Clear();
			dest.AddRange(tmp);
		}

		public static void AddRange(this PowerHistory dest, PowerHistory src)
		{
			for (int i = 0; i < src.Full.Count; ++i)
				dest.Add(src.Full[i]);
		}

		public static void AddRange(this Zone<IPlayable> zone, List<IPlayable> src)
		{
			for (int i = 0; i < src.Count; ++i)
				zone.Add(src[i]);
		}

		public static void AddRange<T>(this T[] dest, T[] src)
		{
			for (int i = 0; i < src.Length; ++i)
				dest[i] = src[i];
		}

		public static void Clear(this PowerHistory p)
		{
			p.Full.Clear();
			//p.Full.Capacity = 0;
			p.Last.Clear();
			//p.Last.Capacity = 0;
		}

		public static HearthNode Pop(this List<HearthNode> children, Func<HearthNode, bool> p)
		{
			for (int i = 0; i < children.Count; ++i)
				if (p(children[i]))
				{
					HearthNode popped = children[i];
					children.Remove(children[i]);
					return popped;
				}
			return null;
		}

		public static IPlayable Pop(this Zone<IPlayable> zone, Func<IPlayable, bool> p)
		{
			for (int i = 0; i < zone.Count; ++i)
			{
				if (p(zone[i]))
				{
					IPlayable popped = zone[i];
					zone.Remove(zone[i]);
					return popped;
				}
			}
			return null;
		}

		public static string PopAt(this List<string> l, int ind)
		{
			string s = l[ind];
			l.Remove(s);
			return s;
		}

		public static IPowerHistoryEntry Pop (this PowerHistory powerHistory, IPowerHistoryEntry e)
		{
			powerHistory.Full.Remove(e);
			powerHistory.Last.Remove(e);
			return e;
		}

		public static bool Remove(this Zone<IPlayable> zone, Func<Card, bool> p)
		{
			int count = zone.Count;
			Zone<IPlayable> zcopy = zone;
			for (int i = 0; i < zcopy.Count; ++i)
				if (p(zcopy[i].Card))
				{
					zone.Remove(zcopy[i]);
				}
			return count > zone.Count;
		}

		public static bool Remove(this Zone<IPlayable> zone, Card c)
		{
			int count = zone.Count;
			Zone<IPlayable> zcopy = zone;
			for (int i = 0; i < zcopy.Count; ++i)
				if (zcopy[i].Card.Name == c.Name)
				{
					zone.Remove(zcopy[i]);
				}
			return count > zone.Count;
		}

		public static bool Remove(this List<HearthNode> children, Func<HearthNode, bool> p)
		{
			int count = children.Count;
			List<HearthNode> chcopy = children;
			for (int i = 0; i < chcopy.Count; ++i)
				if (p(chcopy[i]))
				{
					children.Remove(chcopy[i]);
				}
			return count > children.Count;
		}

		public static bool Remove(this PowerHistory powerHistory, Func<IPowerHistoryEntry, bool> p)
		{
			int count = powerHistory.Full.Count;

			var copy = new PowerHistory();
			powerHistory.Where(e => !p(e)).ForEach(e=> copy.Add(e));

			powerHistory.Clear();
			powerHistory.AddRange(copy);
			return count > powerHistory.Full.Count;
		}

		public static IEntity Find(this Zone<IPlayable> zone, Func<Card, bool> p)
		{
			for (int i = 0; i < zone.Count; ++i)
			{
				if (p(zone[i].Card))
					return zone[i];
			}
			return null;
		}

		public static void ForEach(this PowerHistory h, Action<IPowerHistoryEntry> a)
		{
			for (int i = 0; i < h.Full.Count; ++i)
				a(h.Full[i]);
		}

		public static PowerHistory Where(this PowerHistory ph, Func<IPowerHistoryEntry, bool> p)
		{
			var where = new PowerHistory();
			for (int i = 0; i < ph.Full.Count; ++i)
				if (p(ph.Full[i]))
					where.Add(ph.Full[i]);

			return where;
		}

		public static bool IsEqual(this PlayerTask a, PlayerTask other)
		{
			return a.Game.State == other.Game.State && a.PlayerTaskType == other.PlayerTaskType
				&& a.Controller.PlayerId == other.Controller.PlayerId
				&& a.Source?.Card == other.Source?.Card
				&& a.Target?.Card == other.Target?.Card
				//&& a. == other.ZonePosition
				&& a.ChooseOne == other.ChooseOne
				&& a.SkipPrePhase == other.SkipPrePhase;
		}
	}
}
