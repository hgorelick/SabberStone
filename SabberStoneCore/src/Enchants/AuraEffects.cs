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
using System.Text;
using SabberStoneCore.Enums;
using SabberStoneCore.HearthVector;

// ReSharper disable InconsistentNaming

namespace SabberStoneCore.Enchants
{
	/// <summary>
	/// A simple container for saving tag value perturbations from external Auras. Call indexer to get value for a particular Tag.
	/// </summary>
	public class AuraEffects : IHearthVector
	{
		private static readonly int _int32Size = sizeof(int);

		private const int PlayableLength = 2;
		private const int WeaponLength = PlayableLength + 1;
		private const int CharacterLength = PlayableLength + 2;
		private const int HeroLength = CharacterLength + 3;
		private const int MinionLength = CharacterLength + 7;

		public string Prefix()
		{
			return "AuraEffects.";
		}

		public OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}ATK", ATK },
			{ $"{Prefix()}CantAttackHeroes", Convert.ToInt32(CantAttackHeroes) },
			{ $"{Prefix()}CantBeTargetedBySpells", Convert.ToInt32(CantBeTargetedBySpells) },
			{ $"{Prefix()}CardCostHealth", Convert.ToInt32(CardCostHealth) },
			{ $"{Prefix()}Charge", Charge },
			{ $"{Prefix()}Echo", Convert.ToInt32(Echo) },
			{ $"{Prefix()}Health", Health },
			{ $"{Prefix()}HeroPowerDamage", HeroPowerDamage },
			{ $"{Prefix()}Immune", Immune },
			{ $"{Prefix()}Lifesteal", Convert.ToInt32(Lifesteal) },
			{ $"{Prefix()}Taunt", Convert.ToInt32(Taunt) },
			{ $"{Prefix()}Type", (int)Type }
		};
		}

		public static OrderedDictionary NullVector = new OrderedDictionary
		{
			{ $"NullAuraEffects.ATK", 0 },
			{ $"NullAuraEffects.CantAttackHeroes", 0 },
			{ $"NullAuraEffects.CantBeTargetedBySpells", 0 },
			{ $"NullAuraEffects.CardCostHealth", 0 },
			{ $"NullAuraEffects.Charge", 0 },
			{ $"NullAuraEffects.Echo", 0 },
			{ $"NullAuraEffects.Health", 0 },
			{ $"NullAuraEffects.HeroPowerDamage", 0 },
			{ $"NullAuraEffects.Immune", 0 },
			{ $"NullAuraEffects.Lifesteal", 0 },
			{ $"NullAuraEffects.Taunt", 0 },
			{ $"NullAuraEffects.Type", 0 }
		};

		// Indices:
		// Playables
		// 0 : CardCostHealth
		// 1 : Echo
		// Weapon
		// 2 : Immune
		// Characters
		// 2 : CantBeTargetedBySpells
		// 3 : ATK
		// Hero
		// 4 : CannotAttackHeroes
		// 5 : Immune
		// 6 : HEROPOWER_DAMAGE
		// Minion
		// 4 : Health
		// 5 : Charge
		// 6 : Taunt
		// 7 : Lifesteal
		// 8 : Rush
		// 9 : CantAttack
		internal readonly int[] _data;

		internal AuraEffects(CardType type)
		{
			switch (type)
			{
				case CardType.HERO:
					_data = new int[HeroLength];
					break;
				case CardType.MINION:
					_data = new int[MinionLength];
					break;
				case CardType.WEAPON:
					_data = new int[WeaponLength];
					break;
				case CardType.SPELL:
					_data = new int[PlayableLength];
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}

			Type = type;
		}

		private AuraEffects(AuraEffects original) : this (original.Type)
		{
			Buffer.BlockCopy(original._data, 0, _data, 0, _data.Length * _int32Size);
		}

		public readonly CardType Type;

		public bool CardCostHealth
		{
			get => _data[0] > 0;
			set => _data[0] = value ? 1 : 0;
		}

		public bool Echo
		{
			get => _data[1] > 0;
			set => _data[1] = value ? 1 : 0;
		}

		public bool CantBeTargetedBySpells
		{
			get => _data[2] > 0;
			set => _data[2] = value ? 1 : 0;
		}

		public int ATK
		{
			get => _data[3];
			set => _data[3] = value;
		}

		public int Health
		{
			get => _data[4];
			set => _data[4] = value;
		}

		public int Charge
		{
			get => _data[5];
			set => _data[5] = value;
		}

		public bool Taunt
		{
			get => _data[6] > 0;
			set => _data[6] = value ? 1 : 0;
		}

		public bool Lifesteal
		{
			get => _data[7] > 0;
			set => _data[7] = value ? 1 : 0;
		}

		public bool Rush
		{
			get => _data[8] > 0;
			set => _data[8] = value ? 1 : 0;
		}

		public bool CantAttack
		{
			get => _data[9] > 0;
			set => _data[9] = value ? 1 : 0;
		}

		// Only for Hero entities
		public bool CantAttackHeroes
		{
			get
			{
				if (Type != CardType.HERO)
					return false;
				return _data[4] > 0;
			}
			set
			{
				if (Type != CardType.HERO)
					throw new NotImplementedException();
				_data[4] = value ? 1 : 0;
			}
		}

		public int Immune
		{
			get
			{
				if (Type == CardType.HERO)
					return _data[5];
				if (Type == CardType.WEAPON)
					return _data[2];

				return 0;
			}
			set
			{
				if (Type == CardType.HERO)
					_data[5] = value;
				else if (Type == CardType.WEAPON)
					_data[2] = value;
				else
					throw new NotImplementedException();
			}
		}

		public int HeroPowerDamage
		{
			get
			{
				if (Type == CardType.HERO)
					return _data[6];
				return 0;
			}
			set
			{
				if (Type == CardType.HERO)
					_data[6] = value;
				else
					throw new NotImplementedException();
			}
		}

		public int this[in GameTag t]
		{
			get
			{
				switch (t)
				{
					case GameTag.ATK when Type != CardType.MINION:
						return 0;
					case GameTag.ATK:
						return ATK;
					case GameTag.HEALTH:
						return Health;
					//case GameTag.COST:
					//	//return GetCost() - ((Entity)Owner)._data[GameTag.COST];
					//	return GetCost() - Owner.GetNativeGameTag(GameTag.COST);
					case GameTag.CHARGE:
						return Charge > 0 ? 1 : 0;
					case GameTag.IMMUNE:
						return Immune;
					case GameTag.LIFESTEAL when Type == CardType.MINION:
						return Lifesteal ? 1 : 0;
					//case GameTag.CANT_ATTACK:
					//	return CANT_ATTACK;
					case GameTag.CANT_BE_TARGETED_BY_SPELLS:
					case GameTag.CANT_BE_TARGETED_BY_HERO_POWERS:
						return CantBeTargetedBySpells ? 1 : 0;
					case GameTag.CARD_COSTS_HEALTH:
						return CardCostHealth ? 1 : 0;
					case GameTag.RUSH:
						return Rush ? 1 : 0;
					case GameTag.ECHO:
						return Echo ? 1 : 0;
					case GameTag.CANNOT_ATTACK_HEROES:
						return CantAttackHeroes ? 1 : 0;
					case GameTag.HEROPOWER_DAMAGE:
						return HeroPowerDamage;
					default:
						return 0;
				}
			}
			set
			{
				switch (t)
				{
					case GameTag.ATK:
						ATK = value;
						return;
					case GameTag.HEALTH:
						Health = value;
						return;
					case GameTag.CHARGE:
						Charge += value;
						return;
					case GameTag.HEALTH_MINIMUM:
						//Owner[GameTag.HEALTH_MINIMUM] = value;
						return;
					case GameTag.IMMUNE:
						Immune = value;
						return;
					case GameTag.LIFESTEAL:
						Lifesteal = value > 0;
						return;
					//case GameTag.CANT_ATTACK:
					//	CANT_ATTACK = value;
					//	return;
					case GameTag.CANT_BE_TARGETED_BY_SPELLS:
					case GameTag.CANT_BE_TARGETED_BY_HERO_POWERS:
						CantBeTargetedBySpells = value > 0;
						return;
					case GameTag.CARD_COSTS_HEALTH:
						CardCostHealth = value > 0;
						return;
					case GameTag.RUSH:
						Rush = value > 0;
						return;
					case GameTag.ECHO:
						Echo = value > 0;
						return;
					case GameTag.CANNOT_ATTACK_HEROES:
						CantAttackHeroes = value > 0;
						return;
					case GameTag.HEROPOWER_DAMAGE:
						HeroPowerDamage = value;
						return;
					default:
						return;
				}
			}
		}

		public AuraEffects Clone()
		{
			return new AuraEffects(this);
		}

		public string Hash()
		{
			var hash = new StringBuilder();
			hash.Append("[AE:");
			int[] data = _data;
			for (int i = 0; i < data.Length; i++)
				hash.Append(data[i]);
			hash.Append("]");
			return hash.ToString();
		}
	}

	/// <summary>
	/// A collecton of controller Tag increments from Auras. These tags tends to be checked when a player plays any cards.
	/// </summary>
	public class ControllerAuraEffects : IHearthVector
	{
		public string Prefix()
		{
			return "ControllerAuraEffects.";
		}

		public OrderedDictionary Vector()
		{
			return new OrderedDictionary
		{
			{ $"{Prefix()}AllHealingDouble", AllHealingDouble },
			{ $"{Prefix()}ChooseBoth", ChooseBoth },
			{ $"{Prefix()}ExtraBattlecry", ExtraBattlecry },
			{ $"{Prefix()}ExtraBattlecryAndCombo", ExtraBattlecryAndCombo },
			{ $"{Prefix()}ExtraEndTurnEffect", ExtraEndTurnEffect },
			{ $"{Prefix()}HeroPowerDisabled", HeroPowerDisabled },
			{ $"{Prefix()}HeroPowerDouble", HeroPowerDouble },
			{ $"{Prefix()}RestoreToDamage", RestoreToDamage },
			{ $"{Prefix()}SpellPower", SpellPower },
			{ $"{Prefix()}SpellPowerDouble", SpellPowerDouble },
			{ $"{Prefix()}SpellsCostHealth", SpellsCostHealth },
			{ $"{Prefix()}TimeOut", TimeOut }
		};
		}

		public static OrderedDictionary NullVector =
			new OrderedDictionary
			{
				{ $"NullControllerAuraEffects.ControllerAuraEffects.AllHealingDouble", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.ChooseBoth", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.ExtraBattlecry", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.ExtraBattlecryAndCombo", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.ExtraEndTurnEffect", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.HeroPowerDisabled", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.HeroPowerDouble", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.RestoreToDamage", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.SpellPower", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.SpellPowerDouble", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.SpellsCostHealth", 0 },
				{ $"NullControllerAuraEffects.ControllerAuraEffects.TimeOut", 0 }
			};

		private int _timeOut;
		public int TimeOut => _timeOut;

		private int _spellPowerDouble;
		public int SpellPowerDouble => _spellPowerDouble;

		private int _heroPowerDouble;
		public int HeroPowerDouble => _heroPowerDouble;

		private int _restoreToDamage;
		public int RestoreToDamage => _restoreToDamage;

		private int _extraBattlecry;
		public int ExtraBattlecry => _extraBattlecry;

		private int _chooseBoth;
		public int ChooseBoth => _chooseBoth;

		private int _spellsCostHealth;
		public int SpellsCostHealth => _spellsCostHealth;

		private int _extraEndTurnEffect;
		public int ExtraEndTurnEffect => _extraEndTurnEffect;

		private int _heroPowerDisabled;
		public int HeroPowerDisabled => _heroPowerDisabled;

		private int _allHealingDouble;
		public int AllHealingDouble => _allHealingDouble;

		private int _extraBattlecryAndCombo;
		public int ExtraBattlecryAndCombo => _extraBattlecryAndCombo;

		private int _spellPower;
		public int SpellPower => _spellPower;

		public int this[GameTag t]
		{
			get
			{
				switch (t)
				{
					case GameTag.TIMEOUT:
						return _timeOut;
					case GameTag.SPELLPOWER_DOUBLE:
					case GameTag.SPELL_HEALING_DOUBLE:
						return _spellPowerDouble;
					case GameTag.HERO_POWER_DOUBLE:
						return _heroPowerDouble;
					case GameTag.HEALING_DOES_DAMAGE:
						return _restoreToDamage >= 1 ? 1 : 0;
					case GameTag.CHOOSE_BOTH:
						return _chooseBoth >= 1 ? 1 : 0;
					case GameTag.SPELLS_COST_HEALTH:
						return _spellsCostHealth >= 1 ? 1 : 0;
					case GameTag.EXTRA_BATTLECRIES_BASE:
						return _extraBattlecry;
					case GameTag.EXTRA_END_TURN_EFFECT:
						return _extraEndTurnEffect;
					case GameTag.HERO_POWER_DISABLED:
						return _heroPowerDisabled >= 1 ? 1 : 0;
					case GameTag.ALL_HEALING_DOUBLE:
						return _allHealingDouble;
					case GameTag.EXTRA_MINION_BATTLECRIES_BASE:
						return _extraBattlecryAndCombo;
					case GameTag.SPELLPOWER:
						return _spellPower;
					default:
						return 0;
				}
			}
			set
			{
				switch (t)
				{
					case GameTag.TIMEOUT:
						_timeOut = value;
						return;
					case GameTag.SPELLPOWER_DOUBLE:
					case GameTag.SPELL_HEALING_DOUBLE:
						_spellPowerDouble = value;
						return;
					case GameTag.HERO_POWER_DOUBLE:
						_heroPowerDouble = value;
						return;
					case GameTag.HEALING_DOES_DAMAGE:
						_restoreToDamage = value;
						return;
					case GameTag.CHOOSE_BOTH:
						_chooseBoth = value;
						return;
					case GameTag.SPELLS_COST_HEALTH:
						_spellsCostHealth = value;
						return;
					case GameTag.EXTRA_BATTLECRIES_BASE:
						_extraBattlecry = value;
						return;
					case GameTag.EXTRA_END_TURN_EFFECT:
						_extraEndTurnEffect = value;
						return;
					case GameTag.HERO_POWER_DISABLED:
						_heroPowerDisabled = value;
						return;
					case GameTag.ALL_HEALING_DOUBLE:
						_allHealingDouble = value;
						return;
					case GameTag.EXTRA_MINION_BATTLECRIES_BASE:
						_extraBattlecryAndCombo = value;
						return;
					case GameTag.SPELLPOWER:
						_spellPower = value;
						return;
					default:
						return;
				}
			}
		}

		public ControllerAuraEffects Clone()
		{
			return (ControllerAuraEffects)MemberwiseClone();
		}

		public string Hash()
		{
			var sb = new StringBuilder("[CAE:");
			sb.Append(_timeOut);
			sb.Append(_spellPowerDouble);
			sb.Append(_heroPowerDouble);
			sb.Append(_restoreToDamage);
			sb.Append(_extraBattlecry);
			sb.Append(_chooseBoth);
			sb.Append(_spellsCostHealth);
			sb.Append(_extraEndTurnEffect);
			sb.Append(_heroPowerDisabled);
			sb.Append(_allHealingDouble);
			sb.Append(_extraBattlecryAndCombo);
			sb.Append(_spellPower);
			sb.Append("]");
			return sb.ToString();
		}
	}
}
