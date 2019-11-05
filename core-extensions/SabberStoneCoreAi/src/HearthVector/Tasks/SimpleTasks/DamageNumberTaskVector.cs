using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DamageNumberTaskVector : SimpleTaskVector
	{
		public bool SpellDmg { get; private set; }
		public int Type { get; private set; }

		public DamageNumberTaskVector(DamageNumberTask damageNumberTask) : base(damageNumberTask)
		{
			SpellDmg = damageNumberTask.SpellDmg;
			Type = (int)damageNumberTask.Type;
		}
	}
}
