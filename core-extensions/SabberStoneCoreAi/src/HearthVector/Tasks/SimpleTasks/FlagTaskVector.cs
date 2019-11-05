using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class FlagTaskVector : SimpleTaskVector
	{
		public bool CheckFlag { get; private set; }
		public SimpleTaskVector[] TaskVector { get; private set; }

		public FlagTaskVector(FlagTask flagTask) : base(flagTask)
		{
			CheckFlag = flagTask.CheckFlag;
			TaskVector = Create(flagTask.TaskToDo);
		}
	}
}
