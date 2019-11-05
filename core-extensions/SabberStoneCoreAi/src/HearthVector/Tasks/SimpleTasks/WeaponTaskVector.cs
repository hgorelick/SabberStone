using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class WeaponTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public bool Op { get; private set; }

		public WeaponTaskVector(WeaponTask weaponTask) : base(weaponTask)
		{
			AssetId = weaponTask.AssetId;
			Op = weaponTask.Op;
		}
	}
}
