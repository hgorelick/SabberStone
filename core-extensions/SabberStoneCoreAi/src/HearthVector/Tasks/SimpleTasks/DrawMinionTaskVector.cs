using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class DrawMinionTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool LowestCost { get; private set; }

		public DrawMinionTaskVector(DrawMinionTask drawMinionTask) : base(drawMinionTask)
		{
			Amount = drawMinionTask.Amount;
			LowestCost = drawMinionTask.LowestCost;
		}
	}
}
