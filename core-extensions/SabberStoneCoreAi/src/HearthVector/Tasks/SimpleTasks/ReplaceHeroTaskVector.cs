using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ReplaceHeroTaskVector : SimpleTaskVector
	{
		public int HeroAssetId { get; private set; }
		public int PowerAssetId { get; private set; }
		public int WeaponAssetId { get; private set; }

		public ReplaceHeroTaskVector(ReplaceHeroTask replaceHeroTask) : base(replaceHeroTask)
		{
			HeroAssetId = replaceHeroTask.HeroAssetId;
			PowerAssetId = replaceHeroTask.PowerAssetId;
			WeaponAssetId = replaceHeroTask.WeaponAssetId;
		}
	}
}
