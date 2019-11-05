using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class TempManaTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Tag { get; private set; } = (int)GameTag.TEMP_RESOURCES;

		public TempManaTaskVector(TempManaTask tempManaTask) : base(tempManaTask)
		{
			Amount = tempManaTask.Amount;
		}
	}
}
