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
using System.Collections.Generic;
using System.Collections.Specialized;
using SabberStoneCore.Actions;
using SabberStoneCore.Enums;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class CopyTask : SimpleTask
	{
		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}Amount", Amount },
			{ $"{Prefix()}ToOpponent", Convert.ToInt32(ToOpponent) },
			{ $"{Prefix()}Type", (int)Type },
			{ $"{Prefix()}Zone", (int)Zone }
		};
		}

		private readonly EntityType _entityType;
		public EntityType Type => _entityType;
		private readonly Zone _zoneType;
		public Zone Zone => _zoneType;
		private readonly int _amount;
		public int Amount => _amount;
		private readonly bool _addToStack;
		private readonly bool _toOpponent;
		public bool ToOpponent => _toOpponent;

		public CopyTask(EntityType type, Zone targetZone, int amount = 1, bool addToStack = false, bool toOpponent = false)
		{
			_entityType = type;
			_amount = amount;
			_zoneType = targetZone;
			_addToStack = addToStack;
			_toOpponent = toOpponent;
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			Zone zone = _zoneType;
			bool addToStack = _addToStack;
			int amount = _amount;

			Controller c = _toOpponent ? controller.Opponent : controller;
			IZone targetZone = c.ControlledZones[zone];

			if (targetZone.IsFull)
				return TaskState.STOP;

			List<IPlayable> result = addToStack ? new List<IPlayable>() : null;

			if (_entityType == EntityType.STACK)
			{
				if (stack?.Playables.Count < 1)
					return TaskState.STOP;

				foreach (IPlayable p in stack.Playables)
				{
					for (int i = 0; i < amount; i++)
					{
						IPlayable copied = Generic.Copy(in c, in source, in p, zone);
						Vector().Add($"{Prefix()}Process.copied.AssetId", copied.Card.AssetId);
						if (addToStack)
							result.Add(copied);

						if (targetZone.IsFull)
						{
							if (addToStack)
								stack.Playables = result;
							return TaskState.COMPLETE;
						}
					}
				}
			}
			else
			{
				IPlayable toBeCopied;
				bool deathrattle = false;
				switch (_entityType)
				{
					case EntityType.TARGET:
						toBeCopied = target;
						break;
					case EntityType.SOURCE:
						toBeCopied = source as IPlayable;
						deathrattle = _zoneType == Zone.PLAY && target is Enchantment e && e.Power?.DeathrattleTask != null;
						break;
					case EntityType.EVENT_SOURCE:
						toBeCopied = game.CurrentEventData?.EventSource;
						break;
					case EntityType.OP_HERO_POWER:
						toBeCopied = Entity.FromCard(c,
							Cards.FromId(controller.Opponent.Hero.HeroPower.Card.Id));
						if (addToStack)
						{
							result.Add(toBeCopied);
							stack.Playables = result;
						}
						return TaskState.COMPLETE;
					case EntityType.WEAPON:
						Weapon weapon = controller.Hero.Weapon;
						if (weapon == null) return TaskState.STOP;

						for (int i = 0; i < _amount; i++)
							result.Add(Entity.FromCard(c, Cards.FromId(weapon.Card.Id)));
						return TaskState.COMPLETE;
					default:
						throw new NotImplementedException();
				}

				if (toBeCopied == null)
					return TaskState.STOP;

				for (int i = 0; i < amount; i++)
				{
					IPlayable copied = Generic.Copy(in c, in source, in toBeCopied, zone, deathrattle);
					Vector().Add($"{Prefix()}Process.copied.AssetId", copied.Card.AssetId);
					if (addToStack)
						result.Add(copied);

					if (targetZone.IsFull)
					{
						if (addToStack)
							stack.Playables = result;
						return TaskState.COMPLETE;
					}
				}
			}

			if (addToStack)
				stack.Playables = result;

			return TaskState.COMPLETE;
		}
	}
}
