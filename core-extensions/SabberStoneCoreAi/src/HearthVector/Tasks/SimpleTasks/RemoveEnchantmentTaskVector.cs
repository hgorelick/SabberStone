using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RemoveEnchantmentTaskVector : SimpleTaskVector
	{
		public int Tag { get; private set; } = (int)CardType.ENCHANTMENT;
		public RemoveEnchantmentTaskVector(RemoveEnchantmentTask removeEnchantmentTask) : base(removeEnchantmentTask) { }
		public RemoveEnchantmentTaskVector() : base() { }
	}
}
