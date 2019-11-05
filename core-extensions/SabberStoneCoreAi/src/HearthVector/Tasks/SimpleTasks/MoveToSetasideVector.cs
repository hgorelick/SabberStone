using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class MoveToSetasideVector : SimpleTaskVector
	{
		public int MoveToZone { get; private set; }
		public int Type { get; private set; }

		public MoveToSetasideVector(MoveToSetaside moveToSetaside) : base(moveToSetaside)
		{
			MoveToZone = (int)Zone.SETASIDE;
			Type = (int)moveToSetaside.Type;
		}
	}
}
