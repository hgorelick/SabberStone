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
using SabberStoneCore.Actions;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using System;
using System.Collections.Specialized;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class DrawOpTask : SimpleTask
	{
		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}Card.AssetId", Card.AssetId },
			{ $"{Prefix()}ToStack", Convert.ToInt32(ToStack) }
		};
		}

		public DrawOpTask(Card card = null, bool toStack = false)
		{
			Card = card;
			ToStack = toStack;
		}

		public Card Card { get; set; }

		public bool ToStack { get; set; }

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			IPlayable cardDrawn = Card != null
				? Generic.DrawCardBlock.Invoke(controller.Opponent, Card)
				: Generic.Draw(controller.Opponent);

			if (ToStack && cardDrawn != null)
			{
				Vector().Add($"{Prefix()}Process.cardDrawn.AssetId", cardDrawn.Card.AssetId);
				stack?.Playables.Add(cardDrawn);
			}

			return TaskState.COMPLETE;
		}
	}
}
