using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DrawTaskVector : SimpleTaskVector
	{
		public int Count { get; private set; }

		public DrawTaskVector(DrawTask drawTask) : base(drawTask)
		{
			Count = drawTask.Count;
		}
	}
}
