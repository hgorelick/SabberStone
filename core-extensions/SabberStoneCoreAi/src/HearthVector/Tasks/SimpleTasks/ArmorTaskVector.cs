using SabberStoneCore.Tasks.SimpleTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ArmorTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool Op { get; private set; }
		public bool UseNumber { get; private set; }

		public ArmorTaskVector(ArmorTask armorTask) : base(armorTask)
		{
			Amount = armorTask.Amount;
			Op = armorTask.Op;
			UseNumber = armorTask.UseNumber;
		}
	}
}
