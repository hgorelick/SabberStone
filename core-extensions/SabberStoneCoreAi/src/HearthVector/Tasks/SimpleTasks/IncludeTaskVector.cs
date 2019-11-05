using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class IncludeTaskVector : SimpleTaskVector
	{
		public bool AddFlag { get; private set; }
		public int Type { get; private set; }

		public IncludeTaskVector(IncludeTask includeTask) : base(includeTask)
		{
			AddFlag = includeTask.AddFlag;
			Type = (int)includeTask.IncludeType;
		}
	}

	public class IncludeAdjacentTaskVector : SimpleTaskVector
	{
		public bool IncludeCenter { get; private set; }
		public int Type { get; private set; }

		public IncludeAdjacentTaskVector(IncludeAdjacentTask includeAdjacentTask) : base(includeAdjacentTask)
		{
			IncludeCenter = includeAdjacentTask.IncludeCenter;
			Type = (int)includeAdjacentTask.Type;
		}
	}
}
