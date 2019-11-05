using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DamageTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool SpellDmg { get; private set; }
		public int Type { get; private set; }

		public DamageTaskVector(DamageTask damageTask) : base(damageTask)
		{
			Amount = damageTask.Amount;
			SpellDmg = damageTask.SpellDmg;
			Type = (int)damageTask.Type;
		}
	}
}
