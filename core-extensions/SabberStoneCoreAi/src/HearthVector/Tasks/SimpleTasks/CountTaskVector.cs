using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class CountTaskVector : SimpleTaskVector
	{
		public bool Opponent { get; private set; }
		public int Zone { get; private set; }

		public CountTaskVector(CountTask countTask) : base(countTask)
		{
			Opponent = countTask.Opponent;
			Zone = (int)countTask.Zone;
		}
	}
}
