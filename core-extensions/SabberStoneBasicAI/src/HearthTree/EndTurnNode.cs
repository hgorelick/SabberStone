//using SabberStoneCore.Model;
//using SabberStoneCore.Tasks.PlayerTasks;
//using SabberStoneCoreAi.Utils;

//namespace SabberStoneCoreAi.HearthNodes
//{
//	public class EndTurnNode : HearthNode
//	{
//		/// <summary>
//		/// EndTurnNode constructor
//		/// </summary>
//		/// <param name="parent"></param>
//		/// <param name="root"></param>
//		/// <param name="game"></param>
//		/// <param name="endTurn"</param>
//		/// <param name="isRoot"></param>
//		public EndTurnNode(RootNode root, HearthNode parent, Game game, PlayerTask endTurn)
//			: base(root, parent, game, endTurn)
//		{
//			//Reward += DISCOUNT * _parent.Reward;

//			//Wins = (_game.CurrentPlayer.Hero.Health + _game.CurrentPlayer.Hero.Armor) * OPP_HEALTH_WEIGHT <= 0 ? 100 : Wins;
//		}

//		/// <summary>
//		/// Copy constructor
//		/// </summary>
//		/// <param name="other"></param>
//		private EndTurnNode(EndTurnNode other) : base(other) { }

//		/// <summary>
//		/// Clones this EndTurnNode
//		/// </summary>
//		/// <returns></returns>
//		public override HearthNode Clone() { return new EndTurnNode(this); }


//		public override void Process()
//		{
//			int oppHealthBefore = Game.CurrentOpponent.TotalHealth();

//			Game.Process(Action);
//			//if (!Processed)
//			//{
//			//	Game.Process(Action);
//			//	_processed = true;
//			//}

//			Game.CurrentPlayer.Game = Game;
//			Game.CurrentOpponent.Game = Game;

//			int oppHealthAfter = Game.CurrentPlayer.TotalHealth();
//			_damage = oppHealthBefore - oppHealthAfter;
//		}

//		#region Operator Overloads
//		public static bool operator <(EndTurnNode split1, EndTurnNode split2) => split1.Children.Count < split2.Children.Count;

//		public static bool operator >(EndTurnNode split1, EndTurnNode split2) => split1.Children.Count > split2.Children.Count;

//		public static bool operator <=(EndTurnNode split1, EndTurnNode split2) => split1.Children.Count <= split2.Children.Count;

//		public static bool operator >=(EndTurnNode split1, EndTurnNode split2) => split1.Children.Count >= split2.Children.Count;

//		#endregion
//	}
//}
