using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class PlayTaskVector : SimpleTaskVector
	{
		public int PlayType { get; private set; }
		public bool RandTarget { get; private set; }
		public int Type { get; private set; }

		public PlayTaskVector(PlayTask playTask) : base(playTask)
		{
			PlayType = (int)playTask.PlayType;
			RandTarget = playTask.RandTarget;
			Type = (int)playTask.Type;
		}
	}
}
