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
using System.Linq;
using System.Linq.Expressions;
using SabberStoneCore.Enums;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCore.Conditions
{
	#region Hardcoded SelfConditionType Enum
	//using RG = Tuple<RelaSign, GameTag>;
	//using GRV = Tuple<GameTag, RelaSign, int>;
	//using GRVs = List<Tuple<GameTag, RelaSign, int>>;
	//using GRVl = Tuple<GameTag, RelaSign, Func<IPlayable, int>>;
	//using GRVls = List<Tuple<GameTag, RelaSign, Func<IPlayable, int>>>;

	//public enum SelfConditionType
	//{
	//	UNUSED = -1,
	//	IS_DEAD,
	//	IS_NOT_IMMUNE,
	//	IS_SILENCED,
	//	IS_BOARD_FULL,
	//	IS_HAND_EMPTY,
	//	IS_DECK_EMPTY,
	//	IS_OP_DECK_NOT_EMPTY,
	//	IS_HAND_NOT_EMPTY,
	//	IS_HAND_FULL,
	//	IS_OP_HAND_EMPTY,
	//	IS_OP_HAND_FULL,
	//	IS_CURRENT_PLAYER,
	//	IS_NOT_CURRENT_PLAYER,
	//	IS_COMBO_ACTIVE,
	//	IS_ANY_WEAPON_EQUIPPED,
	//	IS_THIS_WEAPON_EQUIPPED,
	//	IS_DAMAGED,
	//	IS_UNDAMAGED,
	//	IS_CONTROLLING_MURLOC,
	//	IS_CONTROLLING_DEMON,
	//	IS_CONTROLLING_MECH,
	//	IS_CONTROLLING_ELEMENTAL,
	//	IS_CONTROLLING_BEAST,
	//	IS_CONTROLLING_TOTEM,
	//	IS_CONTROLLING_PIRATE,
	//	IS_CONTROLLING_DRAGON,
	//	IS_CONTROLLING_ALL,
	//	IS_CONTROLLING_FROZEN,
	//	IS_DRAGON_IN_HAND,
	//	IS_5_OR_MORE_ATTACK_IN_HAND,
	//	HAS_5_PLUS_COST_SPELL_IN_HAND,
	//	IS_MURLOC,
	//	IS_DEMON,
	//	IS_MECH,
	//	IS_ELEMENTAL,
	//	IS_BEAST,
	//	IS_TOTEM,
	//	IS_PIRATE,
	//	IS_DRAGON,
	//	IS_ALL,
	//	IS_NOT_MURLOC,
	//	IS_NOT_DEMON,
	//	IS_NOT_MECH,
	//	IS_NOT_ELEMENTAL,
	//	IS_NOT_BEAST,
	//	IS_NOT_TOTEM,
	//	IS_NOT_PIRATE,
	//	IS_NOT_DRAGON,
	//	IS_NOT_ALL,
	//	IS_MINION,
	//	IS_SPELL,
	//	IS_WEAPON,
	//	IS_WEAPON_EQUIPPED,
	//	IS_HERO,
	//	IS_HERO_POWER,
	//	IS_SECRET,
	//	IS_HERO_POWER_TARGETING_MINION,
	//	HAS_ARMOR_LESS_THAN_5,
	//	IS_ATTACKING,
	//	IS_CTHUN,
	//	IS_SILVER_HAND_RECRUIT,
	//	IS_TREANT,
	//	IS_CONTROLLING_TREANT,
	//	IS_CONTROLLING_LACKEY,
	//	IS_SPELL_DMG_ON_HERO,
	//	ISNT_SPELL_DMG_ON_HERO,
	//	//IS_NOT_ATTACKING_THIS_TURN
	//	IS_BOMB,
	//	IS_BLOOD_OF_THE_ANCIENT_ONE,
	//	IS_NOT_DEATHKNIGHT,
	//	IS_NOT_DRUID,
	//	IS_NOT_HUNTER,
	//	IS_NOT_MAGE,
	//	IS_NOT_PALADIN,
	//	IS_NOT_PRIEST,
	//	IS_NOT_ROGUE,
	//	IS_NOT_SHAMAN,
	//	IS_NOT_WARLOCK,
	//	IS_NOT_WARRIOR,
	//	IS_NOT_START_IN_DECK,
	//	ZERO_MINIONS_PLAYED_THIS_TURN,
	//	ZERO_SPELLS_PLAYED_THIS_TURN,
	//	ELEMENTAL_PLAYED_LAST_TURN,
	//	HAS_MINION_IN_DECK,
	//	HAS_TAUNT_MINION_IN_DECK,
	//	HAS_DIVINE_SHIELD_MINION_IN_DECK,
	//	HAS_LIFESTEAL_MINION_IN_DECK,
	//	HAS_WINDFURY_MINION_IN_DECK,
	//	HAS_SPELL_IN_DECK,
	//	IS_NO_DUPE_IN_DECK,
	//	HAS_NO_2_COST_CARDS_IN_DECK,
	//	HAS_NO_3_COST_CARDS_IN_DECK,
	//	HAS_NO_4_COST_CARDS_IN_DECK,
	//	HAS_NO_MINION_IN_DECK,
	//	HAS_NO_ODD_COST_IN_DECK,
	//	HAS_NO_EVEN_COST_IN_DECK,
	//	HAS_MINION_IN_HAND,
	//	HAS_MY_HERO_ATTACKED_THIS_TURN,
	//	HAS_MY_HERO_NOT_ATTACKED_THIS_TURN,
	//	IS_MY_HERO_DAMAGED_THIS_TURN,
	//	IS_DEATHRATTLE_CARD,
	//	IS_ECHO_CARD,
	//	IS_COMBO_CARD,
	//	IS_LIFESTEAL_CARD,
	//	IS_DEATHRATTLE_MINION,
	//	IS_BATTLECRY_MINION,
	//	IS_DIVINE_SHIELD_MINION,
	//	IS_LIFESTEAL_MINION,
	//	IS_CHARGE_MINION,
	//	HAS_RUSH,
	//	IS_CTHUN_DEAD,
	//	NOT_PLAYED_ANY_SPELL_THIS_TURN,
	//	IS_IN_HAND,
	//	IS_IN_PLAY,
	//	IS_IN_ZONE,
	//	IS_OVERLOAD_CARD,
	//	IS_BATTLECRY_CARD,
	//	IS_CHOOSE_ONE_CARD,
	//	HAS_TAUNT,
	//	IS_FROZEN,
	//	IS_HERO_POWER_MIND_SPIKE,
	//	IS_MANA_CRYSTAL_FULL,
	//	IS_REMAINING_MANA_FULL,
	//	IS_NOT_DEAD,
	//	IS_NOT_UNTOUCHABLE,
	//	IS_NOT_SILENCED,
	//	IS_NOT_BOARD_FULL,
	//	//IS_DURABILITY_OKAY,
	//	//IS_ANY_NOT_IMMUNE,
	//	IS_OP_NOT_BOARD_FULL,
	//	IS_OP_TURN,
	//	IS_MY_TURN,
	//	//IS_SECRET_OR_QUEST_ACTIVE,
	//	//IS_QUEST_DONE,
	//	IS_SPELL_TARGETING_MINION,
	//	HOLDING_ANOTHER_CLASS_CARD,
	//	IS_PROPOSED_DEFENDER,
	//	//HAS_LESS_HAND_CARDS_THAN_OP,
	//	IS_ANY_DIED_THIS_TURN,
	//	DOES_OP_HAVE_MORE_MINIONS,
	//	HAS_TARGET,
	//	ANY_NON_ROGUE_CARD_IN_HAND,
	//	IS_DECK_HAND_BATTLEFIELD_EMPTY,
	//	IS_MAX_SECRETS_IN_PLAY,
	//	IS_1_MINION_IN_PLAY,
	//	OP_HAND_HAS_6_OR_MORE_CARDS,
	//	IS_CONTROLLING_MINION_5_OR_MORE_ATTACK,
	//	IS_CONTROLLING_MINION_6_OR_MORE_HEALTH,
	//	IS_CONTROLLING_TAUNT_MINION,
	//	IS_CONTROLLING_SPELL_DAMAGE_MINION,
	//	IS_OP_CONTROLLING_TAUNT_MINION,
	//	IS_OP_CONTROLLING_FROZEN,
	//	IS_CONTROLLING_SECRET,
	//	IS_COST_1,
	//	IS_COST_2_OR_LESS,
	//	IS_COST_3_OR_LESS,
	//	IS_COST_4_OR_LESS,
	//	IS_COST_5_OR_MORE,
	//	IS_COST_7,
	//	IS_COST_8,
	//	IS_COST_9,
	//	IS_COST_10,
	//	IS_ATTACK_1,
	//	IS_ATTACK_2,
	//	IS_ATTACK_3,
	//	IS_ATTACK_2_OR_LESS,
	//	IS_ATTACK_2_OR_MORE,
	//	IS_ATTACK_3_OR_MORE,
	//	IS_ATTACK_4_OR_MORE,
	//	IS_ATTACK_5_OR_MORE,
	//	IS_HEALTH_1,
	//	IS_WEAPON_ATTACK_3_OR_MORE,
	//	CANT_ATTACK_HEROES,
	//	CANT_ATTACK_HEROES_THIS_TURN,
	//	IS_CTHUN_ATTACK_10_OR_MORE,
	//	IS_HEALTH_12_OR_LESS,
	//	IS_HEALTH_15_OR_LESS,
	//	IS_OP_HEALTH_15_OR_LESS,
	//	IS_ONLY_MINION,
	//	IS_CONTROLLING_2_OR_MORE_MINIONS,
	//	IS_OP_CONTROLLING_2_OR_MORE_MINIONS,
	//	IS_OP_CONTROLLING_3_OR_MORE_MINIONS,
	//	IS_HERO_LETHAL_PREDAMAGED,
	//	IS_RESTORED_HEALTH_3_OR_MORE,
	//	IS_TARGET_MINION,
	//	IS_TARGET_HERO,
	//	RESTORED_5_OR_MORE_HEALTH_IN_GAME,
	//	RESTORED_10_OR_MORE_HEALTH_IN_GAME,
	//	HAS_HERO_POWER_DEALT_8_OR_MORE_DAMAGE,
	//	RESTORED_HEALTH,
	//	RESTORED_5_OR_MORE_HEALTH,
	//	DID_KILL_MINION
	//}
	#endregion
	/// <summary>
	/// Container for all conditions about the subject <see cref="IPlayable"/>
	/// instance.
	/// </summary>
	public partial class SelfCondition
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	{
		public static readonly SelfCondition IsDead =
			new SelfCondition(me => me.ToBeDestroyed && me.Card.Type == CardType.MINION);

		public static readonly SelfCondition IsNotImmune =
			new SelfCondition(me => me is ICharacter && !((ICharacter)me).IsImmune);

		public static readonly SelfCondition IsSilenced =
			new SelfCondition(me => me is ICharacter && ((ICharacter)me).IsSilenced);

		public static readonly SelfCondition IsBoardFull =
			new SelfCondition(me => me.Controller.BoardZone.IsFull);

		public static readonly SelfCondition IsHandEmpty =
			new SelfCondition(me => me.Controller.HandZone.IsEmpty);

		public static readonly SelfCondition IsDeckEmpty =
			new SelfCondition(me => me.Controller.DeckZone.IsEmpty);

		public static readonly SelfCondition IsOpDeckNotEmpty =
			new SelfCondition(me => !me.Controller.Opponent.DeckZone.IsEmpty);

		public static readonly SelfCondition IsHandNotEmpty =
			new SelfCondition(me => !me.Controller.HandZone.IsEmpty);

		public static readonly SelfCondition IsHandFull =
			new SelfCondition(me => me.Controller.HandZone.IsFull);

		public static readonly SelfCondition IsOpHandEmpty =
			new SelfCondition(me => me.Controller.Opponent.HandZone.IsEmpty);

		public static readonly SelfCondition IsOpHandFull =
			new SelfCondition(me => me.Controller.Opponent.HandZone.IsFull);

		//public static SelfCondition IsStackEmpty => new SelfCondition(me => me == null);

		//public static readonly SelfCondition IsCurrentPlayer =
		//	new SelfCondition(me => me.Game.CurrentPlayer == me.Controller);

		//public static readonly SelfCondition IsNotCurrentPlayer =
		//	new SelfCondition(me => me.Game.CurrentPlayer != me.Controller);

		public static readonly SelfCondition IsComboActive =
			new SelfCondition(me => me.Controller.IsComboActive);

		public static readonly SelfCondition IsAnyWeaponEquipped =
			new SelfCondition(me => me as Hero != null && (me as Hero).Weapon != null);

		//public static readonly SelfCondition IsThisWeaponEquipped =
		//	new SelfCondition(me => me.Controller.Hero.Weapon == me);

		public static readonly SelfCondition IsDamaged =
			new SelfCondition(me => me is ICharacter && ((ICharacter)me).Damage > 0);

		public static readonly SelfCondition IsUndamaged =
			new SelfCondition(me => me is ICharacter && ((ICharacter)me).Damage == 0);

		public static SelfCondition IsControllingRace(Race race)
		{
			return new SelfCondition(me => me.Controller.BoardZone.Any(p => p.Card.IsRace(race)));
		}

		public static readonly SelfCondition IsControllingFrozen =
			new SelfCondition(me => me.Controller.BoardZone.Any(p => p.IsFrozen));

		public static readonly SelfCondition IsControllingSecret =
			new SelfCondition(me => me.Controller.SecretZone.Count > 0);

		public static readonly SelfCondition IsDragonInHand =
			new SelfCondition(me => me.Controller.HandZone.Any(p => p is ICharacter && ((ICharacter)p).IsRace(Race.DRAGON)));

		public static readonly SelfCondition Is5PlusAtkInHand
			= new SelfCondition(me => me.Controller.HandZone.Any(p => p is ICharacter && ((ICharacter)p).AttackDamage >= 5));

		public static readonly SelfCondition Has5PlusCostSpellInHand
			= new SelfCondition(me => me.Controller.HandZone.Any(p => p.Card.Type == CardType.SPELL && p.Cost >= 5));

		public static SelfCondition IsRace(Race race)
		{
			return new SelfCondition(me => me is ICharacter && ((ICharacter)me).IsRace(race));
		}

		public static SelfCondition IsNotRace(Race race)
		{
			return new SelfCondition(me => me.GetType() == typeof(ICharacter) && !(me as ICharacter).IsRace(race));
		}

		public static readonly SelfCondition IsMinion =
			new SelfCondition(me => me is Minion);

		public static readonly SelfCondition IsSpell =
			new SelfCondition(me => me is Spell);

		public static readonly SelfCondition IsSecret =
			new SelfCondition(me => me.Card.IsSecret);

		public static readonly SelfCondition IsWeapon =
			new SelfCondition(me => me is Weapon);

		public static readonly SelfCondition IsWeaponEquipped =
			new SelfCondition(me => me.Controller.Hero.Weapon != null);

		public static readonly SelfCondition IsHero =
			new SelfCondition(me => me is Hero);

		public static readonly SelfCondition IsHeroPower =
			new SelfCondition(me => me is HeroPower);

		public static readonly SelfCondition IsHeroPowerTargetingMinion
			= new SelfCondition(me => me.Card.Type == CardType.HERO_POWER && me.Game.CurrentEventData.EventTarget.Card.Type == CardType.MINION);

		public static SelfCondition HasArmorLessThan(int amount)
		{
			return new SelfCondition(me => me.Controller.Hero.Armor < amount);
		}

		//public static readonly SelfCondition IsAttacking =
		//	new SelfCondition(me => me is ICharacter && ((ICharacter)me).IsAttacking);

		//public static readonly SelfCondition IsCthun =
		//	new SelfCondition(me => me.Card.Id.Equals("OG_280"));

		public static readonly SelfCondition IsSilverHandRecruit =
			new SelfCondition(me => me.Card.Id.Equals("CS2_101t"));  // Added Race.SILVERHAND_RECRUIT

		public static readonly SelfCondition IsTreant =
			new SelfCondition(me => me.Card.Name == "Treant");  // Added Race.TREANT

		public static readonly SelfCondition IsControllingTreant =
			new SelfCondition(me => me.Controller.BoardZone.Any(m => m.Card.Name == "Treant"));

		public static readonly SelfCondition IsControllingLackey =
			new SelfCondition(me => me.Controller.BoardZone.Any(m => m.Card[GameTag.MARK_OF_EVIL] == 1));  // Added Race.LACKEY

		public static readonly SelfCondition IsSpellDmgOnHero =
			new SelfCondition(me => me.Controller.CurrentSpellPower > 0);

		public static readonly SelfCondition IsntSpellDmgOnHero =
			new SelfCondition(me => me.Controller.CurrentSpellPower == 0);

		//public static SelfCondition IsNotAttackingThisTurn(int number)
		//{
		//	return new SelfCondition(me =>
		//	me.GetType() == typeof(ICharacter) &&
		//	(me as ICharacter).NumAttacksThisTurn == number);
		//}

		public static SelfCondition IsCardId(string cardId)
		{
			return new SelfCondition(me => me.Card.Id == cardId);
		}


		public static SelfCondition IsNotCardClass(CardClass cardClass)
		{
			return new SelfCondition(me => me.Card.Class != cardClass);
		}

		public static SelfCondition IsNotStartInDeck =
			new SelfCondition(me => me.Id > (me.Controller.Deck.Count + me.Controller.Opponent.Deck.Count + 7));

		public static SelfCondition MinionsPlayedThisTurn(int number)
		{
			return new SelfCondition(me => me.Controller.NumMinionsPlayedThisTurn == number && me.Controller == me.Game.CurrentPlayer);
		}

		public static SelfCondition SpellsPlayedThisTurn(int number)
		{
			return new SelfCondition(me => me.Controller.CardsPlayedThisTurn.Count(card => card.Type == CardType.SPELL) == number);
		}

		public static SelfCondition ElementalPlayedLastTurn =>
			new SelfCondition(me => me.Controller.NumElementalsPlayedLastTurn > 0);

		public static SelfCondition HasMinionInDeck()
		{
			return new SelfCondition(me => me.Controller.DeckZone.Any(p => p is Minion));
		}

		public static SelfCondition HasMinionInDeck(GameTag tag)
		{
			return new SelfCondition(me => me.Controller.DeckZone.Any(p => p is Minion && p[tag] > 0));
		}

		public static readonly SelfCondition HasSpellInDeck =
			new SelfCondition(me => me.Controller.DeckZone.Any(p => p is Spell));

		public static readonly SelfCondition IsNoDupeInDeck =
			new SelfCondition(me => !me.Controller.DeckZone.GroupBy(x => new { x.Card.Id })
                                                           .Any(x => x.Skip(1).Any()));  // Added GameTag.NO_DUPE_IN_ZONE

		public static SelfCondition HasNoSpecficCostCardsInDeck(int cost)
		{
			return new SelfCondition(me => !me.Controller.DeckZone.Any(x => x.Cost == cost));
		}

		public static readonly SelfCondition HasNoMinionInDeck =
			new SelfCondition(me => !me.Controller.DeckZone.Any(p => p is Minion));

		public static readonly SelfCondition HasNoOddCostInDeck =
			new SelfCondition(me => me.Controller.DeckZone.NoOddCostCards);

		public static readonly SelfCondition HasNoEvenCostInDeck =
			new SelfCondition(me => me.Controller.DeckZone.NoEvenCostCards);

		public static readonly SelfCondition HasMinionInHand =
			new SelfCondition(me => me.Controller.HandZone.Any(p => p is Minion));

		//public static readonly SelfCondition HasMyHeroAttackedThisTurn =
		//	new SelfCondition(me => me.Controller.Hero.NumAttacksThisTurn > 0);

		//public static readonly SelfCondition HasMyHeroNotAttackedThisTurn =
		//	new SelfCondition(me => me.Controller.Hero.NumAttacksThisTurn == 0);

		public static readonly SelfCondition IsMyHeroDamagedThisTurn =
			new SelfCondition(me => me.Controller.Hero.DamageTakenThisTurn > 0);

		public static readonly SelfCondition IsDeathrattleCard =
			new SelfCondition(me => me.Card.Deathrattle);

		public static readonly SelfCondition IsEchoCard =
			new SelfCondition(me => me.Card.Echo);

		public static readonly SelfCondition IsComboCard =
			new SelfCondition(me => me.Card.Combo);

		public static readonly SelfCondition IsLifestealCard =
			new SelfCondition(me => me.Card.LifeSteal);

		public static readonly SelfCondition IsDeathrattleMinion =
			new SelfCondition(me => me is Minion && ((Minion)me).HasDeathrattle);

		public static readonly SelfCondition IsBattlecryMinion =
			new SelfCondition(me => me is Minion && ((Minion)me).HasBattlecry);

		public static readonly SelfCondition IsRushMinion =
			new SelfCondition(me => me is Minion && ((Minion)me).IsRush);

		public static readonly SelfCondition IsCthunDead =
			new SelfCondition(me => me.Controller.GraveyardZone.Any(p => p.Card.Id.Equals("OG_280")));

		public static readonly SelfCondition NotPlayedAnySpellThisTurn =
			new SelfCondition(me => me.Controller.CardsPlayedThisTurn.All(p => p.Type != CardType.SPELL));

		//public static SelfCondition NumSpellPlayedThisturn


		// entities that don't have a real zone like Heroes are checked on the gametag value
		public static SelfCondition IsInZone(Zone zone)
		{
			int value = 0;

			return new SelfCondition(me => me.Zone != null ? me.Zone.Type == zone :
            me.NativeTags.TryGetValue(GameTag.ZONE, out value) && (Zone)value == zone);
		}

		public static readonly SelfCondition IsOverloadCard =
			new SelfCondition(me => me.Card.HasOverload);

		public static readonly SelfCondition IsBattleCryCard =
			new SelfCondition(me => me.Card.Tags.ContainsKey(GameTag.BATTLECRY));

		public static readonly SelfCondition IsChooseOneCard =
			new SelfCondition(me => me.Card.ChooseOne);

		public static readonly SelfCondition IsTauntMinion =
			new SelfCondition(me => me is Minion && ((Minion)me).HasTaunt);

		public static readonly SelfCondition IsFrozen =
			new SelfCondition(me => me is ICharacter && ((ICharacter)me).IsFrozen);

		public static SelfCondition IsHeroPowerCard(string cardId)
		{
			return new SelfCondition(me => me.Controller.Hero.HeroPower.Card.Id.Equals(cardId));
		}

		public static readonly SelfCondition IsManaCrystalFull =
			new SelfCondition(me => me.Controller.BaseMana == 10);

		public static readonly SelfCondition IsRemaningManaFull =
			new SelfCondition(me => me.Controller.RemainingMana == 10);

		public static readonly SelfCondition IsNotDead =
			new SelfCondition(me => me is ICharacter && !me.ToBeDestroyed);

		public static readonly SelfCondition IsNotUntouchable =
			new SelfCondition(me => !me.Card.Untouchable);

		public static readonly SelfCondition IsNotSilenced =
			new SelfCondition(me => me is ICharacter && !((ICharacter)me).IsSilenced);

		public static readonly SelfCondition IsNotBoardFull =
			new SelfCondition(me => !me.Controller.BoardZone.IsFull);

		//public static readonly SelfCondition IsDurabilityOkay =
		//	new SelfCondition(me => me is Weapon && ((Weapon)me).Durability > 0);

		//public static readonly SelfCondition IsAnyNotImmune =
		//	new SelfCondition(me => me.Game.Characters.Exists(p => !p.IsImmune));

		public static readonly SelfCondition IsOpNotBoardFull =
			new SelfCondition(me => !me.Controller.Opponent.BoardZone.IsFull);

		public static readonly SelfCondition IsOpTurn =
			new SelfCondition(me => me.Controller != me.Game.CurrentPlayer);

		public static readonly SelfCondition IsMyTurn =
			new SelfCondition(me => me.Controller == me.Game.CurrentPlayer);

		//public static readonly SelfCondition IsSecretOrQuestActive =
		//	new SelfCondition(me => me.Zone.Type == Zone.SECRET);

		//public static readonly SelfCondition IsQuestDone =
		//	new SelfCondition(me => me[GameTag.QUEST_PROGRESS] == me[GameTag.QUEST_PROGRESS_TOTAL]);

		public static readonly SelfCondition IsSpellTargetingMinion =
			new SelfCondition(me =>
            me.Card.Type == CardType.SPELL && me.Game.IdEntityDic[me.CardTarget].Card.Type == CardType.MINION);

		public static readonly SelfCondition HoldingAnotherClassCard =
			new SelfCondition(me => me.Controller.HandZone.Any(p => p.Card.Class != me.Controller.HeroClass));

		//public static SelfCondition IsProposedDefender(CardType cardType) => new SelfCondition(me => me is ICharacter && me.Game.IdEntityDic[me.Game.ProposedDefender].Card.Type == cardType);
		public static SelfCondition IsProposedDefender(CardType cardType)
		{
			return IsEventTargetIs(cardType);
		}

		public static readonly SelfCondition HasLessHandCardsThanOp =
			new SelfCondition(me => me.Controller.HandZone.Count < me.Controller.Opponent.HandZone.Count);

		public static readonly SelfCondition IsAnyDiedThisTurn =
			new SelfCondition(p => p.Controller.NumFriendlyMinionsThatDiedThisTurn + p.Controller.Opponent.NumFriendlyMinionsThatDiedThisTurn > 0);

		public static readonly SelfCondition DoesOpHaveMoreMinions =
			new SelfCondition(me => me.Controller.BoardZone.CountExceptUntouchables < me.Controller.Opponent.BoardZone.CountExceptUntouchables);

		public static readonly SelfCondition HasTarget =
			new SelfCondition(me => me.CardTarget > 0);

		public static SelfCondition AnyNonClassCardInHand(CardClass cardClass)
		{
			return new SelfCondition(me => me.Controller.HandZone.Any(p => p.Card.Class != cardClass));
		}

		public static SelfCondition IsZoneCount(Zone zone, int amount, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me =>
                                relaSign == RelaSign.EQ && me.Controller.ControlledZones[zone].Count == amount
                             || relaSign == RelaSign.GEQ && me.Controller.ControlledZones[zone].Count >= amount
                             || relaSign == RelaSign.LEQ && me.Controller.ControlledZones[zone].Count <= amount);
		}

		public static SelfCondition IsOpZoneCount(Zone zone, int amount, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me =>
                                relaSign == RelaSign.EQ && me.Controller.Opponent.ControlledZones[zone].Count == amount
                             || relaSign == RelaSign.GEQ && me.Controller.Opponent.ControlledZones[zone].Count >= amount
                             || relaSign == RelaSign.LEQ && me.Controller.Opponent.ControlledZones[zone].Count <= amount);
		}

		public static SelfCondition HasBoardMinion(GameTag tag, int amount, RelaSign relaSign = RelaSign.EQ)
		{
			var func = new Func<IPlayable, bool>(me =>
			{
				return relaSign == RelaSign.EQ && me.Controller.BoardZone.Any(p => GetTagValue(p, tag) == amount)
					   || relaSign == RelaSign.GEQ && me.Controller.BoardZone.Any(p => GetTagValue(p, tag) >= amount)
					   || relaSign == RelaSign.LEQ && me.Controller.BoardZone.Any(p => GetTagValue(p, tag) <= amount);
			});

			return new SelfCondition(me => func(me));
		}

		public static SelfCondition HasOpBoardMinion(GameTag tag, int amount, RelaSign relaSign = RelaSign.EQ)
		{
			var func = new Func<IPlayable, bool>(me =>
			relaSign == RelaSign.EQ && me.Controller.Opponent.BoardZone.Any(p => GetTagValue(p, tag) == amount) ||
			relaSign == RelaSign.GEQ && me.Controller.Opponent.BoardZone.Any(p => GetTagValue(p, tag) >= amount) ||
			relaSign == RelaSign.LEQ && me.Controller.Opponent.BoardZone.Any(p => GetTagValue(p, tag) <= amount));

			return new SelfCondition(me => func(me));
		}

		public static SelfCondition HasOp(GameTag tag, int amount, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me =>
                               relaSign == RelaSign.EQ &&
                               (me.Controller.Opponent.BoardZone.Any(p => p[tag] == amount)
                                || me.Controller.Opponent.Hero[tag] == amount)
                            || relaSign == RelaSign.GEQ &&
                               me.Controller.Opponent.BoardZone.Any(p => p[tag] >= amount
                                || me.Controller.Opponent.Hero[tag] >= amount)
                            || relaSign == RelaSign.LEQ &&
                               me.Controller.Opponent.BoardZone.Any(p => p[tag] <= amount
                                || me.Controller.Opponent.Hero[tag] <= amount));
		}

		public static SelfCondition IsCost(int value, RelaSign relaSign = RelaSign.EQ)
		{
			var func = new Func<IPlayable, bool>(me =>
			{
				int val = me.Cost;

				return relaSign == RelaSign.EQ && val == value
					   || relaSign == RelaSign.GEQ && val >= value
					   || relaSign == RelaSign.LEQ && val <= value;
			});

			return new SelfCondition(me => func(me));
		}

		public static SelfCondition IsTagValue(GameTag tag, int value, RelaSign relaSign = RelaSign.EQ)
		{
			var func = new Func<IPlayable, bool>(me =>
			{
				int val = GetTagValue(me, tag);

				return relaSign == RelaSign.EQ && val == value
					   || relaSign == RelaSign.GEQ && val >= value
					   || relaSign == RelaSign.LEQ && val <= value;
			});

			return new SelfCondition(me => func(me));
		}

		public static SelfCondition IsBaseTagValue(GameTag tag, int value, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me =>
                                relaSign == RelaSign.EQ && me.Card[tag] == value
                             || relaSign == RelaSign.GEQ && me.Card[tag] >= value
                             || relaSign == RelaSign.LEQ && me.Card[tag] <= value);
		}

		public static SelfCondition IsCthunGameTag(GameTag tag, int value, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me =>
            {
                if (!me.Controller.SeenCthun)
                    return false;

                IPlayable proxyCthun = me.Game.IdEntityDic[me.Controller.ProxyCthun];
                int val = GetTagValue(proxyCthun, tag);

                return relaSign == RelaSign.EQ && val == value
                       || relaSign == RelaSign.GEQ && val >= value
                       || relaSign == RelaSign.LEQ && val <= value;
            });
		}

		public static SelfCondition IsHealth(int value, RelaSign relaSign)
		{
			return new SelfCondition(me => relaSign == RelaSign.EQ && me is ICharacter && ((ICharacter)me).Health == value
                                                || relaSign == RelaSign.GEQ && me is ICharacter && ((ICharacter)me).Health >= value
                                                || relaSign == RelaSign.LEQ && me is ICharacter && ((ICharacter)me).Health <= value);
		}

		public static SelfCondition IsBoardCount(int value, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me => relaSign == RelaSign.EQ && me.Controller.BoardZone.CountExceptUntouchables == value
                                                || relaSign == RelaSign.GEQ && me.Controller.BoardZone.CountExceptUntouchables >= value
                                                || relaSign == RelaSign.LEQ && me.Controller.BoardZone.CountExceptUntouchables <= value);
		}

		public static SelfCondition IsOpBoardCount(int value, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(me => relaSign == RelaSign.EQ && me.Controller.Opponent.BoardZone.Count == value
                                                || relaSign == RelaSign.GEQ && me.Controller.Opponent.BoardZone.Count >= value
                                                || relaSign == RelaSign.LEQ && me.Controller.Opponent.BoardZone.Count <= value);
		}

		public static readonly SelfCondition HasProperTargetsInBoard =
			new SelfCondition(me => !me.Card.MustHaveTargetToPlay || me.HasAnyValidPlayTargets);

		public static readonly SelfCondition IsHeroLethalPreDamaged
			= new SelfCondition(me => me is Hero hero && hero.Game.CurrentEventData.EventNumber >= hero.Health);

		public static SelfCondition IsCurrentEventNumber(int value, RelaSign relaSign)
		{
			return new SelfCondition(p => relaSign == RelaSign.EQ ? p.Game.CurrentEventData.EventNumber == value :
                relaSign == RelaSign.GEQ ? p.Game.CurrentEventData.EventNumber >= value :
                p.Game.CurrentEventData.EventNumber <= value);
		}
		public static SelfCondition IsEventTargetIs(CardType type)
		{
			return new SelfCondition(p => p.Game.CurrentEventData?.EventTarget.Card.Type == type);
		}
		public static SelfCondition IsEventTargetTagValue(GameTag tag, int value, RelaSign relaSign = RelaSign.EQ)
		{
			return new SelfCondition(p => relaSign == RelaSign.EQ ? p.Game.CurrentEventData.EventTarget?[tag] == value :
                relaSign == RelaSign.GEQ ? p.Game.CurrentEventData.EventTarget?[tag] >= value :
                p.Game.CurrentEventData.EventTarget?[tag] <= value);
		}

		public static SelfCondition CheckThreshold(RelaSign relaSign)
		{
			return new SelfCondition(me =>
            {
                int currentValue = me.Controller[(GameTag)me.Card[GameTag.PLAYER_TAG_THRESHOLD_TAG_ID]];
                int threshold = me.Card[GameTag.PLAYER_TAG_THRESHOLD_VALUE];

                return relaSign == RelaSign.GEQ ? currentValue >= threshold
                    : relaSign == RelaSign.EQ ? currentValue == threshold
                    : currentValue <= threshold;
            });
		}

		public static readonly SelfCondition IsEventSourceFriendly =
            new SelfCondition(p => p.Game.CurrentEventData.EventSource.Controller == p.Controller);

		public static readonly SelfCondition IsDefenderDead =
			new SelfCondition(p => p.Game.CurrentEventData?.EventTarget.ToBeDestroyed ?? false);

		public static readonly SelfCondition IsDefenderNotDead =
			new SelfCondition(p => !p.Game.CurrentEventData?.EventTarget.ToBeDestroyed ?? false);

		public static SelfCondition IsStep(Step step)
		{
			return new SelfCondition(me => me.Game.Step == step);
		}
	}

	/// <summary>
	/// Constructor, Properties, Members, and Helpers
	/// </summary>
	public partial class SelfCondition
	{
        readonly Func<IPlayable, bool> _function;

		public SelfCondition(Func<IPlayable, bool> function)
        {
			_function = function;
			//var owner = (_function.Body as BinaryExpression).Right as IPlayable;
			//owner.Game.Log(Model.LogLevel.INFO, BlockType.PLAY, Environment.StackTrace,
			//	$" ##### CONDITION INIT #####\n\t{owner.Card.Name} created {GetType().Name}\n");

			//SelfConditionTypes.AddRange(types);
		}

		public bool Eval(IPlayable owner)
		{
            //bool result = _function(owner);
			//Vector.Add(Convert.ToInt32(result));
			return _function(owner);
		}

		public static SelfCondition operator +(SelfCondition a, SelfCondition b) =>
            new SelfCondition(a._function + b._function);

		static int GetTagValue(IPlayable me, GameTag tag)
        {
            if (tag == GameTag.COST)
                return me.Cost;

            if (me is Character c)
            {
                if (tag == GameTag.ATK)
                    return c.AttackDamage;
                else if
                    (tag == GameTag.HEALTH)
                    return c.BaseHealth;
                else
                    return c[tag];
            }
            else
                return me[tag];
        }

		#region Vectorization Methods (UNUSED FOR NOW)
		//      #region Vectorize
		//      static Dictionary<string, int> Vectorize(RelaSign r1, GameTag tag1)
		//      {
		//          return new List<int>
		//          {
		//              (int)r1,
		//              (int)tag1
		//          };
		//      }

		//      //private static Dictionary<string, int> Vectorize(RelaSign r1, int tag1)
		//      //{
		//      //	return new List<int>
		//      //	{
		//      //		(int)r1,
		//      //		tag1
		//      //	};
		//      //}

		//      static Dictionary<string, int> Vectorize(GameTag tag1, RelaSign r, int tag1Value, GameTag tag2)
		//      {
		//          return new List<int>
		//          {
		//              (int)tag1,
		//              (int)r,
		//              tag1Value,
		//              (int)tag2
		//          };
		//      }

		//      //private static Dictionary<string, int> Vectorize(RelaSign r1, GameTag tag1, Conj conj, GameTag tag2, CardType c)
		//      //{
		//      //	return new List<int>
		//      //	{
		//      //		(int)r1,
		//      //		(int)tag1,
		//      //		(int)conj,
		//      //		(int)tag2,
		//      //		(int)c
		//      //	};
		//      //}

		//      static Dictionary<string, int> Vectorize(RelaSign r1, GameTag tag1, Zone zone, GameTag tag2)
		//      {
		//          return new List<int>
		//          {
		//              (int)r1,
		//              (int)tag1,
		//              (int)zone,
		//              (int)tag2
		//          };
		//      }

		//      static Dictionary<string, int> Vectorize(RelaSign r, GameTag tag, Zone zone, GRVs grvs)
		//      {
		//          var l = new List<int>
		//          {
		//              (int)r,
		//              (int)tag,
		//              (int)zone,
		//          };

		//          foreach (GRV grv in grvs)
		//          {
		//              l.Add((int)grv.Item1);
		//              l.Add((int)grv.Item2);
		//              l.Add(grv.Item3);
		//          }

		//          return l;
		//      }

		//      //static Dictionary<string, int> Vectorize(RelaSign r, GameTag tag, Zone zone, GRVls grvls)
		//      //{
		//      //    var l = new List<int>
		//      //    {
		//      //        (int)r,
		//      //        (int)tag,
		//      //        (int)zone,
		//      //    };

		//      //    foreach (GRVl grvl in grvls)
		//      //    {
		//      //        l.Add((int)grvl.Item1);
		//      //        l.Add((int)grvl.Item2);
		//      //        l.Add(grvl.Item3.Invoke((_function.Body as BinaryExpression).Right as IPlayable));
		//      //    }

		//      //    return l;
		//      //}

		//      static Dictionary<string, int> Vectorize(RelaSign r1, GameTag tag, Zone zone, RelaSign r2, int value)
		//      {
		//          return new List<int>
		//          {
		//              (int)r1,
		//              (int)tag,
		//              (int)zone,
		//              (int)r2,
		//              value
		//          };
		//      }

		//      static Dictionary<string, int> Vectorize(GameTag tag, RelaSign r, int value)
		//      {
		//          return new List<int>
		//          {
		//              (int)tag,
		//              (int)r,
		//              value
		//          };
		//      }

		//      static Dictionary<string, int> Vectorize(GameTag tag, RelaSign r, int value, GRVs grvs)
		//      {
		//          var l = new List<int>
		//          {
		//              (int)tag,
		//              (int)r,
		//              value
		//          };

		//          foreach (GRV grv in grvs)
		//          {
		//              l.Add((int)grv.Item1);
		//              l.Add((int)grv.Item2);
		//              l.Add(grv.Item3);
		//          }

		//          return l;
		//      }

		//      static Dictionary<string, int> Vectorize(RelaSign r, GameTag tag, GRVs grvs)
		//      {
		//          var l = new List<int>
		//          {
		//              (int)r,
		//              (int)tag,
		//          };

		//          foreach (GRV grv in grvs)
		//          {
		//              l.Add((int)grv.Item1);
		//              l.Add((int)grv.Item2);
		//              l.Add(grv.Item3);
		//          }

		//          return l;
		//      }

		//      static Dictionary<string, int> Vectorize(RelaSign r, GameTag tag, Zone zone, GRV grv, RG rg)
		//      {
		//          var l = new List<int>
		//          {
		//              (int)r,
		//              (int)tag,
		//              (int)zone
		//          };

		//          l.Add((int)grv.Item1);
		//          l.Add((int)grv.Item2);
		//          l.Add(grv.Item3);

		//          l.Add((int)rg.Item1);
		//          l.Add((int)rg.Item2);

		//          return l;
		//      }

		//      //      private static List<SelfConditionType> GetConditionTypes(GameTag tag, int value, RelaSign relaSign)
		////{
		////	if (tag == GameTag.ATK)
		////	{
		////		if (relaSign == RelaSign.EQ)
		////		{
		////			return value == 1 ? new List<SelfConditionType> { SelfConditionType.IS_ATTACK_1 } :
		////			value == 2 ? new List<SelfConditionType> { SelfConditionType.IS_ATTACK_2 } :
		////			value == 3 ? new List<SelfConditionType> { SelfConditionType.IS_ATTACK_3 } :
		////			new List<SelfConditionType> { SelfConditionType.UNUSED };
		////		}

		////		else if (relaSign == RelaSign.LEQ)
		////		{
		////			return value == 2 ?
		////				new List<SelfConditionType> { SelfConditionType.IS_ATTACK_2_OR_LESS } :
		////				new List<SelfConditionType> { SelfConditionType.UNUSED };
		////		}

		////		else if (relaSign == RelaSign.GEQ)
		////		{
		////			return value == 3 ? new List<SelfConditionType> { SelfConditionType.IS_ATTACK_3_OR_MORE } :
		////				value == 4 ? new List<SelfConditionType> { SelfConditionType.IS_ATTACK_4_OR_MORE } :
		////				value == 5 ? new List<SelfConditionType> { SelfConditionType.IS_ATTACK_5_OR_MORE } :
		////				new List<SelfConditionType> { SelfConditionType.UNUSED };
		////		}
		////	}

		////	else if (tag == GameTag.COST)
		////	{
		////		if (relaSign == RelaSign.EQ)
		////		{
		////			return value == 1 ? new List<SelfConditionType> { SelfConditionType.IS_COST_1 } :
		////				value == 7 ? new List<SelfConditionType> { SelfConditionType.IS_COST_7 } :
		////				value == 8 ? new List<SelfConditionType> { SelfConditionType.IS_COST_8 } :
		////				value == 9 ? new List<SelfConditionType> { SelfConditionType.IS_COST_9 } :
		////				value == 10 ? new List<SelfConditionType> { SelfConditionType.IS_COST_10 } :
		////				new List<SelfConditionType> { SelfConditionType.UNUSED };
		////		}

		////		else if (relaSign == RelaSign.LEQ)
		////		{
		////			return value == 2 ? new List<SelfConditionType> { SelfConditionType.IS_COST_2_OR_LESS } :
		////				value == 3 ? new List<SelfConditionType> { SelfConditionType.IS_COST_3_OR_LESS } :
		////				value == 4 ? new List<SelfConditionType> { SelfConditionType.IS_COST_4_OR_LESS } :
		////				new List<SelfConditionType> { SelfConditionType.UNUSED };
		////		}

		////		else if (relaSign == RelaSign.GEQ)
		////		{
		////			return value == 5 ?
		////				new List<SelfConditionType> { SelfConditionType.IS_COST_5_OR_MORE } :
		////				new List<SelfConditionType> { SelfConditionType.UNUSED };
		////		}
		////	}


		////	return tag == GameTag.TAUNT ? SelfConditionType.IS_CONTROLLING_TAUNT_MINION :
		////		tag == GameTag.ATK && value == 5 && relaSign == RelaSign.GEQ ? SelfConditionType.IS_CONTROLLING_MINION_5_OR_MORE_ATTACK :
		////		tag == GameTag.HEALTH && value == 6 && relaSign == RelaSign.GEQ ? SelfConditionType.IS_CONTROLLING_MINION_6_OR_MORE_HEALTH :
		////		tag == GameTag.SPELLPOWER ? SelfConditionType.IS_CONTROLLING_SPELL_DAMAGE_MINION :
		////		tag == GameTag.COST && value == 2 && relaSign == RelaSign.LEQ ? SelfConditionType.IS_COST_2_OR_LESS :
		////		tag == GameTag.COST && value == 3 && relaSign == RelaSign.LEQ ? SelfConditionType.IS_COST_3_OR_LESS :
		////		tag == GameTag.COST && value == 4 && relaSign == RelaSign.LEQ ? SelfConditionType.IS_COST_4_OR_LESS :
		////		tag == GameTag.COST && value == 5 && relaSign == RelaSign.GEQ ? SelfConditionType.IS_COST_5_OR_MORE :
		////		tag == GameTag.ATK && value == 1 && relaSign == RelaSign.EQ ? SelfConditionType.IS_ATTACK_1 :
		////		tag == GameTag.ATK && value == 2 && relaSign == RelaSign.EQ ? SelfConditionType.IS_ATTACK_2 :
		////		tag == GameTag.ATK && value == 2 && relaSign == RelaSign.LEQ ? SelfConditionType.IS_ATTACK_2_OR_LESS :
		////		SelfConditionType.UNUSED;
		////}

		//#endregion
		#endregion
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
