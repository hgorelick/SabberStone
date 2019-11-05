using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SilenceTaskVector : SimpleTaskVector
	{
		public int Tag { get; private set; } = (int)GameTag.SILENCE;
		public int Type { get; private set; }

		public SilenceTaskVector(SilenceTask silenceTask) : base(silenceTask)
		{
			Type = (int)silenceTask.Type;
		}
	}
}
