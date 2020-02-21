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
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using SabberStoneCore.Actions;
using SabberStoneCore.Enums;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class CastRandomSpellTask : SimpleTask
	{
		private static readonly ConcurrentDictionary<int, Card[]> CachedCardLists =
			new ConcurrentDictionary<int, Card[]>();

		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}PhaseShift", Convert.ToInt32(_phaseShift) }
		};
		}

		private readonly Func<Card, bool> _condition;
		private readonly bool _phaseShift;

		public CastRandomSpellTask(Func<Card, bool> condition = null, bool phaseShift = true)
		{
			_condition = condition;
			_phaseShift = phaseShift;
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			if (_condition != null && !CachedCardLists.TryGetValue(source.Card.AssetId, out Card[] cards))
			{
				cards = Cards.FormatTypeCards(game.FormatType)
					.Where(card => card.Type == CardType.SPELL && _condition(card)).ToArray();

				CachedCardLists.TryAdd(source.Card.AssetId, cards);
			}
			else if (!CachedCardLists.TryGetValue(0, out cards))
			{
				cards = Cards.FormatTypeCards(game.FormatType)
					.Where(card => card.Type == CardType.SPELL && !card.IsQuest).ToArray();
				CachedCardLists.TryAdd(0, cards);
			}

			Controller c = source.Controller;
			Util.DeepCloneableRandom rnd = game.Random;

			Card randCard;
			do
			{
				randCard = cards[rnd.Next(cards.Length)];
			} while (!randCard.Implemented || randCard.HideStat);

			game.OnRandomHappened(true);

			Spell spellToCast = (Spell) target ?? (Spell) Entity.FromCard(c, randCard);
			Vector().Add($"{Prefix()}Process.SpellToCast.AssetId", spellToCast.Card.AssetId);

			game.Log(LogLevel.INFO, BlockType.POWER, "CastRandomSpellTask",
				!game.Logging ? "" : $"{source} casted {c.Name}'s {spellToCast}.");

			if (spellToCast.IsSecret && c.SecretZone.IsFull)
			{
				c.GraveyardZone.Add(spellToCast);
				return TaskState.COMPLETE;
			}

			ICharacter randTarget = spellToCast.GetRandomValidTarget();
			Vector().Add($"{Prefix()}Process.RandomTarget.AssetId", randTarget.Card.AssetId);

			int randChooseOne = rnd.Next(1, 3);

			Choice choiceTemp = c.Choice;
			c.Choice = null;

			game.TaskQueue.StartEvent();
			Generic.CastSpell.Invoke(c, game, spellToCast, randTarget, randChooseOne);
			Generic.OverloadBlock(c, spellToCast, game.History);
			// forced death processing & AA (Yogg)
			if (_phaseShift)
				game.DeathProcessingAndAuraUpdate();
			game.TaskQueue.EndEvent();


			//game.TaskQueue.Execute(spellToCast.Power.PowerTask, source.Controller, spellToCast, randTarget, randChooseOne);
			//game.DeathProcessingAndAuraUpdate();

			while (c.Choice != null)
			{
				game.TaskQueue.StartEvent();
				Generic.ChoicePick.Invoke(c, game, c.Choice.Choices.Choose(game.Random));
				game.ProcessTasks();
				game.TaskQueue.EndEvent();
				game.DeathProcessingAndAuraUpdate();
			}

			c.Choice = choiceTemp;

			return TaskState.COMPLETE;
		}
	}
}
