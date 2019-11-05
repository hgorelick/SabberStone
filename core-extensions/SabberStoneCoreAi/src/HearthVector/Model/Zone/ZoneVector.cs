using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;
using SabberStoneCoreAi.HearthVector.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Model.Zone
{
	public class ZoneVector : HearthVector
	{
		public int Count { get; private set; }
		public CardVector[] Entities { get; protected set; }
		public int FreeSpace { get; private set; }
		public bool IsEmpty { get; private set; }
		public bool IsFull { get; private set; }
		public int Type { get; private set; }

		public ZoneVector(IZone zone)
		{
			var z = zone as Zone<IPlayable>;

			Count = z.Count;
			FreeSpace = z.FreeSpace;
			IsEmpty = z.IsEmpty;
			IsFull = z.IsFull;
			Type = (int)z.Type;
		}
	}

	public class HandZoneVector : ZoneVector
	{
		public HandZoneVector(HandZone hz) : base(hz)
		{
			Entities = new CardVector[hz.MaxSize];

			for (int i = 0; i < hz.MaxSize; ++i)
			{
				if (hz[i] == null)
					Entities[i] = new CardVector();

				else if (hz[i] is Spell s)
					Entities[i] = new SpellVector(s);

				else if (hz[i] is Minion m)
					Entities[i] = new MinionVector(m);

				else if (hz[i] is Weapon w)
					Entities[i] = new WeaponVector(w);

				else if (hz[i] is Hero h)
					Entities[i] = new HeroVector(h);
			}
		}
	}

	public class BoardZoneVector : ZoneVector
	{
		public bool HasUntouchables { get; private set; }
		public int UntouchablesCount { get; private set; }

		public BoardZoneVector(BoardZone bz) : base(bz)
		{
			Entities = new MinionVector[bz.MaxSize];
			HasUntouchables = bz.HasUntouchables;
			UntouchablesCount = Count - bz.CountExceptUntouchables;

			for (int i = 0; i < bz.MaxSize; ++i)
			{
				if (bz[i] == null)
					Entities[i] = new MinionVector();

				else if (bz[i] is Minion m)
					Entities[i] = new MinionVector(m);				
			}
		}
	}

	public class DeckZoneVector : ZoneVector
	{
		public bool NoEvenCostCards { get; private set; }
		public bool NoOddCostCards { get; private set; }
		public CardVector TopCard { get; private set; }

		public DeckZoneVector(DeckZone dz) : base(dz)
		{
			Entities = new CardVector[dz.MaxSize];

			for (int i = 0; i < dz.MaxSize; ++i)
			{
				if (dz[i] == null)
					Entities[i] = new CardVector();

				else if (dz[i] is Spell s)
					Entities[i] = new SpellVector(s);

				else if (dz[i] is Minion m)
					Entities[i] = new MinionVector(m);

				else if (dz[i] is Weapon w)
					Entities[i] = new WeaponVector(w);

				else if (dz[i] is Hero h)
					Entities[i] = new HeroVector(h);
			}

			TopCard = Entities[0];
		}
	}

	public class GraveyardZoneVector : ZoneVector
	{
		public GraveyardZoneVector(GraveyardZone gz) : base(gz)
		{
			Entities = new CardVector[gz.Count];

			for (int i = 0; i < gz.Count; ++i)
			{
				if (gz[i] == null)
					Entities[i] = new CardVector();

				else if (gz[i] is Spell s)
					Entities[i] = new SpellVector(s);

				else if (gz[i] is Minion m)
					Entities[i] = new MinionVector(m);

				else if (gz[i] is Weapon w)
					Entities[i] = new WeaponVector(w);

				else if (gz[i] is Hero h)
					Entities[i] = new HeroVector(h);
			}
		}
	}

	public class SecretZoneVector : ZoneVector
	{
		public SpellVector Quest { get; private set; }

		public SecretZoneVector(SecretZone sz) : base(sz)
		{
			Entities = new SpellVector[SecretZone.SecretZoneMaxSize];

			Quest = sz.Quest != null ? new SpellVector(sz.Quest) : new SpellVector();

			for (int i = 0; i < SecretZone.SecretZoneMaxSize; ++i)
			{
				if (sz[i] == null)
					Entities[i] = new SpellVector();

				else if (sz[i] is Spell s)
					Entities[i] = new SpellVector(s);
			}
		}
	}
}
