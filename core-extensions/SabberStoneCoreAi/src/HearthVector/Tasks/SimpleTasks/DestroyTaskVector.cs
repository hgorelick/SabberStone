using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DestroyTaskVector : SimpleTaskVector
	{
		public bool ForcedDeathPhase { get; private set; }
		public int Type { get; private set; }

		public DestroyTaskVector(DestroyTask destroyTask) : base(destroyTask)
		{
			ForcedDeathPhase = destroyTask.ForcedDeathPhase;
			Type = (int)destroyTask.Type;
		}
	}
}
