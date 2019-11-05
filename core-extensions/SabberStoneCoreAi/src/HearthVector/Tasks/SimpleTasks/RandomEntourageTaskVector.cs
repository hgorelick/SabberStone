using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RandomEntourageTaskVector : SimpleTaskVector
	{
		public int Count { get; private set; }
		public bool Opponent { get; private set; }

		public RandomEntourageTaskVector(RandomEntourageTask randomEntourageTask) : base(randomEntourageTask)
		{
			Count = randomEntourageTask.Count;
			Opponent = randomEntourageTask.Opponent;
		}
	}
}
