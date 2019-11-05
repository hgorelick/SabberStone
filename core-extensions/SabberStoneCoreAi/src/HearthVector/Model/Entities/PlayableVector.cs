using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;
using SabberStoneCore.Tasks;
using System;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public class PlayableVector : CardVector
	{
		public int ArmorAmount { get; protected set; } = 0;
		public int DamageAmount { get; protected set; } = 0;
		public int DiscardAmount { get; protected set; } = 0;
		public int DrawAmount { get; protected set; } = 0;
		public int HealAmount { get; protected set; } = 0;
		public bool Random { get; private set; } = false;

		public PlayableVector(Playable p) : base(p)
		{
			ArmorAmount = GetArmor(p);
			DamageAmount = GetDamage(p);
			DrawAmount = GetDraw(p);
			Random = GetRandom(p);
		}

		public PlayableVector() : base() { }

		protected virtual int GetArmor(Playable p)
		{
			if (p.Power.PowerTask is ArmorTask)
				return ValueGetter.GetArmor.GetAmount(p.Power.PowerTask);

			if (p.Power.PowerTask is StateTaskList)
				return ValueGetter.GetArmor.GetAmount<ArmorTask>(p.Power.PowerTask);

			if (p.Power.Trigger.SingleTask is ArmorTask)
				return ValueGetter.GetArmor.GetAmount(p.Power.Trigger.SingleTask);

			if (p.Power.Trigger.SingleTask is StateTaskList)
				return ValueGetter.GetArmor.GetAmount<ArmorTask>(p.Power.Trigger.SingleTask);

			return 0;
		}

		protected virtual int GetDamage(Playable p)
		{
			int damage = 0;

			if (p.Power.PowerTask is DamageTask)
				damage = ValueGetter.GetDamage.GetAmount(p.Power.PowerTask);

			else if (p.Power.PowerTask is StateTaskList)
				damage = ValueGetter.GetDamage.GetAmount<DamageTask>(p.Power.PowerTask);

			else if (p.Power.Trigger.SingleTask is DamageTask)
				damage = ValueGetter.GetDamage.GetAmount(p.Power.Trigger.SingleTask);

			else if (p.Power.Trigger.SingleTask is StateTaskList)
				damage = ValueGetter.GetDamage.GetAmount<DamageTask>(p.Power.Trigger.SingleTask);

			return damage;
		}

		protected virtual int GetDiscard(Playable p)
		{
			int discard = 0;
			bool controllerNeeded = ValueGetter.ControllerNeeded<DiscardTask>(p.Power.PowerTask) ||
									ValueGetter.ControllerNeeded<DiscardTask>(p.Power.Trigger.SingleTask);

			if (p.Power.PowerTask is StateTaskList)
				discard = ValueGetter.GetDiscard.GetAmount<DiscardTask>(p.Power.PowerTask, controllerNeeded ? p.Controller : null);

			else if (p.Power.Trigger.SingleTask is StateTaskList)
				discard = ValueGetter.GetDiscard.GetAmount<DiscardTask>(p.Power.Trigger.SingleTask, controllerNeeded ? p.Controller : null);

			return discard;
		}

		protected virtual int GetDraw(Playable p)
		{
			if (p.Power.PowerTask is DrawTask)
				return ValueGetter.GetDraw.GetAmount(p.Power.PowerTask);

			else if (p.Power.PowerTask is StateTaskList)
				return ValueGetter.GetDraw.GetAmount<DrawTask>(p.Power.PowerTask);

			else if (p.Power.Trigger.SingleTask is DrawTask)
				return ValueGetter.GetDraw.GetAmount(p.Power.Trigger.SingleTask);

			else if (p.Power.Trigger.SingleTask is StateTaskList)
				return ValueGetter.GetDraw.GetAmount<DrawTask>(p.Power.Trigger.SingleTask);

			return 0;
		}

		protected virtual int GetHeal(Playable p)
		{
			if (p.Power.PowerTask is HealTask)
				return ValueGetter.GetHeal.GetAmount(p.Power.PowerTask);

			else if (p.Power.PowerTask is StateTaskList)
				return ValueGetter.GetHeal.GetAmount<HealTask>(p.Power.PowerTask);

			else if (p.Power.Trigger.SingleTask is HealTask)
				return ValueGetter.GetHeal.GetAmount(p.Power.Trigger.SingleTask);

			else if (p.Power.Trigger.SingleTask is StateTaskList)
				return ValueGetter.GetHeal.GetAmount<HealTask>(p.Power.Trigger.SingleTask);

			return 0;
		}

		protected virtual bool GetRandom(Playable p)
		{
			if (p.Power.PowerTask is HealTask)
				return Convert.ToBoolean(ValueGetter.GetRandom.GetAmount(p.Power.PowerTask));

			else if (p.Power.PowerTask is StateTaskList)
				return Convert.ToBoolean(ValueGetter.GetRandom.GetAmount<HealTask>(p.Power.PowerTask));

			else if (p.Power.Trigger.SingleTask is HealTask)
				return Convert.ToBoolean(ValueGetter.GetRandom.GetAmount(p.Power.Trigger.SingleTask));

			else if (p.Power.Trigger.SingleTask is StateTaskList)
				return Convert.ToBoolean(ValueGetter.GetRandom.GetAmount<HealTask>(p.Power.Trigger.SingleTask));

			return false;
		}
	}

	public class HeroPowerVector : PlayableVector
	{
		public bool IsPassiveHeroPower { get; private set; } = false;

		public HeroPowerVector(HeroPower hp) : base(hp)
		{
			IsPassiveHeroPower = hp.IsPassiveHeroPower;
		}

		public HeroPowerVector() : base() { }
	}
}
