using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DamageWeaponTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool Opponent { get; private set; }

		public DamageWeaponTaskVector(DamageWeaponTask damageWeaponTask) : base(damageWeaponTask)
		{
			Amount = damageWeaponTask.Amount;
			Opponent = damageWeaponTask.Opponent;
		}
	}
}
