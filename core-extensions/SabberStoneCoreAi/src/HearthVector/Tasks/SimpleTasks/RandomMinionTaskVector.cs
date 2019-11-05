using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RandomMinionTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool ClassAndMultiOnlyFlag { get; private set; }
		public bool MaxInDeckFlag { get; private set; }
		public bool Opponent { get; private set; }
		public int RelaSign { get; private set; }
		public int TagValue { get; private set; }
		public int Type { get; private set; }
		public int Value { get; private set; }

		public RandomMinionTaskVector(RandomMinionTask randomMinionTask) : base(randomMinionTask)
		{
			Amount = randomMinionTask.Amount;
			ClassAndMultiOnlyFlag = randomMinionTask.ClassAndMultiOnlyFlag;
			MaxInDeckFlag = randomMinionTask.MaxInDeckFlag;
			Opponent = randomMinionTask.Opponent;
			RelaSign = (int)randomMinionTask.RelaSign;
			//TagValue = ?
			Type = (int)randomMinionTask.Type;
			Value = randomMinionTask.Value;
		}
	}
}
