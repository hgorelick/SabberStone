using SabberStoneCore.Tasks.SimpleTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ChangeEntityTaskVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }
		public int Type { get; private set; }

		public ChangeEntityTaskVector(ChangeEntityTask changeEntityTask) : base(changeEntityTask)
		{
			AssetId = changeEntityTask.AssetId;
			Type = (int)changeEntityTask.Type;
		}
	}
}
