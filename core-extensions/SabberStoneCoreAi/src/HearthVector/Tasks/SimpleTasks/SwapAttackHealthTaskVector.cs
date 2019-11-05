using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SwapAttackHealthTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public int Type { get; private set; }

		public SwapAttackHealthTaskVector(SwapAttackHealthTask swapAttackHealthTask)
			: base(swapAttackHealthTask)
		{
			AssetId = swapAttackHealthTask.AssetId;
			Type = (int)swapAttackHealthTask.Type;
		}
	}
}
