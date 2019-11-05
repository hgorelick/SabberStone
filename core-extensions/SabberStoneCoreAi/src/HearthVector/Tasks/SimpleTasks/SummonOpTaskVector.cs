using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SummonOpTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int AssetId { get; private set; }

		public SummonOpTaskVector(SummonOpTask summonOpTask) : base(summonOpTask)
		{
			Amount = summonOpTask.Amount;
			AssetId = summonOpTask.AssetId;
		}
	}
}
