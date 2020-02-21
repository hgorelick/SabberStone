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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class RandomMinionTask : SimpleTask
	{
		private static readonly ConcurrentDictionary<int, List<Card>> CachedCards =
			new ConcurrentDictionary<int, List<Card>>();

		private readonly bool _opponent;
		public bool Opponent => _opponent;

		public int Amount { get; set; }
		public bool ClassAndMultiOnlyFlag { get; set; }
		public bool MaxInDeckFlag { get; set; }
		public RelaSign RelaSign { get; set; }
		public GameTag Tag { get; set; }
		public EntityType Type { get; set; }
		public int Value { get; set; }

		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}Amount", Amount },
			{ $"{Prefix()}ClassAndMultiOnlyFlag", Convert.ToInt32(ClassAndMultiOnlyFlag) },
			{ $"{Prefix()}MaxInDeckFlag", Convert.ToInt32(MaxInDeckFlag) },
			{ $"{Prefix()}RelaSign", (int)RelaSign },
			{ $"{Prefix()}GameTag", (int)Tag },
			{ $"{Prefix()}Type", (int)Type },
			{ $"{Prefix()}Value", Value }
		};
		}

		public RandomMinionTask(GameTag tag, EntityType type, int amount = 1, bool opponent = false)
		{
			Tag = tag;
			Value = -1;
			Amount = amount;
			Type = type;
			ClassAndMultiOnlyFlag = false;
			MaxInDeckFlag = false;
			RelaSign = RelaSign.EQ;
			_opponent = opponent;
		}

		public RandomMinionTask(GameTag tag, int value, int amount = 1, RelaSign relaSign = RelaSign.EQ,
			bool classAndMultiOnlyFlag = false, bool maxInDeckFlag = false, bool opponent = false)
		{
			Tag = tag;
			Value = value;
			Amount = amount;
			Type = EntityType.INVALID;
			ClassAndMultiOnlyFlag = classAndMultiOnlyFlag;
			MaxInDeckFlag = maxInDeckFlag;
			RelaSign = relaSign;
			_opponent = opponent;
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			List<Card> cardsList = null;
			if (Type != EntityType.INVALID)
			{
				if (Type == EntityType.TARGET && Tag == GameTag.COST)
				{
					Value = ((IPlayable) target).Cost;
					cardsList = Cards.CostMinionCards(game.FormatType)[Value];
				}
				else
				{
					throw new NotImplementedException();
				}
			}

			if (cardsList == null && Tag == GameTag.COST && RelaSign == RelaSign.EQ)
				cardsList = Cards.CostMinionCards(game.FormatType)[Value];

			if (cardsList == null && !CachedCards.TryGetValue(source.Card.AssetId, out cardsList))
			{
				IEnumerable<Card> cards;
				if (game.FormatType == FormatType.FT_STANDARD)
					cards = ClassAndMultiOnlyFlag ? controller.Standard : Cards.AllStandard;
				else
					cards = ClassAndMultiOnlyFlag ? controller.Wild : Cards.AllWild;

				if (Tag == GameTag.CARDRACE && RelaSign == RelaSign.EQ)
				{
					cardsList = cards.Where(p => p.Type == CardType.MINION
							 && p.IsRace((Race)Value)).ToList();;
				}
				else
				{
					cardsList = cards.Where(p => p.Type == CardType.MINION
												 && (RelaSign == RelaSign.EQ && p[Tag] == Value
													 || RelaSign == RelaSign.GEQ && p[Tag] >= Value
													 || RelaSign == RelaSign.LEQ && p[Tag] <= Value)).ToList();
				}

				CachedCards.TryAdd(source.Card.AssetId, cardsList);
			}

			if (cardsList.Count == 0) return TaskState.STOP;

			var randomMinions = new List<IPlayable>(Amount);
			if (Amount > 1)
			{
				var list = new List<Card>(cardsList);
				while (randomMinions.Count < Amount && cardsList.Count > 0)
				{
					Card card = list.Choose(game.Random);
					list.Remove(card);

					// check for deck rules
					if (MaxInDeckFlag && controller.Deck.Count(p => p.Id == card.Id) >= card.MaxAllowedInDeck)
						continue;

					randomMinions.Add(Entity.FromCard(_opponent ? controller.Opponent : controller, card));
				}
			}
			else
			{
				randomMinions.Add(Entity.FromCard(_opponent ? controller.Opponent : controller,
					cardsList.Choose(game.Random)));
			}


			stack.Playables = randomMinions;

			AddStackToVector(stack);

			game.OnRandomHappened(true);

			return TaskState.COMPLETE;
		}
	}
}
