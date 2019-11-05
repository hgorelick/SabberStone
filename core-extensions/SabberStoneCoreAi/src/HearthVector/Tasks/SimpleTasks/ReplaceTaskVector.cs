using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ReplaceTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public int Rarity { get; private set; }
		public int Type { get; private set; }

		public ReplaceTaskVector(ReplaceTask replaceTask) : base(replaceTask)
		{
			AssetId = replaceTask.Card.AssetId;
			Rarity = (int)replaceTask.Rarity;
			Type = (int)replaceTask.Type;
		}
	}
}
