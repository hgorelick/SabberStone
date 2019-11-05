using SabberStoneCore.Enchants;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.HearthVector.Auras;
using SabberStoneCoreAi.HearthVector.Tasks;
using SabberStoneCoreAi.HearthVector.Triggers;

namespace SabberStoneCoreAi.HearthVector.Enchants
{
	public class PowerVector : HearthVector
	{
		public AuraVector Aura { get; private set; }
		public SimpleTaskVector[] ComboTask { get; private set; }
		public SimpleTaskVector[] DeathrattleTask { get; private set; }
		public EnchantVector Enchant { get; private set; }
		public SimpleTaskVector[] OverkillTask { get; private set; }
		public SimpleTaskVector[] PowerTask { get; private set; }
		public SimpleTaskVector[] TopDeckTask { get; private set; }
		public TriggerVector Trigger { get; private set; }

		public static PowerVector Create(Power power)
		{
			if (power == null)
				return new PowerVector();
			return new PowerVector(power);
		}

		private PowerVector(Power power)
		{
			Aura = AuraVector.Create(power.Aura);
			ComboTask = SimpleTaskVector.Create(power.ComboTask);
			DeathrattleTask = SimpleTaskVector.Create(power.DeathrattleTask);
			Enchant = EnchantVector.Create(power.Enchant);
			OverkillTask = SimpleTaskVector.Create(power.OverkillTask);
			PowerTask = SimpleTaskVector.Create(power.PowerTask);
			TopDeckTask = SimpleTaskVector.Create(power.TopdeckTask);
			Trigger = TriggerVector.Create(power.Trigger);
		}

		public PowerVector()
		{
			Aura = new AuraVector();
			ComboTask = SimpleTaskVector.Create();
			DeathrattleTask = SimpleTaskVector.Create();
			Enchant = new EnchantVector();
			OverkillTask = SimpleTaskVector.Create();
			PowerTask = SimpleTaskVector.Create();
			TopDeckTask = SimpleTaskVector.Create();
			Trigger = new TriggerVector();
		}
	}
}
