﻿#region copyright
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
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class SetGameTagNumberTask : SimpleTask
	{
		public SetGameTagNumberTask(GameTag tag, EntityType entityType, bool ignoreDamage = false)
		{
			Tag = tag;
			Type = entityType;
			IgnoreDamage = ignoreDamage;
		}

		public GameTag Tag { get; set; }

		public EntityType Type { get; set; }

		public bool IgnoreDamage { get; set; }

		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}GameTag", (int)Tag },
			{ $"{Prefix()}Type", (int)Type}
		};
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			foreach (IPlayable p in IncludeTask.GetEntities(Type, in controller, source, target, stack?.Playables))
			{
				if (p is Character c)
					switch (Tag)
					{
						case GameTag.ATK:
							c.AttackDamage = stack.Number;
							break;
						case GameTag.HEALTH:
							c.BaseHealth = stack.Number;
							break;
						case GameTag.DAMAGE:
							c.Damage = stack.Number;
							break;
						case GameTag.EXTRA_ATTACKS_THIS_TURN:
							if (c is Hero h)
								h.ExtraAttacksThisTurn = stack.Number;
							break;
						default:
							c[Tag] = stack.Number;
							break;
					}
				else
					p[Tag] = stack.Number;

				Vector().Add($"{Prefix()}Process.GameTagAppliedTo.AssetId", p.Card.AssetId);
			}

			Vector().Add($"{Prefix()}Process.ValueAppliedToGameTag", stack.Number);
			return TaskState.COMPLETE;
		}
	}
}
