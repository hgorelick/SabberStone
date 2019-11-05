using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks;
using SabberStoneCore.Tasks.SimpleTasks;
using System.Collections.Generic;
using System;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public class SpellVector : PlayableVector
	{
		//public int AttackBuff { get; private set; }
		//public int HealthBuff { get; private set; }
		public bool IsAffectedBySpellpower { get; private set; } = false;
		public bool IsCountered { get; private set; } = false;
		public bool IsQuest { get; private set; } = false;
		public bool IsSecret { get; private set; } = false;
		public bool IsTwinSpell { get; private set; } = false;
		public int QuestTotalProgress { get; private set; } = 0;
		public bool ReceveivesDoubleSpellDamage { get; private set; } = false;

		public SpellVector(Spell s) : base(s)
		{
			IsAffectedBySpellpower = s.IsAffectedBySpellpower;
			IsCountered = s.IsCountered;
			IsQuest = s.IsQuest;
			IsSecret = s.IsSecret;
			IsTwinSpell = s.IsTwinSpell;
			QuestTotalProgress = s.QuestTotalProgress;
			ReceveivesDoubleSpellDamage = s.ReceveivesDoubleSpellDamage;
		}

		public SpellVector() : base() { }

		protected override int GetDamage(Playable p)
		{
			int damage = base.GetDamage(p);

			if (ReceveivesDoubleSpellDamage)
				damage *= 2;

			return damage;
		}
	}
}
