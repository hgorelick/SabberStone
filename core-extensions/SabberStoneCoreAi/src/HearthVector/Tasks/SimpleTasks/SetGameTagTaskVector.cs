using SabberStoneCore.Enchants;
using SabberStoneCore.Tasks.SimpleTasks;
using SabberStoneCoreAi.HearthVector.Enchants;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class SetGameTagTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Tag { get; private set; }
		public int Type { get; private set; }

		public SetGameTagTaskVector(SetGameTagTask setGameTagTask) : base(setGameTagTask)
		{
			Amount = setGameTagTask.Amount;
			Tag = (int)setGameTagTask.Tag;
			Type = (int)setGameTagTask.Type;
		}
	}

	public class ApplyEffectTaskVector : SimpleTaskVector
	{
		public EffectVector[] Effects { get; private set; }
		public int Type { get; private set; }

		public ApplyEffectTaskVector(ApplyEffectTask applyEffectTask) : base(applyEffectTask)
		{
			Effects = EffectVector.Create(applyEffectTask.Effects);
			Type = (int)applyEffectTask.Type;
		}
	}
}
