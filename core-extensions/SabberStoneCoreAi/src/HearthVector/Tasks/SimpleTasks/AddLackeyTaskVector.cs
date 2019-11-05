using SabberStoneCore.Tasks.SimpleTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class AddLackeyTaskVector
	{
		public int Amount { get; private set; }
		public int AssetId { get; private set; }

		public AddLackeyTaskVector(AddLackeyTask addLackeyTask)
		{
			Amount = addLackeyTask.Amount;
		}
	}
}
