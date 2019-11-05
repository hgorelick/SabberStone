using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RandomTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Type { get; private set; }

		public RandomTaskVector(RandomTask randomTask) : base(randomTask)
		{
			Amount = randomTask.Amount;
			Type = (int)randomTask.Type;
		}
	}

	public class SplitTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Type { get; private set; }

		public SplitTaskVector(SplitTask splitTask) : base(splitTask)
		{
			Amount = splitTask.Amount;
			Type = (int)splitTask.Type;
		}
	}
}
