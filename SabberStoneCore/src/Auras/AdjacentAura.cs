using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SabberStoneCore.Enchants;
using SabberStoneCore.HearthVector;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;
// ReSharper disable ObjectCreationAsStatement

namespace SabberStoneCore.Auras
{
	public class AdjacentAura : IAura
	{
		public string Prefix()
		{
			return "AdjacentAura.";
		}

		public OrderedDictionary Vector()
		{
			var v = new OrderedDictionary
				{
					{ $"{Prefix()}BoardChanged", Convert.ToInt32(BoardChanged) },
					{ $"{Prefix()}EnchantmentCard.AssetId", EnchantmentCard.AssetId }
				};

			if (Effects.Length > 0)
				for (int i = 0; i < Effects.Length; ++i)
					this.AddRange(Effects[i].Vector());
			//else
			//	this.AddRange(Effect.NullVector);

			v.Add($"{Prefix()}Left", Convert.ToInt32(Left != null));
			//v.Add($"{Prefix}Owner", Owner != null ? Owner.Card.AssetId : 0);
			v.Add($"{Prefix()}Right", Convert.ToInt32(Right != null));
			v.Add($"{Prefix()}ToBeRemoved", Convert.ToInt32(ToBeRemoved));

			return v;
		}

		public static OrderedDictionary NullVector
		{
			get
			{
				string prefix = "NullAdjacentAura.";
				var v = new OrderedDictionary
				{
					{ $"{prefix}BoardChanged", 0 },
					{ $"{prefix}EnchantmentCard.AssetId", 0 }
				};

				v.AddRange(Effect.NullVector, prefix);
				v.Add($"{prefix}Left", 0);
				v.Add($"{prefix}Owner", 0);
				v.Add($"{prefix}Right", 0);
				v.Add("ToBeRemoved", 0);

				return v;
			}
		}
			

		private Minion _left;
		public Minion Left => _left;

		private Minion _right;
		public Minion Right => _right;

		private bool _toBeRemoved;
		public bool ToBeRemoved => _toBeRemoved;

		private readonly IEffect[] _effects;
		public IEffect[] Effects => _effects;

		private readonly Minion _owner;
		public Minion Owner => _owner;

		private readonly BoardZone _board;
		private readonly bool _history;

		public readonly Card EnchantmentCard;

		IPlayable IAura.Owner => _owner;
		public bool BoardChanged { get; set; }


		public AdjacentAura(params IEffect[] effects)
		{
			_effects = effects;
		}

		public AdjacentAura(string enchantmentId)
		{
			EnchantmentCard = Cards.FromId(enchantmentId);
			//_effects = EnchantmentCard.Power.Enchant.Effects;
		}

		private AdjacentAura(AdjacentAura prototype, Minion owner, bool cloning) : this()
		{
			EnchantmentCard = prototype.EnchantmentCard;
			_effects = prototype._effects ?? EnchantmentCard.Power.Enchant.Effects;
			_owner = owner;
			_history = owner.Game.History;
			owner.OngoingEffect = this;
			owner.Game.Auras.Add(this);
			_board = owner.Controller.BoardZone;
			_board.AdjacentAuras.Add(this);

			if (cloning)
			{
				if (prototype._left != null)
					_left = (Minion)owner.Game.IdEntityDic[prototype._left.Id];
				if (prototype._right != null)
					_right = (Minion)owner.Game.IdEntityDic[prototype._right.Id];
			}
		}

		public void Update()
		{
			if (_toBeRemoved)
			{
				if (_left != null)
					DeApply(_left);
				if (_right != null)
					DeApply(_right);
				_owner.OngoingEffect = null;
				_owner.Game.Auras.Remove(this);
				return;
			}

			if (!BoardChanged) return;

			int pos = _owner.ZonePosition;

			// Check left-side
			if (_left != null)
			{
				//if (!(_left.Zone is BoardZone) || _left.ZonePosition != pos - 1)
				//{
				//	DeApply(_left);
				//	_left = null;
				//}
				if (!(_left.Zone is BoardZone))
				{
					_left = null;
				}
				else if
					(_left.ZonePosition != pos - 1)
				{
					DeApply(_left);
					_left = null;
				}
			}

			if (_left == null)
			{
				if (pos > 0)
				{
					Minion left = _board[pos - 1];
					if (!left.Untouchable)
					{
						Apply(left);
						_left = left;
					}
				}
				else
					_left = null;
			}

			// Check right-side
			if (_right != null)
			{
				//if (!(_right.Zone is BoardZone) || _right.ZonePosition != pos + 1)
				//{
				//	DeApply(_right);
				//	_right = null;
				//}
				if (!(_right.Zone is BoardZone))
				{
					_right = null;
				}
				else if
					(_right.ZonePosition != pos - 1)
				{
					DeApply(_right);
					_right = null;
				}
			}

			if (_right == null)
			{
				if (pos < _board.Count - 1)
				{
					Minion right = _board[pos + 1];
					if (!right.Untouchable)
					{
						Apply(right);
						_right = right;
					}
				}
				else
					_right = null;
			}

			BoardChanged = false;
		}

		public void Remove()
		{
			_toBeRemoved = true;
		}

		public void Clone(IPlayable clone)
		{
			new AdjacentAura(this, (Minion) clone, true);
		}

		void IAura.Activate(IPlayable owner)
		{
			new AdjacentAura(this, (Minion) owner, false);
		}

		private void Apply(Minion m)
		{
			for (int i = 0; i < _effects.Length; i++)
				_effects[i].ApplyAuraTo(m);

			if (EnchantmentCard != null && _history)
				Enchantment.GetInstance(m.Controller, _owner, m, in EnchantmentCard);
		}

		private void DeApply(Minion m)
		{
			for (int i = 0; i < _effects.Length; i++)
				_effects[i].RemoveAuraFrom(m);

			if (EnchantmentCard != null && (_history || EnchantmentCard.Power.Trigger != null))
			{
				int cardId = EnchantmentCard.AssetId;
				List<Enchantment> enchantments = m.AppliedEnchantments;
				for (int i = enchantments.Count - 1; i >= 0; i--)
					if (enchantments[i].Creator == _owner && enchantments[i].Card.AssetId == cardId)
					{
						enchantments.RemoveAt(i);
						break;
					}
			}
		}

		public override string ToString()
		{
			return $"[AdjAura:{_owner}]";
		}
	}
}
