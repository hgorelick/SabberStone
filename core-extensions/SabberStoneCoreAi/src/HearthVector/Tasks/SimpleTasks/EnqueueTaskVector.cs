using SabberStoneCore.Auras;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class EnqueueTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool SpellDmg { get; private set; }
		public SimpleTaskVector[] TaskVector { get; private set; }

		public EnqueueTaskVector(EnqueueTask enqueueTask) : base(enqueueTask)
		{
			Amount = enqueueTask.Amount;
			SpellDmg = enqueueTask.SpellDmg;
			TaskVector = Create(enqueueTask.Task);
		}
	}

	public class EnqueuePendingTaskVector : SimpleTaskVector
	{
		public int Tag { get; private set; } = (int)AuraType.ADJACENT;
		public int Type { get; private set; }

		public EnqueuePendingTaskVector(EnqueuePendingTask enqueuePendingTask) : base(enqueuePendingTask)
		{
			Type = (int)enqueuePendingTask.TargetType;
		}
	}

	public class OverloadTaskVector : SimpleTaskVector
	{
		public int Tag { get; private set; } = (int)GameTag.OVERLOAD;

		public OverloadTaskVector(OverloadTask overloadTask) : base(overloadTask) { }
		public OverloadTaskVector() : base() { }
	}
}
