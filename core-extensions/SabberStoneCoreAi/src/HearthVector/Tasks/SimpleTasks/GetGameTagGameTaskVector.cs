using SabberStoneCore.Model;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class GetGameTagGameTaskVector : SimpleTaskVector
	{
		public int TagValue { get; private set; }

		public GetGameTagGameTaskVector(GetGameTagGameTask gameTagGameTask, Game g) : base(gameTagGameTask)
		{
			TagValue = g[gameTagGameTask.Tag];
		}
	}
}
