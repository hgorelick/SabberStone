//using SabberStoneCore.Model;
//using SabberStoneCore.Tasks.PlayerTasks;
//using SabberStoneCoreAi.Utils;

//namespace SabberStoneCoreAi.HearthNodes
//{
//	public class ActionNode : HearthNode
//	{
//		/// <summary>
//		/// ActionNode constructor
//		/// </summary>
//		/// <param name="parent"></param>
//		/// <param name="root"></param>
//		/// <param name="game"></param>
//		/// <param name="action"</param>
//		public ActionNode(RootNode root, HearthNode parent, Game game, PlayerTask action)
//			: base(root, parent, game, action)
//		{
//		}

//		/// <summary>
//		/// Copy constructor
//		/// </summary>
//		/// <param name="other"></param>
//		private ActionNode(ActionNode other) : base(other) { }

//		/// <summary>
//		/// Clones this EndTurnNode
//		/// </summary>
//		/// <returns></returns>
//		public override HearthNode Clone() { return new ActionNode(this); }

//		/// <summary>
//		/// Processes the given action and calculates this ActionNode's Reward
//		/// </summary>
//		public override void Process()
//		{
//			int oppHealthBefore = Game.CurrentOpponent.TotalHealth();// * OPP_HEALTH_WEIGHT;
//			int myHealthBefore = Game.CurrentPlayer.TotalHealth();// * MY_HEALTH_WEIGHT;

//			Game.Process(Action);
//			//if (!Processed)
//			//{
//			//	Game.Process(Action);
//			//	_processed = true;
//			//}

//			Game.CurrentPlayer.Game = Game;
//			Game.CurrentOpponent.Game = Game;

//			int oppHealthAfter = Game.CurrentOpponent.TotalHealth();
//			_damage = oppHealthBefore - oppHealthAfter;

//			int myHealthAfter = Game.CurrentPlayer.TotalHealth();
//			if (myHealthAfter <= 0)
//			{
//				Wins = -100;
//				return;
//			}

//			//myHealthAfter *= MY_HEALTH_WEIGHT;
//			//
//			//double oppHealthAfter = _game.CurrentOpponent.Hero.Health + _game.CurrentOpponent.Hero.Armor;
//			//if (oppHealthAfter <= 0)
//			//{
//			//	Wins = 1;
//			//	return;
//			//}
//			//
//			//oppHealthAfter *= OPP_HEALTH_WEIGHT;
//			//
//			//Reward += oppHealthBefore != oppHealthAfter ? CalculateReward(oppHealthBefore, oppHealthAfter) : 0;
//		}

//		/// <summary>
//		/// Calculates the immediate reward of this action, which is the percent difference
//		/// in the opponent's health before and after the action.
//		/// </summary>
//		/// <returns></returns>
//		private double CalculateReward(double healthBefore, double healthAfter)
//		{
//			return 1 - (healthAfter / healthBefore);
//		}

//		/// <summary>
//		/// Creates this ActionNode's hash
//		/// </summary>
//		//public override void SetHash()
//		//{
//		//	var str = new StringBuilder();
//		//	str.Append("{S:{");
//		//	str.Append($"[VS:{Visits}]");
//		//	str.Append($"[R:{Reward}]");
//		//	str.Append("[GS:{");

//		//	str.Append($"[H:{_game.Player1.Hero.Health}][A:{_game.Player1.Hero.Armor}]");
//		//	str.Append($"[OH:{_game.Player2.Hero.Health}][OA:{_game.Player2.Hero.Armor}]");

//		//	//str.Append($"[AC:{PrintAction()}]");

//		//	str.Append($"[TM:{_game.Player1.BaseMana}][RM:{_game.Player1.RemainingMana}]");

//		//	str.Append($"[HZ:{{CNT:{_game.Player1.HandZone.Count}:[");
//		//	int ind = 0;
//		//	foreach (IPlayable card in _game.Player1.HandZone)
//		//	{
//		//		str.Append($"[P{ind},{card.Card.Name},{card.Cost}]");
//		//		ind++;
//		//	}
//		//	str.Append("]}]");

//		//	str.Append($"[DZ:{{CNT:{_game.Player1.DeckZone.Count}:[");
//		//	foreach (Card card in _game.Player1.Deck.)
//		//		str.Append($"{{{card.Name},{card.Cost}}}");
//		//	str.Append("]}]");

//		//	str.Append("[B:[");
//		//	ind = 0;
//		//	foreach (Minion minion in _game.Player1.BoardZone)
//		//	{
//		//		str.Append($"[P{ind},{minion.Card.Name},{minion.AttackDamage},{minion.Health}]");
//		//		ind++;
//		//	}
//		//	str.Append("]]");

//		//	str.Append("[OB:[");
//		//	ind = 0;
//		//	foreach (Minion minion in _game.Player2.BoardZone)
//		//	{
//		//		str.Append($"[P{ind},{minion.Card.Name},{minion.AttackDamage},{minion.Health}]");
//		//		ind++;
//		//	}
//		//	str.Append("]]");

//		//	if (_game.Player1.SecretZone.Count > 0)
//		//	{
//		//		str.Append($"[SZ:{{CNT:{_game.Player1.SecretZone.Count}:[");
//		//		foreach (Spell secret in _game.Player1.SecretZone)
//		//			str.Append($"{{{secret.Card.Name}}}");
//		//		str.Append("]}]");
//		//	}

//		//	if (_game.Player2.SecretZone.Count > 0)
//		//		str.Append($"OSZ:{{CNT:{_game.Player2.SecretZone.Count}}}");

//		//	Hash = str.ToString();
//		//}

//		//public bool Equals(ActionNode other) { return Reward == other.Reward; }

//		#region Operator Overloads
//		public static bool operator <(ActionNode stateAction1, ActionNode stateAction2) => stateAction1.Wins < stateAction2.Wins;

//		public static bool operator >(ActionNode stateAction1, ActionNode stateAction2) => stateAction1.Wins > stateAction2.Wins;

//		public static bool operator <=(ActionNode stateAction1, ActionNode stateAction2) => stateAction1.Wins <= stateAction2.Wins;

//		public static bool operator >=(ActionNode stateAction1, ActionNode stateAction2) => stateAction1.Wins >= stateAction2.Wins;

//		#endregion
//	}
//}
