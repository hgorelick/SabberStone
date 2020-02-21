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
using System.Collections.Generic;
using System.Collections.Specialized;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class ActivatePowerTask : SimpleTask
	{
		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}SourceType", (int)_sourceType },
			{ $"{Prefix()}TargetType", (int)_targetType },
			{ $"{Prefix()}EnqueueBase", Convert.ToInt32(_enqueueBase) }
		};
		}

		private readonly EntityType _sourceType;
		private readonly EntityType _targetType;
		private readonly bool _enqueueBase;

		public ActivatePowerTask(EntityType sourceType, EntityType targetType = EntityType.INVALID, bool enqueueBase = false)
		{
			_sourceType = sourceType;
			_enqueueBase = enqueueBase;
			_targetType = targetType;
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source, in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			bool enqueueBase = _enqueueBase;

			if (_targetType != EntityType.INVALID)
			{
				IList<IPlayable> targets = IncludeTask.GetEntities(in _targetType, in controller, source, target,
					stack?.Playables);

				foreach (IPlayable p in IncludeTask.GetEntities(in _sourceType, in controller, source, target,
					stack?.Playables))
				{
					Vector().Add($"{Prefix()}Process.CardWithPowerToProcess.AssetId", p.Card.AssetId);
					ISimpleTask task = p.Card.Power.PowerTask;

					foreach (IPlayable t in targets)
					{
						Vector().Add($"{Prefix()}Process.TargetForTaskQueue.AssetId", t.Card.AssetId);
						if (enqueueBase)
							game.TaskQueue.EnqueueBase(in task, in controller, p, in t);
						else
							game.TaskQueue.Enqueue(in task, in controller, p, in t);
					}
				}
			}
			else
				foreach (IPlayable p in IncludeTask.GetEntities(in _sourceType, in controller, source, target,
					stack?.Playables))
				{
					Vector().Add($"{Prefix()}Process.CardWithPowerToProcess.AssetId", p.Card.AssetId);
					if (enqueueBase)
						game.TaskQueue.EnqueueBase(p.Card.Power.PowerTask, in controller, p, null);
					else
						game.TaskQueue.Enqueue(p.Card.Power.PowerTask, in controller, p, null);
				}

			return TaskState.COMPLETE;
		}
	}
}
