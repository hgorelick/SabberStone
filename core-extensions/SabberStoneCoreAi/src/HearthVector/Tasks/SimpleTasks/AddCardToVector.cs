using SabberStoneCoreAi.HearthVector.Enums;
using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class AddCardToVector : SimpleTaskVector
	{
		public int Amount { get; protected set; }
		public int AssetId { get; protected set; }
		public int AddToZone { get; protected set; }

		public AddCardToVector(AddCardTo addCardTo) : base(addCardTo)
		{
			Amount = addCardTo.Amount;
			AssetId = addCardTo.Card.AssetId;
			AddToZone = (int)addCardTo.Type;
		}
	}
}
