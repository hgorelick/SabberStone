#region copyright
// SabberStone, Hearthstone Simulator in C# .NET Core
// Copyright (C) 2017-2019 SabberStone Team, darkfriend77 & rnilva
//
// SabberStone is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License.
// SabberStone is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
#endregion

using SabberStoneCore.Auras;
using SabberStoneCore.Enums;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Tasks;
using SabberStoneCore.Tasks.SimpleTasks;
using SabberStoneCore.Triggers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SabberStoneCore.Enchants
{
	public class Power : IHearthVector
	{
		public string Prefix()
		{
			return "Power.";
		}

		public OrderedDictionary Vector()
		{
			var v = new OrderedDictionary();

			if (Aura != null)
				v.AddRange(Aura.Vector(), Prefix());
			//v.AddRange(Aura != null ? Aura.Vector : Auras.Aura.NullVector, Prefix);

			if (Enchant != null)
				v.AddRange(Enchant.Vector(), Prefix());
			//v.AddRange(Enchant != null ? Enchant.Vector : Enchant.NullVector, Prefix);

			if (Trigger != null)
				v.AddRange(Trigger.Vector(), Prefix());
			//v.AddRange(Trigger != null ? Trigger.Vector : Trigger.GetNullVector(), Prefix);

			if (PowerTask != null)
				v.AddRange(PowerTask.Vector(), Prefix());
			//v.AddRange(PowerTask != null ? SimpleTask.GetVector(PowerTask) : SimpleTask.GetNullVector("NullPowerTask."), Prefix);

			if (DeathrattleTask != null)
				v.AddRange(DeathrattleTask.Vector(), Prefix());
			//v.AddRange(DeathrattleTask != null ? SimpleTask.GetVector(DeathrattleTask) : SimpleTask.GetNullVector("NullDeathrattleTask."), Prefix);

			if (ComboTask != null)
				v.AddRange(ComboTask.Vector(), Prefix());
			//v.AddRange(ComboTask != null ? SimpleTask.GetVector(ComboTask) : SimpleTask.GetNullVector("NullComboTask."), Prefix);

			if (TopDeckTask != null)
				v.AddRange(TopDeckTask.Vector(), Prefix());
			//v.AddRange(TopDeckTask != null ? SimpleTask.GetVector(TopDeckTask) : SimpleTask.GetNullVector("NullTopDeckTask."), Prefix);

			if (OverkillTask != null)
				v.AddRange(OverkillTask.Vector(), Prefix());
			//v.AddRange(OverkillTask != null ? SimpleTask.GetVector(OverkillTask) : SimpleTask.GetNullVector("NullOverkillTask."), Prefix);

			return v;
		}

		public static OrderedDictionary NullVector
		{
			get
			{
				string prefix = "NullPower.";
				OrderedDictionary v = new OrderedDictionary().AddRange(Auras.Aura.NullVector, prefix);

				v.AddRange(Enchant.NullVector, prefix);
				v.AddRange(Trigger.GetNullVector(), prefix);
				v.AddRange(SimpleTask.GetNullVector("NullPowerTask."), prefix);
				v.AddRange(SimpleTask.GetNullVector("NullDeathrattleTask."), prefix);
				v.AddRange(SimpleTask.GetNullVector("NullComboTask."), prefix);
				v.AddRange(SimpleTask.GetNullVector("NullTopDeckTask."), prefix);
				v.AddRange(SimpleTask.GetNullVector("NullOverkillTask."), prefix);

				return v;
			}
		}

		public string InfoCardId { get; set; } = null;

		public IAura Aura { get; set; }

		public Enchant Enchant { get; set; }

		public Trigger Trigger { get; set; }

		public ISimpleTask PowerTask { get; set; }

		public ISimpleTask DeathrattleTask { get; set; }

		public ISimpleTask ComboTask { get; set; }

		public ISimpleTask TopDeckTask { get; set; }

		public ISimpleTask OverkillTask { get; set; }

		internal static Power OneTurnStealthEnchantmentPower =>
			new Power
			{
				Enchant = new Enchant(Effects.StealthEff),
				Trigger = new Trigger(TriggerType.TURN_START)
				{
					SingleTask = RemoveEnchantmentTask.Task,
					RemoveAfterTriggered = true,
				}
			};
	}
}
