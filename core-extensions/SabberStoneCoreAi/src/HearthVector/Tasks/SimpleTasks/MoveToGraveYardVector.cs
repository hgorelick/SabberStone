using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class MoveToGraveYardVector : SimpleTaskVector
	{
		public int MoveToZone { get; private set; }
		public int Type { get; private set; }

		public MoveToGraveYardVector(MoveToGraveYard moveToGraveYard) : base(moveToGraveYard)
		{
			MoveToZone = (int)Zone.GRAVEYARD;
			Type = (int)moveToGraveYard.Type;
		}
	}
}
