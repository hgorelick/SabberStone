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
using SabberStoneCore.Actions;
using SabberStoneCore.Conditions;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class RecruitTask : SimpleTask
	{
		private readonly SelfCondition[] _conditions;
		public SelfCondition[] Conditions => _conditions;

		private readonly int _amount;
		public int Amount => _amount;

		private readonly bool _addToStack;
		public bool AddToStack => _addToStack;

		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}Amount", Amount },
			{ $"{Prefix()}AddToStack", Convert.ToInt32(AddToStack) }
		};
		}

		/// <summary>
		/// Recruits a random minion satisfying the given conditions.
		/// </summary>
		public RecruitTask(int amount, params SelfCondition[] conditions)
		{
			_amount = amount;
			_conditions = conditions;
		}

		public RecruitTask(int amount, bool addToStack = false)
		{
			_amount = amount;
			_addToStack = addToStack;
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);
			int amount = Math.Min(_amount, controller.BoardZone.FreeSpace);

			if (amount == 0) return TaskState.STOP;

			ReadOnlySpan<IPlayable> deck = controller.DeckZone.GetSpan();
			SelfCondition[] conditions = _conditions;
			var indices = new List<int>();

			for (int i = 0; i < deck.Length; i++)
			{
				if (!(deck[i] is Minion)) continue;

				bool flag = true;
				for (int j = 0; j < conditions?.Length; j++)
					flag &= conditions[j].Eval(deck[i]);
				if (flag)
					indices.Add(i);
			}

			if (indices.Count == 0)
				return TaskState.STOP;

			int[] results = indices.ChooseNElements(amount, game.Random);

			var entities = new IPlayable[results.Length];
			for (int i = 0; i < entities.Length; i++)
				entities[i] = deck[results[i]];

			if (indices.Count > amount)
				game.OnRandomHappened(true);

			for (int i = 0; i < entities.Length; i++)
			{
				Generic.RemoveFromZone.Invoke(controller, entities[i]);
				Generic.SummonBlock.Invoke(game, (Minion)entities[i], -1, source);
				Vector().Add($"{Prefix()}Recruited.AssetId", entities[i].Card.AssetId);
				if (controller.BoardZone.IsFull)
					break;
			}

			if (_addToStack)
			{
				stack.Playables = entities;
				AddStackToVector(stack);
			}

			return TaskState.COMPLETE;
		}
	}
}
