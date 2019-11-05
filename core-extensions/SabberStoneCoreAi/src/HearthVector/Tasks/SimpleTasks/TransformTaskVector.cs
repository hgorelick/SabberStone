using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class TransformTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public int Type { get; private set; }

		public TransformTaskVector(TransformTask transformTask) : base(transformTask)
		{
			AssetId = transformTask.Card.AssetId; ;
			Type = (int)transformTask.Type;
		}
	}
}
