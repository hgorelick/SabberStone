using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public class CharacterVector : PersistentPlayableVector
	{
		public int BaseHealth { get; private set; } = 0;
		public bool CantAttack { get; private set; } = false;
		public bool CantAttackHeroes { get; private set; } = false;
		public bool CantBeTargetedByHeroPowers { get; private set; } = false;
		public bool CantBeTargetedByOpponents { get; private set; } = false;
		public bool CantBeTargetedBySpells { get; private set; } = false;
		public bool HasAnyValidAttackTargets { get; private set; } = false;
		public int Health { get; private set; } = 0;
		public bool Frozen { get; private set; } = false;
		public int NumAttacksThisTurn { get; private set; } = 0;
		public bool Stealth { get; private set; } = false;

		public CharacterVector(Character c) : base(c)
		{
			BaseHealth = c.BaseHealth;
			CantAttack = c.CantAttack;
			CantAttackHeroes = c.CantAttackHeroes;
			CantBeTargetedByHeroPowers = c.CantBeTargetedByHeroPowers;
			CantBeTargetedByOpponents = c.CantBeTargetedByOpponents;
			CantBeTargetedBySpells = c.CantBeTargetedBySpells;
			HasAnyValidAttackTargets = c.HasAnyValidAttackTargets;
			Health = c.Health;
			Frozen = c.IsFrozen;
			NumAttacksThisTurn = c.NumAttacksThisTurn;
			Stealth = c.HasStealth;
		}

		public CharacterVector() : base() { }
	}

	public class HeroVector : CharacterVector
	{
		public int Armor { get; private set; } = 0;
		public int DamageTakenThisTurn { get; private set; } = 0;
		public int EquippedWeapon { get; private set; } = 0;
		public int ExtraAttacksThisTurn { get; private set; } = 0;
		public int Fatigue { get; private set; } = 0;
		public HeroPowerVector HeroPower { get; private set; }
		public WeaponVector Weapon { get; private set; }

		public HeroVector(Hero h) : base(h)
		{
			Armor = h.Armor;
			DamageTakenThisTurn = h.DamageTakenThisTurn;
			EquippedWeapon = h.EquippedWeapon;
			ExtraAttacksThisTurn = h.ExtraAttacksThisTurn;
			Fatigue = h.Fatigue;
			HeroPower = new HeroPowerVector(h.HeroPower);
			Weapon = new WeaponVector(h.Weapon);
		}

		public HeroVector() : base()
		{
			HeroPower = new HeroPowerVector();
			Weapon = new WeaponVector();
		}
	}

	public class MinionVector : CharacterVector
	{
		public bool Charge { get; private set; } = false;
		public bool DivineShield { get; private set; } = false;
		public bool Freeze { get; private set; } = false;
		public bool Inspire { get; private set; } = false;
		public bool IsEnraged { get; private set; } = false;
		public int LastBoardPosition { get; private set; } = 0;
		public bool Poisonous { get; private set; } = false;
		public bool Rush { get; private set; } = false;
		public bool Silenced { get; private set; } = false;
		public int SpellPower { get; private set; } = 0;
		public bool Taunt { get; private set; } = false;
		public bool Untouchable { get; private set; } = false;

		public MinionVector(Minion m) : base(m)
		{
			Charge = m.HasCharge;
			DivineShield = m.HasDivineShield;
			Freeze = m.Freeze;
			Inspire = m.HasInspire;
			IsEnraged = m.IsEnraged;
			LastBoardPosition = m.LastBoardPosition;
			Poisonous = m.Poisonous;
			Rush = m.IsRush;
			Silenced = m.IsSilenced;
			SpellPower = m.SpellPower;
			Taunt = m.HasTaunt;
			Untouchable = m.Untouchable;
		}

		public MinionVector() : base() { }
	}
}
