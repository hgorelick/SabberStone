using System.Linq;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;
using SabberStoneCore.Tasks;
using System.Reflection;
using System;
using SabberStoneCoreAi.HearthVector.Model.Entities;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCoreAi.HearthVector.Enchants;
using SabberStoneCoreAi.HearthVector.Triggers;
using SabberStoneCoreAi.HearthVector.Auras;

namespace SabberStoneCoreAi.HearthVector.Model
{
	public class CardVector : EntityVector
	{
		public TriggerVector ActivatedTrigger { get; private set; }
		public bool ChooseOne { get; private set; }
		public bool Combo { get; private set; }
		public int Cost { get; private set; }
		public bool Discover { get; private set; }
		public bool Echo { get; private set; }
		public AuraVector OngoingEffect { get; protected set; }
		public bool HasOverload { get; private set; }
		public bool IsPlayable { get; private set; }
		public bool LifeSteal { get; private set; }
		public bool Overkill { get; private set; }
		public int Overload { get; private set; }
		public PowerVector Power { get; private set; }
		public int Zone { get; private set; }
		public int ZonePosition { get; private set; }

		public static CardVector Create(IPlayable p)
		{
			if (p == null)
				return new CardVector();

			switch (p.Card.Type)
			{
				case CardType.HERO:
					return new HeroVector(p as Hero);
				case CardType.HERO_POWER:
					return new HeroPowerVector(p as HeroPower);
				case CardType.MINION:
					return new MinionVector(p as Minion);
				case CardType.SPELL:
					return new SpellVector(p as Spell);
				case CardType.WEAPON:
					return new WeaponVector(p as Weapon);
				default:
					return new CardVector(p);
			}
		}

		protected CardVector(IPlayable p) : base(p)
		{
			ActivatedTrigger = TriggerVector.Create(p.ActivatedTrigger);
			ChooseOne = p.ChooseOne;
			Combo = p.Combo;
			Cost = p.Cost;
			Discover = p.Power.PowerTask is DiscoverTask ?
				true : p.Power.PowerTask is StateTaskList ?
				(p.Power.PowerTask as StateTaskList).TaskList.Any(t => t is DiscoverTask) : false;
			Echo = p.IsEcho;
			HasOverload = p.Overload > 0;
			OngoingEffect = AuraVector.Create(p.OngoingEffect);
			IsPlayable = p.IsPlayable;
			LifeSteal = p.HasLifeSteal;
			Overkill = p.HasOverkill;
			Overload = p.Overload;
			Power = PowerVector.Create(p.Power);
			Zone = (int)p.Zone.Type;
			ZonePosition = p.ZonePosition;

			SetVector();
		}

		public CardVector() : base()
		{
			Cost = 0;
			ChooseOne = false;
			Combo = false;
			Discover = false;
			HasOverload = false;
			OngoingEffect = new AuraVector();
			IsPlayable = false;
			LifeSteal = false;
			Overload = 0;
			Overkill = false;
			Zone = 0;
			ZonePosition = -1;

			SetVector();
		}

		private void SetVector()
		{
			foreach (PropertyInfo pi in GetProperties())
			{
				object value = pi.GetValue(this);
				if (value is bool)
					Vector.Add(Convert.ToInt32(value));
				else
					Vector.Add((int)value);
			}
		}
	}
}
