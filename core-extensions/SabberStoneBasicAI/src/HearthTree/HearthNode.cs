﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.Kettle;
using SabberStoneCoreAi.Utils;
using SabberStoneCore.HearthVector;
using System.Collections.Specialized;

namespace SabberStoneCoreAi.HearthNodes
{
	/// <summary>
	/// Constructors and defining members
	/// </summary>
	public partial class HearthNode : IHearthVector
	{
		//protected HearthNode _root { get; set; }
		//public HearthNode Root => _root;

		public bool IsRoot => Parent == null; // || this is RootNode;

		protected bool _isEndTurn = false;
		public bool IsEndTurn => _isEndTurn;

		protected HearthNode _parent { get; set; }
		public HearthNode Parent => _parent;

		protected Game _game { get; set; }
		public Game Game => _game;

		//public double Reward;
		public int Wins { get; set; } = 0;

		public int Visits { get; set; } = 0;

		//protected PowerHistory _powerHistory { get; set; }
		//public PowerHistory PowerHistory => _powerHistory;

		protected int _damage = 0;
		public int Damage => _damage;

		protected readonly PlayerTask _action;
		public PlayerTask Action => _action;

		protected bool _logging { get; set; } = true;
		public bool Logging => _logging;

		//protected bool _processed { get; set; } = false;
		//public bool Processed => _processed;

		protected List<HearthNode> _frontier { get; } = new List<HearthNode>();
		protected List<HearthNode> _children { get; set; } = new List<HearthNode>();
		protected List<HearthNode> _endTurns { get; set; } = new List<HearthNode>();

		/// <summary>
		/// Actions that have yet to be applied to this HearthNode. Once expanded,
		/// the resulting HearthNode is added to Children.
		/// </summary>
		public List<HearthNode> Frontier => _frontier;

		/// <summary>
		/// Expanded PossibleActions
		/// </summary>
		/// <value><see cref="ActionNode"/></value>
		public List<HearthNode> Children => _children;

		public List<HearthNode> EndTurns => _endTurns;

		public string Prefix()
		{
			return $"{GetType().Name}.";
		}

		/// <summary>
		/// Vectorization of the current Game state, along with some HearthNode info.
		/// </summary>
		/// <returns><see cref="Game.Vector()"/></returns>
		public OrderedDictionary Vector()
		{
			return Game.Vector();
		}

		/// <summary>
		/// Base class constructor
		/// </summary>
		/// <param name="root"></param>
		/// <param name="game"></param>
		public HearthNode(HearthNode parent, Game game, PlayerTask action)//, bool isRoot = false)
        {
			//_root = root;
			_parent = parent;
			_action = action;
			_isEndTurn = (Action?.PlayerTaskType ?? PlayerTaskType.ROOT) == PlayerTaskType.END_TURN;

			_game = game.Clone();
			_logging = Game.Logging;

			//_powerHistory = new PowerHistory();
			//IsRoot = isRoot;

			if (IsRoot)
				GenPossibleActions();

			else
				Process();
			//PowerHistory.AddRange(game.PowerHistory);
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="other"></param>
		protected HearthNode(HearthNode other)
		{
			//_root = other.Root;
			_parent = null;// other.Parent?.Clone() ?? null;
			_logging = other.Logging;
			_game = other.Game.Clone();
			//_powerHistory = other.CopyPowerHistory();
			_action = other.Action;
			_damage = other.Damage;

			for (int i = 0; i < other.Frontier.Count; ++i)
				AddFrontier(other.Frontier[i]);

			for (int i = 0; i < other.Children.Count; ++i)
				AddChild(other.Children[i]);	
		}


		public static HearthNode CreateRoot(Game game)
        {
			var root = new HearthNode(null, game, null);
			//root._root = root;
			return root;
		}

		/// <summary>
		/// Clones this HearthNode
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual HearthNode Clone() { return new HearthNode(this); }

		/// <summary>
		/// Processes this HearthNode's action
		/// </summary>
		public virtual void Process()
		{
			int oppHealthBefore = Game.CurrentOpponent.TotalHealth();// * OPP_HEALTH_WEIGHT;
			int myHealthBefore = Game.CurrentPlayer.TotalHealth();// * MY_HEALTH_WEIGHT;

			Game.Process(Action);

			#region OLD -- Check if Processed 
			//if (!Processed)
			//{
			//	Game.Process(Action);
			//	_processed = true;
			//}
			#endregion

			Game.CurrentPlayer.Game = Game;
			Game.CurrentOpponent.Game = Game;

			int oppHealthAfter = Game.CurrentOpponent.TotalHealth();
			_damage = oppHealthBefore - oppHealthAfter;

			int myHealthAfter = Game.CurrentPlayer.TotalHealth();
			if (myHealthAfter <= 0)
			{
				Wins = -100;
				return;
			}

			#region Weighted Reward
			//myHealthAfter *= MY_HEALTH_WEIGHT;
			//
			//double oppHealthAfter = _game.CurrentOpponent.Hero.Health + _game.CurrentOpponent.Hero.Armor;
			//if (oppHealthAfter <= 0)
			//{
			//	Wins = 1;
			//	return;
			//}
			//
			//oppHealthAfter *= OPP_HEALTH_WEIGHT;
			//
			//Reward += oppHealthBefore != oppHealthAfter ? CalculateReward(oppHealthBefore, oppHealthAfter) : 0;
			#endregion
		}
	}

	/// <summary>
	/// MCTS methods
	/// </summary>
	public partial class HearthNode
	{
		protected const double DISCOUNT = 0.75;
		protected const double MY_HEALTH_WEIGHT = 0.1;
		protected const double OPP_HEALTH_WEIGHT = 0.1;

		/// <summary>
		/// Override method for RootNode
		/// </summary>
		protected void GenPossibleActions()
		{
			List<PlayerTask> options = Game.CurrentPlayer.Options();
			for (int i = 0; i < options.Count; ++i)
			{
				#region OLD -- Before removing endturn/action nodes
				//if (options[i].PlayerTaskType == PlayerTaskType.END_TURN)
				//	AddFrontier(new EndTurnNode(Root, this, Game.Clone(), options[i]));

				//else
				#endregion

				var possibleAction = new HearthNode(this, Game, options[i]);
				AddFrontier(possibleAction);

				//if (possibleAction.IsEndTurn)
				//{
				//	if (IsRoot)
				//		EndTurns.Add(possibleAction);
				//	else
				//	{
				//		HearthNode root = this;
				//		while (!root.IsRoot)
				//			root = root.Parent;
				//		root.EndTurns.Add(possibleAction);
				//	}
				//}

				//if (possibleAction.Game.Turn == Game.Turn && possibleAction.Game.State != State.COMPLETE)
				//	possibleAction.GenPossibleActions();					
			}
		}
	}

	#region Getters, setters, adders, printers, and other utilities
	/// <summary>
	/// Getters, setters, adders, printers, and other utilities
	/// </summary>
	public partial class HearthNode
	{
		/// <summary>
		/// Adds child to parent's children
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="child"></param>
		public void AddChild(HearthNode child)
		{
			Children.Add(child);
			child._parent = this;
		}

		/// <summary>
		/// Adds possibility to parent's PossibleActions
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="frontier"></param>
		internal void AddFrontier(HearthNode frontier)
		{
			Frontier.Add(frontier);
			frontier._parent = this;
		}

		/// <summary>
		/// Essentially pop() method for parent's PossibleActions (i.e. remove and return)
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="frontier"></param>
		internal HearthNode PopFrontier(HearthNode frontier)
		{
			Frontier.Remove(frontier);
			return frontier;
		}

		/// <summary>
		/// Moves possibility from this HearthNode's PossibleActions' to its Children.
		/// </summary>
		/// <param name="frontier"></param>
		internal void BirthPossibility(HearthNode frontier)
		{
			Frontier.Remove(frontier);
			AddChild(frontier);
		}

		/// <summary>
		/// Builds a string describing the move leading to this HearthNode
		/// </summary>
		/// <returns></returns>
		public string PrintAction()
		{
			var str = new StringBuilder();
			try
			{
				PlayerTask action = Action;
				string currentPlayer = action?.Controller?.Hero.Card.Name ?? action.Source.Controller.Hero.Card.Name;
				string pronoun = Game.CurrentOpponent.HeroClass == CardClass.MAGE || Game.CurrentOpponent.HeroClass == CardClass.ROGUE ? "her" : "his";
				if (action == null)
					str.AppendLine($"{Game.CurrentOpponent.Hero.Card.Name} ended {pronoun} turn\n");

				switch (action.PlayerTaskType)
				{
					case PlayerTaskType.END_TURN:
						str.AppendLine($"{Game.CurrentOpponent.Hero.Card.Name} ended {pronoun} turn\n");
						break;

					case PlayerTaskType.PLAY_CARD:
						if (action.Source is Spell)
						{
							str.AppendLine($"{currentPlayer} used {action.Source.Card.Name} {(action.HasTarget ? $"on {action.Target.Card.Name}!\n" : "\n")}");
							break;
						}
						str.AppendLine($"{currentPlayer} played {action.Source.Card.Name}\n");
						break;

					case PlayerTaskType.HERO_ATTACK:
						str.AppendLine($"{currentPlayer} attacked {action.Target.Card.Name}\n");
						break;

					case PlayerTaskType.MINION_ATTACK:
						str.AppendLine($"{action.Source.Card.Name} attacked {action.Target.Card.Name}\n");
						break;

					case PlayerTaskType.HERO_POWER:
						str.AppendLine($"{currentPlayer} used {pronoun} hero power");
						break;

					case PlayerTaskType.CHOOSE:
						str.AppendLine($"{currentPlayer} played {Game.CurrentPlayer.CardsPlayedThisTurn.Last().Name}");
						str.AppendLine($"{currentPlayer} is choosing...");
						try
						{
							string choice = Game.Logs.Single(l => l.Location == "ChoicePick")?.Text.Substring(13).Split("as")[0];
							str.AppendLine($"{currentPlayer} {(choice != null ? "chooses " + choice : "made a choice")}");
							break;
						}
						catch (Exception)
						{
							str.AppendLine($"{currentPlayer} made a choice");
						}
						break;
				}
			}
			catch (Exception)
			{
				return "Issue printing move";
			}
			return str.ToString();
		}

		/// <summary>
		/// Returns a json formatted string of this HearthNode's Action
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public string PrintAction(bool json)
		{
			var str = new StringBuilder();
			str.AppendLine("\t\"move\": {");
			str.AppendLine($"\t\t\"player\": {Action.Controller.PlayerId}");
			str.AppendLine($"\t\t\"type\": \"{Action.PlayerTaskType.ToString().ToLower()}\",");

			if (Action.PlayerTaskType == PlayerTaskType.END_TURN)
			{
				str.AppendLine("\t}");
				return str.ToString();
			}

			if (Action.HasSource)
				str.AppendLine($"\t\t\"source\": \"{Action.Source}\",");

			if (Action.HasTarget)
				str.AppendLine($"\t\t\"target\": \"{Action.Target}\"");

			str.AppendLine("\t}");
			return str.ToString();
		}

		/// <summary>
		/// Returns true if state is an only-child
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		internal bool OnlyChild()
		{
			if (Parent != null)
				return Parent.Children.Count == 1;
			return true;
		}

		/// <summary>
		/// Returns true if the sum of state and its siblings' visits == their parent's visits
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		internal bool FamilyVisits(bool exp = false)
		{
			HearthNode climber = Clone();
			int familyVisits = 0;

			if (climber.Children.Count > 0)
			{
				familyVisits += climber.Children.Sum(c => c.Visits);
				if (familyVisits == climber.Visits)
					familyVisits = 0;
				else
					return false;
			}

			while (!climber.IsRoot)
				climber = climber.Parent;

			familyVisits = climber.Children.Sum(c => c.Visits);
			return familyVisits == climber.Visits;
		}
	}
	#endregion

	/// <summary>
	/// HearthNode extension methods 
	/// </summary>
	internal static class HearthNodeHelper
	{
		internal static double ScoreSpecial(this HearthNode h, double score)
		{
			if (h.Action.Source?.Card.Type == CardType.HERO)
				score *= 1.15;

			else if (h.Parent.Game.CurrentPlayer.BoardZone.Sum(m => m.AttackDamage) >= h.Parent.Game.CurrentOpponent.Hero.Health - 5
					&& h.Action.PlayerTaskType == PlayerTaskType.MINION_ATTACK && h.Action.Target?.Card.Type == CardType.HERO)
				score *= 1.1;

			else
			{
				IEntity quest = h.Parent.Game.CurrentPlayer.GraveyardZone.Find(q => q.IsQuest);

				if (quest != null)
				{
					if (h.Action.HasSource && h.Action.PlayerTaskType == PlayerTaskType.PLAY_CARD)
						if (h.Action.Source.NativeTags.Contains(kv => kv.Key == GameTag.DISPLAYED_CREATOR
																			&& kv.Value == quest.NativeTags[GameTag.ENTITY_ID]))
							score *= 1.05;
				}
			}
			return score;
		}

		internal static PowerHistory CopyPowerHistory(this HearthNode h)
		{
			var copy = new PowerHistory();
			//copy.AddRange(h.PowerHistory);
			return copy;
		}
	}
}
