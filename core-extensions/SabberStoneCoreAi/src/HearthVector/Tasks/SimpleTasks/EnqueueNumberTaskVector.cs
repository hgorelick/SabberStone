using SabberStoneCore.Tasks.SimpleTasks;
using System;
using Type = System.Type;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class EnqueueNumberTaskVector : SimpleTaskVector
	{
		public bool SpellDmg { get; private set; }
		public SimpleTaskVector[] TaskVector { get; private set; }

		public EnqueueNumberTaskVector(EnqueueNumberTask enqueueNumberTask) : base(enqueueNumberTask)
		{
			SpellDmg = enqueueNumberTask.SpellDmg;
			TaskVector = Create(enqueueNumberTask.Task);
		}
	}
}
