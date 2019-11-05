using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Tasks
{
	public class SimpleTaskVector : HearthVector
	{
		public bool IsNull { get; set; } = false;
		public bool IsTrigger { get; set; } = false;

		protected SimpleTaskVector(ISimpleTask simpleTask)
		{
			IsTrigger = simpleTask.IsTrigger;
		}

		protected SimpleTaskVector()
		{
			IsNull = true;
		}

		public static SimpleTaskVector[] Create(ISimpleTask simpleTask = null)
		{
			if (simpleTask == null)
				return new SimpleTaskVector[] { new SimpleTaskVector() };

			if (simpleTask is StateTaskList stateTaskList)
				return Create(stateTaskList);

			var type = Type.GetType(simpleTask.GetType().ToString() + "Vector");
			return new SimpleTaskVector[] { (SimpleTaskVector)Activator.CreateInstance(type, simpleTask) }; 
		}

		private static SimpleTaskVector[] Create(StateTaskList stateTaskList)
		{
			if (stateTaskList == null)
				return null;

			var simpleTaskVectors = new SimpleTaskVector[stateTaskList.TaskList.Length];
			for (int i = 0; i < stateTaskList.TaskList.Length; ++i)
			{
				SimpleTaskVector[] simpleTasks = Create(stateTaskList.TaskList[i]);
				if (simpleTasks.Length == 1)
					simpleTaskVectors[i] = simpleTasks[0];
				else
					simpleTaskVectors.AddRange(simpleTasks);
			}
			return simpleTaskVectors;
		}
	}
}
