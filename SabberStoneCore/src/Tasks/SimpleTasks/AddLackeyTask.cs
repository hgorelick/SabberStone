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
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class AddLackeyTask : SimpleTask
	{
		public static readonly Card[]
			Lackeys = Cards.All.Where(card => card[GameTag.MARK_OF_EVIL] == 1).ToArray();

		public override OrderedDictionary Vector()
		{
			var v = new OrderedDictionary { { $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) } };
			v.Add($"{Prefix()}Amount", Amount);
			//for (int i = 0; i < Entities.Length; ++i)
			//	v.Add($"{Prefix()}{Entities[i].Card.Name}.AssetId", Entities[i].Card.AssetId);
			return v;
		}

		private readonly int _amount;
		public int Amount => _amount;
		private readonly IPlayable[] _entities;
		public IPlayable[] Entities => _entities;
		public AddLackeyTask(int amount)
		{
			_amount = amount;
			_entities = new IPlayable[_amount];
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			game.OnRandomHappened(true);

			for (int i = 0; i < _amount && !controller.HandZone.IsFull; i++)
				_entities[i] = Entity.FromCard(in controller, Lackeys.Choose(game.Random), zone: controller.HandZone);

			return TaskState.COMPLETE;
		}
	}
}
