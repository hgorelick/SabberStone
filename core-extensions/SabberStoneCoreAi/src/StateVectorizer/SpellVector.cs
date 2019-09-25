using System.Collections.Generic;

namespace SabberStoneCoreAi.StateVectorizer
{
	public class SpellVector : CardVector
	{
		public static readonly List<string> SpellKeys = new List<string>
		{
			"Damage",
			"Heal",
			"Armor",
			"Draw",
			"Return Destination",
			"Random",
			"Target Zone",
			"Single Target",
			"Hero Target",
			"Minion Target",
			"Weapon Target",
			"Self Target",
			"Opponent Target",
			"Attack Buff",
			"Health Buff",
			"Tags",
			"Summon What",
			"Summon Count",
			"Summon Zone",
			"Summon Tags"
		};
	}

	public class ConditionalSpellVector : SpellVector
	{
		public static readonly List<string> ConditionalSpellKeys = new List<string>
		{
			"Freeze Condition",
			"Death Condition",
			"Type Condition",
			"Tag Condition",
			"Attack Condition",
			"Health Condition",
			"Weapon Condition",
			"Cost Condition",
			"Hand Condition",
			"Deck Condition",
			"Draw Condition"
		};
	}
}
