using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DrawOpTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public bool ToStack { get; private set; }

		public DrawOpTaskVector(DrawOpTask drawOpTask) : base(drawOpTask)
		{
			AssetId = drawOpTask.Card.AssetId;
			ToStack = drawOpTask.ToStack;
		}
	}
}
