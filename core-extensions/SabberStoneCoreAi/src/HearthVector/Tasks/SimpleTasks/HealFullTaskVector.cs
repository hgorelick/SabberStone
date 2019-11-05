using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class HealFullTaskVector : SimpleTaskVector
	{
		public int Type { get; private set; }

		public HealFullTaskVector(HealFullTask healFullTask) : base(healFullTask)
		{
			Type = (int)healFullTask.Type;
		}

		public HealFullTaskVector(HealNumberTask healNumberTask) : base(healNumberTask)
		{
			Type = (int)healNumberTask.Type;
		}
	}

	// HealNumberTask same as above...
}
