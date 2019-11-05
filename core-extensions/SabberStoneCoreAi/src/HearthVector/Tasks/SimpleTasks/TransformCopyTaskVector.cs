using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class TransformCopyTaskVector : SimpleTaskVector
	{
		public bool AddToStack { get; private set; }
		public int Tag { get; private set; } = (int)GameTag.CREATOR;

		public TransformCopyTaskVector(TransformCopyTask transformCopyTask) : base(transformCopyTask)
		{
			AddToStack = transformCopyTask.AddToStack;
		}
	}
}
