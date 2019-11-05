using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class GetGameTagControllerTaskVector : SimpleTaskVector
	{
		public int TagValue { get; private set; }

		public GetGameTagControllerTaskVector(GetGameTagControllerTask tagControllerTask, Controller c) : base(tagControllerTask)
		{
			TagValue = c[tagControllerTask.Tag];
		}
	}
}
