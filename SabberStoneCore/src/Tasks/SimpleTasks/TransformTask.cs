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
using SabberStoneCore.Actions;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class TransformTask : SimpleTask
	{
		public TransformTask(Card card, EntityType type)
		{
			Card = card;
			Type = type;
		}

		public TransformTask(string cardId, EntityType type)
		{
			Card = Cards.FromId(cardId);
			Type = type;
		}

		public Card Card { get; set; }
		public EntityType Type { get; set; }

		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}Card.AssetId", Card.AssetId },
			{ $"{Prefix()}Type", (int)Type }
		};
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			foreach (IPlayable p in IncludeTask.GetEntities(Type, in controller, source, target, stack?.Playables))
			{
				int before = p.Card.AssetId;
				Vector().Add($"{Prefix()}Process.BeforeTransform.AssetId", before);
				Generic.TransformBlock.Invoke(p.Controller, Card, p as Minion);

				//if (before != p.Card.AssetId)
				Vector().Add($"{Prefix()}Process.AfterTransform.AssetId", p.Card.AssetId);
			}

			return TaskState.COMPLETE;
		}
	}
}
