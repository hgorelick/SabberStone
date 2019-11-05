using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SetGameTagNumberTaskVector : SimpleTaskVector
	{
		public int TagValue { get; private set; }
		public int Type { get; private set; }

		public SetGameTagNumberTaskVector(SetGameTagNumberTask setGameTagNumberTask, Controller c)
			: base(setGameTagNumberTask)
		{
			TagValue = c[setGameTagNumberTask.Tag];
			Type = (int)setGameTagNumberTask.Type;
		}
	}
}
