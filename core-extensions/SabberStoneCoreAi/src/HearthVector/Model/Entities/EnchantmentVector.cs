using SabberStoneCore.Model.Entities;
using SabberStoneCoreAi.HearthVector.Auras;
using SabberStoneCoreAi.HearthVector.Enchants;
using SabberStoneCoreAi.HearthVector.Triggers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public class EnchantmentVector : EntityVector
	{
		public TriggerVector ActivatedTrigger { get; private set; }
		public int CapturedCardAssetId { get; private set; } = 0;
		public int CardAssetId { get; private set; } = 0;
		public int CreatorAssetId { get; private set; } = 0;
		public bool IsOneTurnActive { get; private set; } = false;
		public AuraVector OngoingEffect { get; private set; }
		public PowerVector Power { get; private set; }
		public int ScriptTagValue1 { get; private set; } = 0;
		public int ScriptTagValue2 { get; private set; } = 0;
		public int TargetAssetId { get; private set; } = 0;
		public int Zone { get; private set; } = 0;

		public static List<EnchantmentVector> Create(List<Enchantment> appliedEnchantments = null)
		{
			if (appliedEnchantments == null)
				return new List<EnchantmentVector>() { new EnchantmentVector() };

			var enchantList = new List<EnchantmentVector>();
			for (int i = 0; i < appliedEnchantments.Count; ++i)
				enchantList[i] = Create(appliedEnchantments[i]);

			return enchantList;
		}

		private static EnchantmentVector Create(Enchantment enchantment)
		{
			if (enchantment == null)
				return new EnchantmentVector();
			return new EnchantmentVector(enchantment);
		}

		private EnchantmentVector(Enchantment enchantment) : base (enchantment)
		{
			ActivatedTrigger = TriggerVector.Create(enchantment.ActivatedTrigger);
			CapturedCardAssetId = enchantment.CapturedCard.AssetId;
			CardAssetId = enchantment.Card.AssetId;
			CreatorAssetId = enchantment.Creator?.Card.AssetId ?? 0;
			IsOneTurnActive = enchantment.IsOneTurnActive;
			OngoingEffect = AuraVector.Create(enchantment.OngoingEffect);
			Power = PowerVector.Create(enchantment.Power);
			ScriptTagValue1 = enchantment.ScriptTagValue1;
			ScriptTagValue2 = enchantment.ScriptTagValue2;
			TargetAssetId = enchantment.Target?.Card.AssetId ?? 0;
			Zone = (int)enchantment.Zone.Type;
		}

		public EnchantmentVector() : base()
		{
			ActivatedTrigger = new TriggerVector();
			OngoingEffect = new AuraVector();
			Power = new PowerVector();
		}
	}
}
