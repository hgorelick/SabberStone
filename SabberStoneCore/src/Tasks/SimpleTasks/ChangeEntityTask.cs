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

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class ChangeEntityTask : SimpleTask
	{
		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}ChangeEntityTo.AssetId", AssetId },
			{ $"{Prefix()}CardClass", (int)_cardClass },
			{ $"{Prefix()}CardType", (int)_cardType },
			{ $"{Prefix()}OpClass", Convert.ToInt32(_opClass) },
			{ $"{Prefix()}ProtoType", (int)_protoType },
			{ $"{Prefix()}Race", (int)_race },
			{ $"{Prefix()}Rarity", (int)_rarity },
			{ $"{Prefix()}RemoveEnchantments", Convert.ToInt32(_removeEnchantments) },
			{ $"{Prefix()}Type", (int)Type },
			{ $"{Prefix()}UseRandomCard", Convert.ToInt32(_useRandomCard) }
		};
		}

		private readonly Card _card;
		public int AssetId => _card.AssetId;
		private readonly CardType _cardType;
		//public CardType CardType => _cardType;
		private readonly bool _opClass;
		//public bool OpClass => _opClass;
		private readonly Rarity _rarity;
		private readonly Race _race;
		private readonly EntityType _type;
		public EntityType Type => _type;
		private readonly EntityType _protoType;
		//public EntityType ProtoType => _protoType;
		private readonly bool _useRandomCard;
		private readonly bool _removeEnchantments;
		private CardClass _cardClass;

		/// <summary>
		/// Creates a task that transforms the given type of entities
		/// into random cards satisfying the criteria.
		/// </summary>
		/// <param name="opClass">Transform entities into the opponent's class cards.</param>
		/// <param name="removeEnchantments">Removes any applied enchantments during this transformation.</param>
		public ChangeEntityTask(EntityType type, CardType cardType, CardClass cardClass = CardClass.INVALID,
			Rarity rarity = Rarity.INVALID, Race race = Race.INVALID, bool opClass = false, bool removeEnchantments = false)
		{
			_type = type;
			_cardType = cardType;
			_cardClass = cardClass;
			_opClass = opClass;
			_rarity = rarity;
			_race = race;
			_useRandomCard = true;
			_removeEnchantments = removeEnchantments;
		}

		/// <summary>
		/// Creates a task that transforms the given type of entities
		/// into the given card.
		/// </summary>
		public ChangeEntityTask(string cardId, EntityType type = EntityType.SOURCE)
		{
			_card = Cards.FromId(cardId);
			_type = type;
		}

		/// <summary>
		/// Creates a task that transforms the given type of entities
		/// into the card of the another type of entity.
		/// </summary>
		public ChangeEntityTask(EntityType sourceType, EntityType protoType)
		{
			_type = sourceType;
			_protoType = protoType;
		}

		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			if (_opClass)
				_cardClass = controller.Opponent.HeroClass;

			if (_useRandomCard)
			{
				IReadOnlyList<Card> randCards =
					RandomCardTask.GetCardList(source, _cardType, _cardClass, race: _race, rarity: _rarity);
				foreach (IPlayable p in IncludeTask.GetEntities(_type, in controller, source, target, stack?.Playables))
				{
					Card pick = randCards.Choose(game.Random);

					Vector().Add($"{Prefix()}Process.RandomChoice.AssetId", Generic.ChangeEntityBlock.Invoke(controller, p, pick, _removeEnchantments).Card.AssetId);

					//TODO p[GameTag.DISPLAYED_CREATOR] = source.Id;
				}

				return TaskState.COMPLETE;
			}

			Card card = _protoType != EntityType.INVALID
				? IncludeTask.GetEntities(_protoType, in controller, source, target, stack?.Playables)[0].Card
				: _card;

			foreach (IPlayable p in IncludeTask.GetEntities(_type, in controller, source, target, stack?.Playables))
				Vector().Add($"{Prefix()}Process.EntityChangedTo.AssetId", Generic.ChangeEntityBlock.Invoke(controller, p, card, _removeEnchantments).Card.AssetId);

			// TODO p[GameTag.DISPLAYED_CREATOR] = source.Id;

			return TaskState.COMPLETE;
		}
	}
}
