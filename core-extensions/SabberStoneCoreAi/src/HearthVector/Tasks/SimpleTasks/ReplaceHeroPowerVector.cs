using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ReplaceHeroPowerVector : SimpleTaskVector
	{
		public int AssetId { get; private set; }

		public ReplaceHeroPowerVector(ReplaceHeroPower replaceHeroPower) : base(replaceHeroPower)
		{
			AssetId = replaceHeroPower.AssetId;
		}
	}
}
