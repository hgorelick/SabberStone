using SabberStoneCore.Enchants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Enchants
{
	public class AuraEffectsVector : HearthVector
	{
		public int ATK { get; private set; } = 0;
		//public bool CantAttack { get; private set; }
		public bool CantAttackHeroes { get; private set; } = false;
		public bool CantBeTargetedBySpells { get; private set; } = false;
		public bool CardCostHealth { get; private set; } = false;
		public int Charge { get; private set; } = 0;
		public bool Echo { get; private set; } = false;
		public int Health { get; private set; } = 0;
		public int HeroPowerDamage { get; private set; } = 0;
		public int Immune { get; private set; } = 0;
		public bool Lifesteal { get; private set; } = false;
		public bool Taunt { get; private set; } = false;
		public int Type { get; private set; } = 0;

		public static AuraEffectsVector Create(AuraEffects auraEffects)
		{
			if (auraEffects == null)
				return new AuraEffectsVector();
			return new AuraEffectsVector(auraEffects);
		}

		private AuraEffectsVector(AuraEffects auraEffects)
		{
			ATK = auraEffects.ATK;
			CantAttackHeroes = auraEffects.CantAttackHeroes;
			CantBeTargetedBySpells = auraEffects.CantBeTargetedBySpells;
			CardCostHealth = auraEffects.CardCostHealth;
			Charge = auraEffects.Charge;
			Echo = auraEffects.Echo;
			Health = auraEffects.Health;
			Immune = auraEffects.Immune;
			Lifesteal = auraEffects.Lifesteal;
			Taunt = auraEffects.Taunt;
			Type = (int)auraEffects.Type;
		}

		public AuraEffectsVector() : base() { }
	}

	public class ControllerAuraEffectsVector : HearthVector
	{
		public int AllHealingDouble { get; private set; } = 0;
		public int ChooseBoth { get; private set; } = 0;
		public int ExtraBattlecry { get; private set; } = 0;
		public int ExtraBattlecryAndCombo { get; private set; } = 0;
		public int ExtraEndTurnEffect { get; private set; } = 0;
		public int HeroPowerDisabled { get; private set; } = 0;
		public int HeroPowerDouble { get; private set; } = 0;
		public int RestoreToDamage { get; private set; } = 0;
		public int SpellPower { get; private set; } = 0;
		public int SpellPowerDouble { get; private set; } = 0;
		public int SpellsCostHealth { get; private set; } = 0;
		public int TimeOut { get; private set; } = 0;

		public static ControllerAuraEffectsVector Create(ControllerAuraEffects controllerAuraEffects)
		{
			if (controllerAuraEffects == null)
				return new ControllerAuraEffectsVector();
			return new ControllerAuraEffectsVector(controllerAuraEffects);
		}

		private ControllerAuraEffectsVector(ControllerAuraEffects controllerAuraEffects)
		{
			//var propNames = controllerAuraEffects.GetType().GetProperties().Select(p => p.Name).ToList()
			PropertyInfo[] props = controllerAuraEffects.GetType().GetProperties();
			foreach (PropertyInfo p in GetProperties())
			{
				if (props.Any(prop => prop.Name == p.Name))
					p.SetValue(this, props.Where(prop => prop.Name == p.Name).ToList()[0]);
			}
		}

		public ControllerAuraEffectsVector() : base() { }
	}
}
