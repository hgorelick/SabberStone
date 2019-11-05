using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SummonCopyTaskVector : SimpleTaskVector
	{
		public bool AddToStack { get; private set; }
		public bool RandomFlag { get; private set; }
		public int SummonSide { get; private set; }
		public int Type { get; private set; }

		public SummonCopyTaskVector(SummonCopyTask summonCopyTask) : base(summonCopyTask)
		{
			AddToStack = summonCopyTask.AddToStack;
			RandomFlag = summonCopyTask.RandomFlag;
			SummonSide = (int)summonCopyTask.SummonSide;
			Type = (int)summonCopyTask.Type;
		}
	}
}
