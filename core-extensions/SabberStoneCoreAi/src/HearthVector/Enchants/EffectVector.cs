using SabberStoneCore.Enchants;
using SabberStoneCore.Enums;
using SabberStoneCore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Enchants
{
	public class EffectVector : HearthVector
	{
		public int Operator { get; private set; }
		public int Tag { get; private set; }
		public int Value { get; private set; }

		public static EffectVector Create(IEffect effect)
		{
			if (effect == null)
				return new EffectVector();

			if (effect is Effect)
				return new EffectVector((Effect)effect);

			if (effect is GenericEffect<Cost, Playable>)
				return new EffectVector((GenericEffect<Cost, Playable>)effect);

			if (effect is GenericEffect<ATK, Playable>)
				return new EffectVector((GenericEffect<ATK, Playable>)effect);

			if (effect is GenericEffect<Health, Character>)
				return new EffectVector((GenericEffect<Health, Character>)effect);

			if (effect is GenericEffect<Taunt, Minion>)
				return new EffectVector((GenericEffect<Taunt, Minion>)effect);

			if (effect is GenericEffect<CantBeTargetedBySpells, Character>)
				return new EffectVector((GenericEffect<CantBeTargetedBySpells, Character>)effect);

			throw new NotImplementedException();
		}

		public static EffectVector[] Create(IEffect[] iEffects = null)
		{
			if (iEffects == null)
				return new EffectVector[] { new EffectVector() };

			var effects = new EffectVector[iEffects.Length];
			for (int i = 0; i < iEffects.Length; ++i)
				effects[i] = Create(iEffects[i]);
			return effects;
		}

		private EffectVector (Effect effect)
		{
			Operator = (int)effect.Operator;
			Tag = (int)effect.Tag;
			Value = effect.Value;
		}

		private EffectVector(GenericEffect<Cost, Playable> genericEffect)
		{
			Operator = (int)genericEffect.Operator;
			Tag = (int)GameTag.COST;
			Value = genericEffect.Value;
		}

		private EffectVector(GenericEffect<ATK, Playable> genericEffect)
		{
			Operator = (int)genericEffect.Operator;
			Tag = (int)GameTag.ATK;
			Value = genericEffect.Value;
		}

		private EffectVector(GenericEffect<Health, Character> genericEffect)
		{
			Operator = (int)genericEffect.Operator;
			Tag = (int)GameTag.HEALTH;
			Value = genericEffect.Value;
		}

		private EffectVector(GenericEffect<Taunt, Minion> genericEffect)
		{
			Operator = (int)genericEffect.Operator;
			Tag = (int)GameTag.TAUNT;
			Value = genericEffect.Value;
		}

		private EffectVector(GenericEffect<CantBeTargetedBySpells, Character> genericEffect)
		{
			Operator = (int)genericEffect.Operator;
			Tag = (int)GameTag.CANT_BE_TARGETED_BY_SPELLS;
			Value = genericEffect.Value;
		}

		public EffectVector()
		{
			Operator = -1;
			Tag = 0;
			Value = 0;
		}
	}
}
