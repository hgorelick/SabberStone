using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SummonStackTaskVector : SimpleTaskVector
	{
		public bool RemoveFromStack { get; private set; }
		public bool RemoveFromZone { get; private set; }

		public SummonStackTaskVector(SummonStackTask summonStackTask) : base(summonStackTask)
		{
			RemoveFromStack = summonStackTask.RemoveFromStack;
			RemoveFromZone = summonStackTask.RemoveFromZone;
		}
	}
}
