using SabberStoneCore.Model;
using SabberStoneCore.Tasks;
using SabberStoneCore.Kettle;
using SabberStoneCoreAi.Utils;

namespace SabberStoneCoreAi.HearthNodes
{
	public class RootNode : HearthNode
	{
		/// <summary>
		/// Root node constructor
		/// </summary>
		/// <param name="root"></param>
		/// <param name="parent"></param>
		/// <param name="game"></param>
		public RootNode(RootNode root, HearthNode parent, Game game, PlayerTask action)
			: base(root, parent, game, action)
		{
			//IsRoot = true;

			//if (Root == null)
			//{
			//	//game.PowerHistory.HSReplaySort();
			//	//game.PowerHistory.Dump(@"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\SabberStone.log", root: true);
			//	this.Write("Sabber", true);
			//	game.PowerHistory = new PowerHistory();
			//}

			GenPossibleActions();
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="other"></param>
		public RootNode(RootNode other) : base(other) { }

		/// <summary>
		/// Clones this EndTurnNode
		/// </summary>
		/// <returns></returns>
		public override HearthNode Clone() { return new RootNode(this); }

		/// <summary>
		/// Does nothing
		/// </summary>
		public override void Process() { }
	}
}
