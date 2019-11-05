using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class TransformMinionTaskVector : SimpleTaskVector
	{
		public int CostChange { get; private set; }
		public int Type { get; private set; }

		public TransformMinionTaskVector(TransformMinionTask transformMinionTask) : base(transformMinionTask)
		{
			CostChange = transformMinionTask.CostChange;
			Type = (int)transformMinionTask.Type;
		}
	}
}
