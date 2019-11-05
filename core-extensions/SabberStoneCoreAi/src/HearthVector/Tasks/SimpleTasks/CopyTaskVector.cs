using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class CopyTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool ToOpponent { get; private set; }
		public int Type { get; private set; }
		public int Zone { get; private set; }

		public CopyTaskVector(CopyTask copyTask) : base(copyTask)
		{
			Amount = copyTask.Amount;
			ToOpponent = copyTask.ToOpponent;
			Type = (int)copyTask.Type;
			Zone = (int)copyTask.Zone;
		}
	}
}
