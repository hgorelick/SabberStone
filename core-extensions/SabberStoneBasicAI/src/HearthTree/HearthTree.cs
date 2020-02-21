using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.Model.Entities;
using SabberStoneCoreAi.Utils;

namespace SabberStoneCoreAi.HearthNodes
{
	public partial class HearthTree
	{
		private const int MAX_ITERATIONS = 49;

		HearthNode _root { get; set; }
		public HearthNode Root => _root;

		int _height { get; set; } = 1;
		public int Height => _height;

		int _width { get; set; } = 1;
		public int Width => _width;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="root"></param>
		public HearthTree(HearthNode root)
		{
			_root = root;
			//CalcDimensions(Root, 1, Root.Children.Count);
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="other"></param>
		HearthTree(HearthTree other)
		{
			_root = other.Root.Clone();
			_height = other.Height;
			_width = other.Width;
		}

		/// <summary>
		/// Clones this HearthTree
		/// </summary>
		/// <returns></returns>
		public HearthTree Clone() { return new HearthTree(this); }

		/// <summary>
		/// Finds the HearthNode with the most children, and returns the count
		/// </summary>
		/// <param name="h"></param>
		/// <param name="hWidth"></param>
		/// <returns></returns>
		public void CalcDimensions(HearthNode h, int height, int width)
		{
			if (h.Children.Count == 0)
				return;

			int maxHeight = Math.Max(1, height);
			int maxWidth = Math.Max(width, h.Children.Count);
			for (int i = 0; i < Root.Children.Count; ++i)
			{
				CalcDimensions(Root.Children[i], maxHeight, maxWidth);
			}

			_height = maxHeight;
			_width = maxWidth;
		}
	}

	public partial class HearthTree
	{
		/// <summary>
		/// Performs a MCTS on the given node
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public HearthNode MCTS()
		{
			var move = new Tuple<HearthNode, bool>(null, false);

			HearthNode obvious = Root.GetObvious();
			if (obvious != null)
				move = new Tuple<HearthNode, bool>(obvious, true);

			else
			{
				int iterations = 0;
				while (iterations < MAX_ITERATIONS)
				{
					_root = Select(Root);
					_root = Expand(Root);
					int value = Simulate(Root.Clone());
					_root = Backpropogate(Root, value);//, iterations);
					iterations++;
				}
				move = new Tuple<HearthNode, bool>(Root, false);
			}

			return ChooseMove(move);
		}

		/// <summary>
		/// Chooses the MoveNode that maximizes the UCT function
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		HearthNode ChooseMove(Tuple<HearthNode, bool> move)
		{
			HearthNode selection = null;

			if (move.Item2)
				selection = move.Item1;

			else if (move.Item1.Children.Count == 0)
				selection = move.Item1.Frontier.Find(p => p.IsEndTurn);

			else
			{
				HearthNode MCTSNode = move.Item1;
				List<HearthNode> children = MCTSNode.Children;

				HearthNode endTurn = (MCTSNode.Game.CurrentOpponent.BoardZone.IsEmpty
								   && MCTSNode.Game.CurrentOpponent.SecretZone.IsEmpty
								   && children.Count > 1) ? children.Pop(h => h.IsEndTurn) : null;

				///if (MCTSNode.Game.CurrentOpponent.BoardZone.IsEmpty && MCTSNode.Game.CurrentOpponent.SecretZone.IsEmpty && children.Count > 1)
				///	endTurn = children.Pop(h => h.IsEndTurn);

				double selScore = Int32.MinValue;

				for (int i = 0; i < children.Count; ++i)
				{
					double childScore = SelectionScore(children[i]);
					if (childScore > selScore)
					{
						selScore = childScore;
						selection = children[i];
					}
				}

				if (endTurn != null)
					MCTSNode.AddChild(endTurn);

				if (selection != null)
				{
					if (selection.Action.PlayerTaskType == PlayerTaskType.HERO_ATTACK)
						selection = MCTSNode.Children.Find(p => p.Action.Target?.Card.Name == selection.Action.Target?.Card.Name);

					else if (selection.Action.PlayerTaskType == PlayerTaskType.HERO_POWER)
						selection = MCTSNode.Children.Find(p => p.Action.PlayerTaskType == PlayerTaskType.HERO_POWER);

					else
						selection = MCTSNode.Children.Find(p => p.Action.Source?.Card.Name == selection.Action.Source?.Card.Name);
				}

				else
					selection = endTurn;
			}

			bool check = true;
			if (selection.Action.PlayerTaskType == PlayerTaskType.PLAY_CARD)
				if (selection.Action.Source.Card.Cost != (selection.Action.Source as IPlayable).Cost)
					check = false;
			return selection;
		}

		/// <summary>
		/// Default selection function, overridden by children
		/// </summary>
		/// <param name="Score"></param>
		/// <returns></returns>
		public HearthNode Select(HearthNode state)//IScore Score = null)
		{
			HearthNode endTurn = null;

			if (state.Frontier.Count == 0 && state.Children.Count > 0 && !state.IsEndTurn)
			{
				if (state.Game.CurrentOpponent.BoardZone.IsEmpty
					&& state.Game.CurrentOpponent.SecretZone.IsEmpty
					&& state.Children.Count > 1)
				{
					endTurn = state.Children.Pop(c => c.IsEndTurn);
				}

				HearthNode selection = state.ScoreAndSelect();

				if (endTurn != null)
					state.AddChild(endTurn);

				return Select(selection);
			}
			return state;
		}

		/// <summary>
		/// Calculates this ActionNode's score.
		/// </summary>
		/// <returns></returns>
		public double SelectionScore(HearthNode child)
		{
			double exploitation = ((double)child.Wins / child.Visits);// * child.Reward;
			double exploration = Math.Sqrt(Math.Log((double)child.Parent.Visits / child.Visits));
			exploration = Double.IsNaN(exploration) ? 0 : 2.0 * exploration;

			double score = exploitation + exploration;

			if (!child.IsEndTurn)
				score = child.ScoreSpecial(score);

			return score;
		}

		/// <summary>
		/// Creates ActionNodes that contain simulated games from a given list of task options.
		/// Also adds these new nodes as children to this node.
		/// </summary>
		/// <param name="tasksToSimulate"></param>
		/// <returns></returns>
		HearthNode Expand(HearthNode state)
		{
			List<HearthNode> dumbMoves = state.Purge();

			if (dumbMoves.Count > 0)
				for (int i = 0; i < dumbMoves.Count; ++i)
					state.Frontier.Remove(dumbMoves[i]);

			if (state.Frontier.Count == 0 || state.IsEndTurn)
			{
				if (state.IsEndTurn)
					state.Frontier.Clear();

				return state;
			}

			var rnd = new Random();
			int rndInd = rnd.Next(state.Frontier.Count);
			HearthNode expansion = state.Frontier[rndInd];

			state.BirthPossibility(expansion);
			//expansion = state.PopPossibility(expansion);
			//state.AddChild(expansion);

			if (expansion.Game.State != State.COMPLETE && expansion.Game.State != State.INVALID && !expansion.IsEndTurn)
				expansion.GenPossibleActions();

			if (dumbMoves.Count > 0)
				for (int i = 0; i < dumbMoves.Count; ++i)
					state.AddFrontier(dumbMoves[i]);

			return expansion;
		}

		/// <summary>
		/// Simulates the game from this HearthNode
		/// </summary>
		/// <returns></returns>
		int Simulate(HearthNode state)
		{
			int currentPlayer = state.IsEndTurn ? state.Game.CurrentOpponent.PlayerId : state.Game.CurrentPlayer.PlayerId;

			while (state.Game.State != State.COMPLETE && state.Game.State != State.INVALID)
			{
				if (state.Frontier.Count == 0)
					state.GenPossibleActions();

				List<HearthNode> deathNodes = state.Frontier.FindAll(p => p.Wins == -100);
				if (deathNodes != null && state.Frontier.Count > 1)
					for (int i = 0; i < deathNodes.Count; ++i)
						state.Frontier.Remove(deathNodes[i]);

				var rnd = new Random();
				int rndInd = rnd.Next(state.Frontier.Count);
				state = state.Frontier[rndInd];
			}

			if (state.Game.CurrentPlayer.PlayerId == currentPlayer)
				return state.Game.CurrentPlayer.PlayState == PlayState.WON ? 1 : 0;

			else
				return state.Game.CurrentPlayer.PlayState == PlayState.WON ? 0 : 1;
		}

		/// <summary>
		/// Propogates back up the tree and updates each parent's Quct, n(s), n(s,a), and value
		/// </summary>
		HearthNode Backpropogate(HearthNode state, int value)//Tuple<double, double> valueWithReward)
		{
			int currentPlayer = state.IsEndTurn ? state.Game.CurrentOpponent.PlayerId : state.Game.CurrentPlayer.PlayerId;
			bool won = value == 1 ? true : false;

			while (!state.IsRoot)// || state.Parent != null)
			{
				if (state.Parent.Game.CurrentPlayer.PlayerId == currentPlayer)
					state = UpdateRV(state, value);//, iterations);

				else
					state = won ? UpdateRV(state, 0) : UpdateRV(state, 1);//, iterations);

				state = state.Parent;
			}
			state.Visits++;
			return state;
		}

		/// <summary>
		/// Updates the Root/Action node according to the optimal policy function
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		HearthNode UpdateRV(HearthNode state, int value)//, double reward)
		{
			state.Visits++;
			state.Wins += value;

			return state;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	internal static class TreeHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		internal static HearthNode GetObvious(this HearthNode state)
		{
			HearthNode obvious = state.Frontier.Count == 1 ? state.GetSingleChoice() : state.PlayQuest();

			if (obvious == null)
			{
				List<HearthNode> lethalMoves = state.GetLethalMoves();
				if (lethalMoves != null)
					obvious = lethalMoves[0];

				else if (state.AttackOnly())
					obvious = state.Frontier[0];
			}

			else
				state.BirthPossibility(obvious);

			return obvious;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		internal static HearthNode PlayQuest(this HearthNode state)
		{
			return state.Frontier.Count > 1 ? state.Frontier.Find(p => p.Action?.Source?.Card.IsQuest ?? false) : null;
		}

		/// <summary>
		/// If there's only one possible move, no need to simulate
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		internal static HearthNode GetSingleChoice(this HearthNode state)
		{
			HearthNode choice = state.Frontier[0];
			state.Frontier.Remove(choice);
			state.AddChild(choice);
			return choice;
		}

		internal static List<HearthNode> Purge(this HearthNode state)
		{
			var purged = new List<HearthNode>();

			for (int i = 0; i < state.Frontier.Count; ++i)
			{
				HearthNode p = state.Frontier[i];

				if (p.Action.Source?.Card.Name == "Warpath" && state.Game.CurrentOpponent.BoardZone.IsEmpty)
					purged.Add(p);

				else if (p.Game.CurrentPlayer.Hero.HeroPower.Card.Name == "Bladestorm"
															 && p.Action.PlayerTaskType == PlayerTaskType.HERO_POWER
															 && state.Game.CurrentOpponent.BoardZone.IsEmpty)
					purged.Add(p);

				else if (p.Action.Source?.Card.Name == "Drywhisker Armorer"
															 && state.Game.CurrentOpponent.BoardZone.IsEmpty
															 && p.Action.PlayerTaskType == PlayerTaskType.PLAY_CARD)
					purged.Add(p);

				else if (p.Action.Source?.Card.Name == "Serrated Shield"
															 && (p.Action.Target == p.Game.CurrentPlayer ||
																 p.Action.Target.Controller == p.Game.CurrentPlayer))
					purged.Add(p);

				else if (p.Action.Source?.Card.Name == "Execute"
															 && p.Action.Target.Controller == p.Game.CurrentPlayer)
					purged.Add(p);

				else if (p.Action.Source?.Card.Name == "Brawl"
															 && (state.Game.BoardCount() > 2)
															 && (state.Game.CurrentPlayer.BoardZone.Sum(m => m.Health) > state.Game.CurrentOpponent.BoardZone.Sum(m => m.AttackDamage) ||
																 state.Game.CurrentPlayer.BoardZone.Count(m => m.HasTaunt) > state.Game.CurrentOpponent.BoardZone.Count))
					purged.Add(p);

				else if (p.Action.Source?.Card.Name == "Molten Blade")
					purged.Add(p);
			}

			return purged;
		}

		/// <summary>
		/// Generates this HearthNode's possible actions from the move options
		/// available to the current player.
		/// </summary>
		internal static void GenPossibleActions(this HearthNode state)
		{
			List<PlayerTask> options = state.Game.CurrentPlayer.Options();
			for (int i = 0; i < options.Count; ++i)
			{
				state.AddFrontier(new HearthNode(state, state.Game, options[i]));
			}
		}

		/// <summary>
		/// Scores each of state's children, then returns the one with the highest score.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		internal static HearthNode ScoreAndSelect(this HearthNode state)
		{
			HearthNode selection = null;
			double selScore = Int32.MinValue;

			for (int i = 0; i < state.Children.Count; ++i)
			{
				double childScore = state.Children[i].SelectionScore();
				if (childScore > selScore)
				{
					selScore = childScore;
					selection = state.Children[i];
				}
			}
			return selection;
		}

		/// <summary>
		/// Calculates this ActionNode's score;
		/// </summary>
		/// <returns></returns>
		static double SelectionScore(this HearthNode child)
		{
			double exploitation = ((double)child.Wins / child.Visits);
			double exploration = Math.Sqrt(Math.Log((double)child.Parent.Visits / child.Visits));
			exploration = Double.IsNaN(exploration) ? 0 : 2.0 * exploration;

			double score = exploitation + exploration;

			if (!child.IsEndTurn)
				score = child.ScoreSpecial(score);

			return score;
		}
	}
}
