#region copyright
// SabberStone, Hearthstone Simulator in C# .NET Core
// Copyright (C) 2017-2019 SabberStone Team, darkfriend77 & rnilva
//
// SabberStone is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License.
// SabberStone is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks
{
	public interface ISimpleTask : IHearthVector
	{
		TaskState State { get; set; }

		TaskState Process(in Game game, in Controller controller, in IEntity source, in IPlayable target,
			in TaskStack stack = null);

		bool IsTrigger { get; set; }
	}

	public abstract class SimpleTask : ISimpleTask
	{
		public string Prefix()
		{
			return $"{GetType().Name}.";
		}

		public virtual OrderedDictionary Vector()
		{
			return new OrderedDictionary { { $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) } };
		}

		public static OrderedDictionary NullVector = new OrderedDictionary { { "NullTask.IsTrigger", 0 } };

		public TaskState State { get; set; } = TaskState.READY;
		//public IEntity Source;
		//public IPlayable Target;
		//public TaskStack Stack;

		public abstract TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target, in TaskStack stack = null);

		public void ResetState()
		{
			State = TaskState.READY;
		}

		public bool IsTrigger { get; set; }

		public override string ToString()
		{
			return GetType().Name;
		}

		public void AddSourceAndTargetToVector(IEntity source, IPlayable target)
		{
			Vector().Add($"{Prefix()}Process.source.AssetId", source?.Card.AssetId ?? 0);
			Vector().Add($"{Prefix()}Process.target.AssetId", target?.Card.AssetId ?? 0);
		}

		public void AddStackToVector(TaskStack stack)
		{
			for (int i = 0; i < stack?.Playables.Count; ++i)
				Vector().Add($"{Prefix()}Process.stack.Playables{i}.AssetId", stack.Playables[i].Card.AssetId);
		}

		public static OrderedDictionary GetVector(ISimpleTask simpleTask)
		{
			var v = new OrderedDictionary();
			if (simpleTask is StateTaskList stateTaskList)
				for (int i = 0; i < stateTaskList.TaskList.Length; ++i)
					v.AddRange(stateTaskList.TaskList[i].Vector(), stateTaskList.TaskList[i].Prefix());
			else
				v.AddRange(simpleTask.Vector(), simpleTask.Prefix());

			return v;
		}

		public static OrderedDictionary GetNullVector(string prefix = "NullTask.")
		{
			return new OrderedDictionary { { $"{prefix}IsTrigger", 0 } };
		}
	}
}
