using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ReplaceWeaponTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public int Rarity { get; private set; }
		public int Type { get; private set; }

		public ReplaceWeaponTaskVector(ReplaceWeaponTask replaceWeaponTask) : base(replaceWeaponTask)
		{
			AssetId = replaceWeaponTask.AssetId;
		}
	}
}
