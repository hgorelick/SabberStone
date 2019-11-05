using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SetControllerGameTagTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool OpFlag { get; private set; }
		public int TagValue { get; private set; }

		public SetControllerGameTagTaskVector(SetControllerGameTagTask setControllerGameTagTask, Controller c)
			: base(setControllerGameTagTask)
		{
			Amount = setControllerGameTagTask.Amount;
			OpFlag = setControllerGameTagTask.OpFlag;
			TagValue = OpFlag ? c.Opponent[setControllerGameTagTask.Tag] : c[setControllerGameTagTask.Tag];
		}
	}
}
