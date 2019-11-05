using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RevealTaskVector : SimpleTaskVector
	{
		public SimpleTaskVector[] FailedJoustTask { get; private set; }
		public SimpleTaskVector[] SuccessJoustTask { get; private set; }
		public int CardType { get; private set; }

		public RevealTaskVector(RevealTask revealTask) : base(revealTask)
		{
			FailedJoustTask = Create(revealTask.FailedJoustTask);
			SuccessJoustTask = Create(revealTask.SuccessJoustTask);
			CardType = (int)revealTask.CardType;
		}
	}
}
