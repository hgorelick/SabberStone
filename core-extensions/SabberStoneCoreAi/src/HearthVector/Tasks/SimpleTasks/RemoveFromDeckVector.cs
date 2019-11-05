using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RemoveFromDeckVector : SimpleTaskVector
	{
		public bool AddToStack { get; private set; }
		public int RemoveFromZone { get; private set; } = (int)Zone.DECK;
		public int Type { get; private set; }

		public RemoveFromDeckVector(RemoveFromDeck removeFromDeck) : base(removeFromDeck)
		{
			AddToStack = removeFromDeck.AddToStack;
			Type = (int)removeFromDeck.Type;
		}
	}
}
