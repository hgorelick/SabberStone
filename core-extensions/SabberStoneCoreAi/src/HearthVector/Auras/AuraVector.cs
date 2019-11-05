using SabberStoneCore.Auras;
using SabberStoneCoreAi.HearthVector.Enchants;
using SabberStoneCoreAi.HearthVector.Model;
using SabberStoneCoreAi.HearthVector.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Auras
{
	public class AuraVector : HearthVector
	{
		public int AuraType { get; private set; }
		//public bool ConditionsMet { get; private set; }
		public EffectVector[] Effects { get; private set; }
		public int EnchantmentAssetId { get; private set; }
		public bool On { get; private set; }
		public CardVector Owner { get; private set; }

		public static AuraVector Create(IAura aura)
		{
			if (aura == null)
				return new AuraVector();
			return new AuraVector(aura);
		}

		public static List<AuraVector> Create(List<IAura> auras = null)
		{
			if (auras == null)
				return new List<AuraVector>() { new AuraVector() };

			var auraVectors = new List<AuraVector>();
			for (int i = 0; i < auras.Count; ++i)
				auraVectors.Add(Create(auras[i]));

			return auraVectors;
		}

		private AuraVector(IAura aura)
		{
			AuraType = (int)((Aura)aura).Type;
			Effects = EffectVector.Create(((Aura)aura).Effects);
			EnchantmentAssetId =((Aura)aura).EnchantmentCard.AssetId;
			On = ((Aura)aura).On;
			Owner = CardVector.Create(((Aura)aura).Owner);
		}

		public AuraVector() : base()
		{
			AuraType = 0;
			Effects = new EffectVector[] { new EffectVector() };
			EnchantmentAssetId = 0;
			On = false;
			Owner = new CardVector();
		}
	}
}
