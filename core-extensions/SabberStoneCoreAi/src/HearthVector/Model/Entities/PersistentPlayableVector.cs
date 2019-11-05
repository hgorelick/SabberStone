using SabberStoneCore.Model.Entities;
using SabberStoneCore.Enums;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public class PersistentPlayableVector : CardVector
	{
		public int Attack { get; private set; } = 0;
		public int AttackDamage { get; private set; } = 0;
		public bool Battlecry { get; private set; } = false;
		public int Damage { get; private set; } = 0;
		public bool Deathrattle { get; private set; } = false;
		public bool Exhausted { get; private set; } = false;
		public bool Immune { get; private set; } = false;
		public bool ToBeDestroyed { get; private set; } = false;
		public bool Windfury { get; private set; } = false;

		public PersistentPlayableVector(Playable p) : base(p)
		{
			AppliedEnchantments = EnchantmentVector.Create(p.AppliedEnchantments);
			Attack = p.Card.ATK;
			AttackDamage = p as Minion != null ? (p as Minion).AttackDamage :
				p as Hero != null ? (p as Hero).AttackDamage :
				(p as Weapon).AttackDamage;
			Battlecry = p.NativeTags.ContainsKey(GameTag.BATTLECRY);
			Damage = p as Minion != null ? (p as Minion).Damage :
				p as Hero != null ? (p as Hero).Damage :
				(p as Weapon).Damage;
			Deathrattle = p.HasDeathrattle;
			Exhausted = p.IsExhausted;
			Immune = p as Character != null ? (p as Character).IsImmune :
				(p as Weapon).IsImmune;
			Windfury = p as Character != null ? (p as Character).HasWindfury :
				(p as Weapon).IsWindfury;
		}

		public PersistentPlayableVector() : base() { }
	}

	public class WeaponVector : PersistentPlayableVector
	{
		public int Durability { get; private set; } = 0;
		public bool Poisonous { get; private set; } = false;

		public WeaponVector(Weapon w) : base(w)
		{
			Durability = w.Durability;
			Poisonous = w.Poisonous;
		}

		public WeaponVector() : base() { }
	}
}
