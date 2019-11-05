using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class HealTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Type { get; private set; }

		public HealTaskVector(HealTask healTask) : base(healTask)
		{
			Amount = healTask.Amount;
			Type = (int)healTask.Type;
		}
	}
}
