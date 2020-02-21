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
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Model.Entities;
using System.Collections.Generic;
using SabberStoneCore.Exceptions;
using SabberStoneCore.HearthVector;
using System.Collections.Specialized;
using System.Collections;

namespace SabberStoneCore.Model.Zones
{
	public class DeckZone : LimitedZone<IPlayable>
	{
		public const int StartingCards = 30;
		public const int DeckMaximumCapcity = 60;

		public override string Prefix()
		{
			return "DeckZone.";
		}

		// TODO: Barnabus the Stomper
		public bool NoEvenCostCards { get; private set; } = true;
		public bool NoOddCostCards { get; private set; } = true;

		public override OrderedDictionary Vector()
		{
			OrderedDictionary v = base.Vector();

			v.Add($"{Prefix()}NoEvenCostCards", Convert.ToInt32(NoEvenCostCards));
			v.Add($"{Prefix()}NoOddCostCards", Convert.ToInt32(NoOddCostCards));

			// v.Add(TopCard.Card.AssetId);
			//this.AddRange(TopCard.Vector);

			//if (Count > 0)
			for (int i = Count - 1; i > -1; --i)
			{
				v.AddRange(_entities[i].Vector(), Prefix());
				//if (_entities[i] != null)
				//{
				//	int j = 2;
				//	string prefix = _entities[i].Prefix;
				//	OrderedDictionary dup = _entities[i].Vector;
				//	if (v.Contains(s => s.Contains(prefix)))
				//	{
				//		prefix = prefix.Remove(prefix.Length - 1, 1) + j.ToString();
				//		while (v.Contains(s => s.Contains(prefix)))
				//		{
				//			j++;
				//			prefix = prefix.Remove(prefix.Length - 1, 1) + j.ToString();
				//		}
				//		dup = ReplacePrefix(i, prefix);
				//	}
				//	v.AddRange(dup, Prefix);
				//}
				//else
				//	v.AddRange(Playable.NullVector, Prefix);
			}

			return v;
		}

		private OrderedDictionary ReplacePrefix(int i, string prefix)
		{
			var dup = new OrderedDictionary();
			foreach (DictionaryEntry kv in _entities[i].Vector())
			{
				var kvKeyParts = kv.Key.ToString().Split('.').ToList();
				kvKeyParts[0] = prefix;
				string kvKey = String.Join(".", kvKeyParts);
				dup.Add(kvKey, kv.Value);
			}
			return dup;
		}

		public DeckZone(Controller controller) : base(Zone.DECK, DeckMaximumCapcity)
		{
			Game = controller.Game;
			Controller = controller;
		}

		private DeckZone(Controller c, DeckZone zone) : base(c, zone)
		{
			NoEvenCostCards = zone.NoEvenCostCards;
			NoOddCostCards = zone.NoOddCostCards;
		}

		public override bool IsFull => _count == DeckMaximumCapcity;

		public override void Add(IPlayable entity, int zonePosition = -1)
		{
			base.Add(entity, zonePosition);

			entity.Power?.Trigger?.Activate(entity, TriggerActivation.DECK);

			CheckParity(entity.Cost);
		}

		public override void ChangeEntity(IPlayable oldEntity, IPlayable newEntity)
		{
			Span<IPlayable> span = new Span<IPlayable>(_entities, 0, _count);
			bool flag = false;
			for (int i = 0; i < span.Length; i++)
				if (span[i] == oldEntity)
				{
					span[i] = newEntity;
					flag = true;
					break;
				}
			if (!flag) throw new ZoneException($"ChangeEntity: Can't find {oldEntity} in {this}.");
			newEntity.Zone = this;
		}

		public IPlayable TopCard => _entities[_count - 1];

		public void Fill(IReadOnlyCollection<string> excludeIds = null)
		{
			IReadOnlyList<Card> cards = Game.FormatType == FormatType.FT_STANDARD ? Controller.Standard : Controller.Wild;
			int cardsToAdd = StartingCards - _count;

			Game.Log(LogLevel.INFO, BlockType.PLAY, "Deck", !Game.Logging ? "" :
				$"Deck[{Game.FormatType}] from {Controller.Name} filling up with {cardsToAdd} random cards.");

			Util.DeepCloneableRandom rnd = Game.Random;
			while (cardsToAdd > 0)
			{
				Card card = cards.Choose(rnd);

				// don't add cards that have to be excluded here.
				if (excludeIds != null && excludeIds.Contains(card.Id))
					continue;

				// don't add cards that have already reached max occurence in deck.
				if (this.Count(c => c.Card == card) >= card.MaxAllowedInDeck)
					continue;

				Controller.Deck.Add(card);

				IPlayable entity = Entity.FromCard(Controller, card);
				Add(entity, 0);

				cardsToAdd--;
			}
		}

		public void Shuffle()
		{
			int n = _count;

			Util.DeepCloneableRandom rnd = Game.Random;

			Game.Log(LogLevel.INFO, BlockType.PLAY, "Deck", !Game.Logging ? "" : $"{Controller.Name} shuffles its deck.");

			IPlayable[] entities = _entities;
			for (int i = 0; i < n; i++)
			{
				int r = rnd.Next(i, n);
				IPlayable temp = entities[i];
				entities[i] = entities[r];
				entities[r] = temp;
			}

			ResetPositions();
		}

		public void ResetPositions()
		{
			for (int i = 0; i < _count; i++)
				_entities[_count - (i + 1)].ZonePosition = _count - (_count - i);
		}

		public void AddAtRandomPosition(IPlayable entity)
		{
			Add(entity, _count == 0 ? - 1 : Game.Random.Next(_count + 1));
		}

		public DeckZone Clone(Controller c)
		{
			return new DeckZone(c, this);
		}

		internal void SetEntity(int index, IPlayable newEntity)
		{
			_entities[index] = newEntity;
			newEntity.Zone = this;
			CheckParity(newEntity.Cost);
		}

		private void CheckParity(int cost)
		{
			if (!NoEvenCostCards && !NoOddCostCards) return;

			if ((cost & 1) == 0)
				NoEvenCostCards = false;
			else if (NoOddCostCards)
				NoOddCostCards = false;
		}
	}
}
