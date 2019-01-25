using SabberStoneCore.Tasks.PlayerTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SabberStoneCoreAi.POGamespace;

namespace SabberStoneCoreAi.Tyche2
{
    class TySimTree
    {	
		private TyStateAnalyzer _analyzer;
		private POGame _rootGame;

		private Dictionary<PlayerTask, TyTaskNode> _nodesToEstimate = new Dictionary<PlayerTask, TyTaskNode>();
		private List<TyTaskNode> _explorableNodes = new List<TyTaskNode>();
		private List<TyTaskNode> _sortedNodes = new List<TyTaskNode>();

		public void InitTree(TyStateAnalyzer analyzer, POGame root, List<PlayerTask> options)
		{
			_sortedNodes.Clear();
			_explorableNodes.Clear();
			_nodesToEstimate.Clear();

			_analyzer = analyzer;
			_rootGame = root;

			//var initialResults = TyStateUtility.GetSimulatedGames(root, options, _analyzer);

			for (int i = 0; i < options.Count; i++)
			{
				PlayerTask task = options[i];

				var node = new TyTaskNode(this, _analyzer, task, 0.0f);

				//end turn is pretty straight forward, should not really be looked at later in the simulations, just simulate once and keep the value:
				if (task.PlayerTaskType == PlayerTaskType.END_TURN)
				{
					TySimResult sim = TyStateUtility.GetSimulatedGame(root, task, _analyzer);
					node.AddValue(sim.value);
				}
				else
				{
					_explorableNodes.Add(node);
					_sortedNodes.Add(node);
				}

				_nodesToEstimate.Add(task, node);
			}
		}

		public void SimulateEpisode(Random random, int curEpisode, bool shouldExploit)
		{
			TyTaskNode nodeToExplore = null;

			//exploiting:
			if (shouldExploit)
			{	
				_sortedNodes.Sort((x, y) => y.TotalValue.CompareTo(x.TotalValue));
				//exploit only 50% best nodes:
				int count = ((int)(_sortedNodes.Count * 0.5 + 0.5));
				nodeToExplore = _sortedNodes.GetUniformRandom(random, count);
			}

			//explore:
			else
				nodeToExplore = _explorableNodes[curEpisode % _explorableNodes.Count];

			//should not be possible:
			if (nodeToExplore == null)
				return;

			PlayerTask task = nodeToExplore.Task;
			TySimResult result = TyStateUtility.GetSimulatedGame(_rootGame, task, _analyzer);
			nodeToExplore.Explore(result, random);
		}

		public TyTaskNode GetBestNode()
		{
			var nodes = new List<TyTaskNode>(_nodesToEstimate.Values);
			nodes.Sort((x, y) => y.GetAverage().CompareTo(x.GetAverage()));
			return nodes[0];
		}

		public PlayerTask GetBestTask()
		{
			return GetBestNode().Task;
		}
	}
}
