using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class MoveToDeckVector : SimpleTaskVector
	{
		public int MoveToZone { get; private set; }
		public bool Opponent { get; private set; }
		public int Type { get; private set; }

		public MoveToDeckVector(MoveToDeck moveToDeck) : base(moveToDeck)
		{
			MoveToZone = (int)Zone.DECK;
			Opponent = moveToDeck.Opponent;
			Type = (int)moveToDeck.Type;
		}
	}
}
