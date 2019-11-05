using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ReturnToHandTaskVector : SimpleTaskVector
	{
		public int ReturnToZone { get; private set; } = (int)Zone.HAND;
		public int Type { get; private set; }

		public ReturnToHandTaskVector(ReturnToHandTask returnToHandTask) : base(returnToHandTask)
		{
			Type = (int)returnToHandTask.Type;
		}
	}
}
