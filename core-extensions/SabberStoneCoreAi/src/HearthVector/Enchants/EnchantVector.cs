using SabberStoneCore.Enchants;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Enchants
{
	public class EnchantVector : HearthVector
	{
		public EffectVector[] Effects { get; private set; }
		public bool RemoveWhenPlayed { get; private set; } = false;
		public int ScriptTagValue1 { get; private set; } = 0;
		public int ScriptTagValue2 { get; private set; } = 0;
		public bool UseScriptTag { get; private set; } = false;

		public static EnchantVector Create(Enchant enchant)
		{
			if (enchant == null)
				return new EnchantVector();
			return new EnchantVector(enchant);
		}

		private EnchantVector(Enchant enchant)
		{
			Effects = EffectVector.Create(enchant.Effects);
			RemoveWhenPlayed = enchant.RemoveWhenPlayed;
			ScriptTagValue1 = enchant.ScriptTagValue1;
			ScriptTagValue2 = enchant.ScriptTagValue2;
			UseScriptTag = enchant.UseScriptTag;
		}

		public EnchantVector()
		{
			Effects = EffectVector.Create();
		}
	}
}
