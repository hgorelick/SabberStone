using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DiscardTaskVector : SimpleTaskVector
	{
		public int Type { get; private set; }

		public DiscardTaskVector(DiscardTask discardTask) : base(discardTask)
		{
			Type = (int)discardTask.Type;
		}
	}
}
