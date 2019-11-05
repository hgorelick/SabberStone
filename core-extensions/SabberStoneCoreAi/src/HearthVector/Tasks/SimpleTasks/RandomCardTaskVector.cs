using SabberStoneCore.Tasks.SimpleTasks;
using System.Linq;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RandomCardTaskVector : SimpleTaskVector
	{
		public int CardClass { get; private set; }
		public int CardSet { get; private set; }
		public int CardType { get; private set; }
		public int[] GameTagFilter { get; private set; }
		public bool Opposite { get; private set; }
		public int Race { get; private set; }
		public int Rarity { get; private set; }
		public int Type { get; private set; }

		public RandomCardTaskVector(RandomCardTask randomCardTask) : base(randomCardTask)
		{
			CardClass = (int)randomCardTask.CardClass;
			CardSet = (int)randomCardTask.CardSet;
			CardType = (int)randomCardTask.CardType;
			GameTagFilter = randomCardTask.GameTagFilter.Select(gt => (int)gt).ToArray();
			Opposite = randomCardTask.Opposite;
			Race = (int)randomCardTask.Race;
			Rarity = (int)randomCardTask.Rarity;
			Type = (int)randomCardTask.Type;
		}
	}
}
