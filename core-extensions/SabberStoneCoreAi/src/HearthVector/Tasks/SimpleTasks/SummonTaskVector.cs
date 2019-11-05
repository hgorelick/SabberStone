using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SummonTaskVector : SimpleTaskVector
	{
		public bool AddToStack { get; private set; }
		public int Amount { get; private set; }
		public int AssetId { get; private set; }
		public bool RemoveFromStack { get; private set; }
		public int SummonSide { get; private set; }

		public SummonTaskVector(SummonTask summonTask) : base(summonTask)
		{
			AddToStack = summonTask.AddToStack;
			Amount = summonTask.Amount;
			AssetId = summonTask.Card.AssetId;
			RemoveFromStack = summonTask.RemoveFromStack;
			SummonSide = (int)summonTask.Side;
		}
	}

	public class SummonNumberTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public bool Op { get; private set; }

		public SummonNumberTaskVector(SummonNumberTask summonNumberTask) : base(summonNumberTask)
		{
			AssetId = summonNumberTask.AssetId;
			Op = summonNumberTask.Op;
		}
	}
}
