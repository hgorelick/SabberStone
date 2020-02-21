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
using System.Collections.Specialized;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class FlagTask : SimpleTask
	{
		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}CheckFlag", Convert.ToInt32(CheckFlag) }
		};
		}

		private readonly bool _checkFlag;
		public bool CheckFlag => _checkFlag;
		private readonly ISimpleTask _taskToDo;
		public ISimpleTask TaskToDo => _taskToDo;

		public FlagTask(bool checkFlag, ISimpleTask taskToDo)
		{
			_checkFlag = checkFlag;
			_taskToDo = taskToDo;
		}


		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			if (stack.Flag != _checkFlag) return TaskState.COMPLETE;

			//if (_taskToDo is StateTaskList list)
			//	list.Stack = game.TaskStack;
			//else
			//{
			//	_taskToDo.Game = Game;
			//	_taskToDo.Controller = Controller;
			//	_taskToDo.source = source;
			//	_taskToDo.Target = target;
			//	_taskToDo.Flag = Flag;
			//}

			TaskState result = _taskToDo.Process(in game, in controller, in source, in target, in stack);
			Vector().AddRange(_taskToDo.Vector(), $"{Prefix()}Process.");
			return result;
		}

		public override string ToString()
		{
			return $"[FlagTask][{_checkFlag}:{_taskToDo.GetType().Name}]";
		}
	}
}
