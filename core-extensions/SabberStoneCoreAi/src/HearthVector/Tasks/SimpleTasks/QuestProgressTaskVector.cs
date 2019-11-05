using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class QuestProgressTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }

		public QuestProgressTaskVector(QuestProgressTask questProgressTask) : base(questProgressTask)
		{
			AssetId = questProgressTask.AssetId;
		}
	}
}
