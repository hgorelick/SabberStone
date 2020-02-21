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
using SabberStoneCore.Enums;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Tasks.SimpleTasks
{
	public class AddCardTo : SimpleTask
	{
		public override OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}IsTrigger", Convert.ToInt32(IsTrigger) },
			{ $"{Prefix()}Amount", Amount },
			{ $"{Prefix()}CardToAdd.AssetId", Card.AssetId },
			{ $"{Prefix()}Type", (int)Type }
		};
		}

		private readonly int _amount;
		public int Amount => _amount;
		private readonly Card _card;
		public Card Card => _card;
		private readonly EntityType _type;
		public EntityType Type => _type;

		public AddCardTo(Card card, EntityType type, int amount = 1)
		{
			_card = card;
			_type = type;
			_amount = amount;
		}

		public AddCardTo(string cardId, EntityType type, int amount = 1)
		{
			_card = Cards.FromId(cardId);
			_type = type;
			_amount = amount;
		}


		public override TaskState Process(in Game game, in Controller controller, in IEntity source,
			in IPlayable target,
			in TaskStack stack = null)
		{
			AddSourceAndTargetToVector(source, target);

			var entities = new IPlayable[_amount];

			switch (_type)
			{
				case EntityType.DECK:
					for (int i = 0; i < entities.Length; i++)
					{
						entities[i] = Entity.FromCard(in controller, in _card);
						entities[i][GameTag.DISPLAYED_CREATOR] = source.Id;
					}

					for (int i = 0; i < entities.Length && !controller.DeckZone.IsFull; i++)
					{
						//Vector.Add(entities[i].Card.AssetId);
						Generic.ShuffleIntoDeck.Invoke(controller, source, entities[i]);
					}
					return TaskState.COMPLETE;

				case EntityType.HAND:
					for (int i = 0; i < entities.Length; i++)
					{
						entities[i] = Entity.FromCard(controller, _card);
						entities[i][GameTag.DISPLAYED_CREATOR] = source.Id;
					}

					for (int i = 0; i < entities.Length; i++)
					{
						//Vector.Add(entities[i].Card.AssetId);
						Generic.AddHandPhase.Invoke(controller, entities[i]);
					}
					return TaskState.COMPLETE;

				case EntityType.OP_HAND:
					for (int i = 0; i < entities.Length; i++)
					{
						entities[i] = Entity.FromCard(controller.Opponent, _card);
						entities[i][GameTag.DISPLAYED_CREATOR] = source.Id;
					}

					for (int i = 0; i < entities.Length; i++)
					{
						//Vector.Add(entities[i].Card.AssetId);
						Generic.AddHandPhase.Invoke(controller.Opponent, entities[i]);
					}
					return TaskState.COMPLETE;

				case EntityType.OP_DECK:
					for (int i = 0; i < entities.Length; i++)
					{
						entities[i] = Entity.FromCard(controller.Opponent, _card);
						entities[i][GameTag.DISPLAYED_CREATOR] = source.Id;
					}

					for (int i = 0; i < entities.Length && !controller.Opponent.DeckZone.IsFull; i++)
					{
						//Vector.Add(entities[i].Card.AssetId);
						Generic.ShuffleIntoDeck.Invoke(controller.Opponent, source, entities[i]);
					}
					return TaskState.COMPLETE;

				default:
					throw new NotImplementedException();
			}
		}
	}
}
