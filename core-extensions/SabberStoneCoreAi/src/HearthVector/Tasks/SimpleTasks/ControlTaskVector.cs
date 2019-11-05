using SabberStoneCore.Tasks.SimpleTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ControlTaskVector : SimpleTaskVector
	{
		public bool Opposite { get; private set; }
		public int Type { get; private set; }

		public ControlTaskVector(ControlTask controlTask) : base(controlTask)
		{
			Opposite = controlTask.Opposite;
			Type = (int)controlTask.Type;
		}
	}
}
