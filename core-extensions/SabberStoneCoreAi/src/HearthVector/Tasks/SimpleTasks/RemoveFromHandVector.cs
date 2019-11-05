using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RemoveFromHandVector : SimpleTaskVector
	{
		public int RemoveFromZone { get; private set; } = (int)Zone.HAND;
		public int Type { get; private set; }

		public RemoveFromHandVector(RemoveFromHand removeFromHand) : base(removeFromHand)
		{
			Type = (int)removeFromHand.Type;
		}
	}
}
