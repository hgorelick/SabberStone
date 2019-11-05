using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RevealStealthTaskVector : SimpleTaskVector
	{
		public int Tag { get; private set; } = (int)GameTag.STEALTH;
		public int Type { get; private set; }

		public RevealStealthTaskVector(RevealStealthTask revealStealthTask) : base(revealStealthTask)
		{
			Type = (int)revealStealthTask.Type;
		}
	}
}
