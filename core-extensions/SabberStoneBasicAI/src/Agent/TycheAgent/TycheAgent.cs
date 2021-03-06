﻿using SabberStoneCore.Model;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCore.Meta;
using SabberStoneCoreAi.POGamespace;
using System;
using System.Collections.Generic;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.Utils;

namespace SabberStoneCoreAi.Tyche2
{
	class TycheAgentCompetition : AbstractAgent
	{
		public static Deck GetUserCreatedDeck() { return Decks.MidrangeJadeShaman; }
		public Deck UserCreatedDeck { get { return GetUserCreatedDeck(); } }

		readonly Deck deck;
		readonly CardClass hero;

		//x% of episodes below this value, are used for exploration, the remaining are used for exploitation:
		private const double EXPLORE_TRESHOLD = 0.75;

		private const int DEFAULT_NUM_EPISODES_MULTIPLIER = 100;
		private const int LEARNING_NUM_EPISODES_MULTIPLIER = 20;

		public enum Algorithm { Greedy, SearchTree }

		private TyStateAnalyzer _analyzer;
		private TySimTree _simTree;
		private Random _random;

		private bool _isTurnBegin = true;
		private bool _hasInitialized;

		private double _turnTimeStart;
		private bool _heroBasedWeights;
		private int _curEpisodeMultiplier;
		private int _defaultEpisodeMultiplier;
		
		public Algorithm UsedAlgorithm = Algorithm.SearchTree;
		public bool AdjustEpisodeMultiplier = false;
		public bool PrintTurnTime = false;

		private int _playerId;
		public int PlayerId { get { return _playerId; } }

		public TycheAgentCompetition()
			: this(TyStateWeights.GetDefault(), true, DEFAULT_NUM_EPISODES_MULTIPLIER, true)
		{		
			deck = Decks.MidrangeJadeShaman;
			hero = CardClass.SHAMAN;
		}

		public TycheAgentCompetition(CardClass heroClass, CardClass enemy, Deck d)
			: this(TyStateWeights.GetHeroBased(heroClass, enemy), true, DEFAULT_NUM_EPISODES_MULTIPLIER, true)
		{
			deck = d;
			hero = heroClass;
		}

		private TycheAgentCompetition(TyStateWeights weights, bool heroBasedWeights, int episodeMultiplier, bool adjustEpisodeMultiplier)
		{
			_defaultEpisodeMultiplier = episodeMultiplier;
			_curEpisodeMultiplier = episodeMultiplier;
			_heroBasedWeights = heroBasedWeights;

			_analyzer = new TyStateAnalyzer(weights);
			_simTree = new TySimTree();
			_random = new Random();

			AdjustEpisodeMultiplier = adjustEpisodeMultiplier;
		}

		public override HearthNode PlayTurn(HearthNode state)
		{
			var poGame = new POGame(state.Game, false);

			if (!_hasInitialized)
				CustomInit(poGame);

			if (_isTurnBegin)
				OnMyTurnBegin(poGame);

			List<PlayerTask> options = poGame._game.CurrentPlayer.Options();
			PlayerTask chosen = ChooseTask(poGame, options);

			HearthNode selected = state.Frontier.Find(h => h.Action.IsEqual(chosen));

			//if (selected == null)
			//	foreach (HearthNode p in state.PossibleActions)
			//	{
			//		if (p.Action.IsEqual(chosen))
			//			selected = p;
			//	}

			//should not happen, but if, just return anything:
			if (selected == null)
			{
				if(TyConst.LOG_UNKNOWN_CORRECTIONS)
				{	
					TyDebug.LogError("Choosen task was null!");
				}

				selected = state.Frontier.Find(h => h.Action.IsEqual(options.GetUniformRandom(_random)));
			}

			//selected.Process();
			try
			{
				state.BirthPossibility(selected);

			}
			catch (Exception)
			{
				Console.Write("Why");
			}

			if (selected.IsEndTurn)
				OnMyTurnEnd();

			return selected;
		}

		private PlayerTask ChooseTask(POGame poGame, List<PlayerTask> options)
		{
			if (options.Count == 1)
				return options[0];

			else if (UsedAlgorithm == Algorithm.SearchTree)
				return GetSimulationTreeTask(poGame, options);

			else if(UsedAlgorithm == Algorithm.Greedy)
				return GetGreedyBestTask(poGame, options);

			else
				return null;
		}

		private PlayerTask GetSimulationTreeTask(POGame poGame, List<PlayerTask> options)
		{
			double time = TyUtility.GetSecondsSinceStart() - _turnTimeStart;

			if (time >= TyConst.MAX_TURN_TIME)
			{
				TyDebug.LogError("Turn takes too long, fall back to greedy.");
				return GetGreedyBestTask(poGame, options);
			}

			_simTree.InitTree(_analyzer, poGame, options);

			//-1 because TurnEnd won't be looked at:
			int optionCount = options.Count - 1;
			int numEpisodes = (int)((optionCount) * _curEpisodeMultiplier);

			double simStart = TyUtility.GetSecondsSinceStart();

			for (int i = 0; i < numEpisodes; i++)
			{
				if (!IsAllowedToSimulate(simStart, i, numEpisodes, optionCount))
					break;

				bool shouldExploit = ((double)i / (double)numEpisodes) > EXPLORE_TRESHOLD;
				_simTree.SimulateEpisode(_random, i, shouldExploit);
			}

			TyTaskNode bestNode = _simTree.GetBestNode();
			return bestNode.Task;
		}

		private PlayerTask GetGreedyBestTask(POGame poGame, List<PlayerTask> options)
		{
			List<TySimResult> bestTasks = TyStateUtility.GetSimulatedBestTasks(1, poGame, options, _analyzer);
			return bestTasks[0].task;
		}

		/// <summary> False if there is not enough time left to do simulations. </summary>
		private bool IsAllowedToSimulate(double startTime, int curEpisode, int maxEpisode, int options)
		{
			double time = TyUtility.GetSecondsSinceStart() - startTime;

			if (time >= TyConst.MAX_SIMULATION_TIME)
			{	
				TyDebug.LogWarning("Stopped simulations after " + time.ToString("0.000") + "s and " + curEpisode + " of " + maxEpisode + " episodes. Having " + options + " options.");
				return false;
			}

			return true;
		}

		private void OnMyTurnBegin(POGame state)
		{
			_isTurnBegin = false;
			_turnTimeStart = TyUtility.GetSecondsSinceStart();
		}

		private void OnMyTurnEnd()
		{
			_isTurnBegin = true;

			double timeNeeded = TyUtility.GetSecondsSinceStart() - _turnTimeStart;

			if (AdjustEpisodeMultiplier && UsedAlgorithm == Algorithm.SearchTree)
			{
				const double MAX_DIFF = 4.0;
				double diff = Math.Min(TyConst.DECREASE_SIMULATION_TIME - timeNeeded, MAX_DIFF);
				double factor = 0.05;

				//reduce more if above the time limit:
				if(diff <= 0.0f)
					factor = 0.2;

				//simulate at max this value * _defaultEpisodeMultiplier:
				const int MAX_EPISODE_MULTIPLIER = 4;
				_curEpisodeMultiplier = Math.Clamp(_curEpisodeMultiplier + (int)(factor * diff * _defaultEpisodeMultiplier),
													_defaultEpisodeMultiplier,
													_defaultEpisodeMultiplier * MAX_EPISODE_MULTIPLIER);
			}

			if (PrintTurnTime)
			{	
				TyDebug.LogInfo("Turn took " + timeNeeded.ToString("0.000") + "s");
			}

			if(timeNeeded >= TyConst.MAX_TURN_TIME)
			{
				TyDebug.LogWarning("Turn took " + timeNeeded.ToString("0.000") + "s");
			}
		}

		/// <summary> Called the first round (might be second round game wise) this agents is able to see the game and his opponent. </summary>
		private void CustomInit(POGame initialState)
		{
			_hasInitialized = true;

			_playerId = initialState._game.CurrentPlayer.PlayerId;
			_analyzer.OwnPlayerId = _playerId;

			if (_heroBasedWeights)
				_analyzer.Weights = TyStateWeights.GetHeroBased(initialState._game.CurrentPlayer.HeroClass, initialState._game.CurrentOpponent.HeroClass);
		}

		public override void InitializeGame()
		{
			_hasInitialized = false;
		}
		
		public static TycheAgentCompetition GetLearningAgent(TyStateWeights weights)
		{	
			return new TycheAgentCompetition(weights, false, LEARNING_NUM_EPISODES_MULTIPLIER, false);
		}

		public static TycheAgentCompetition GetSearchTreeAgent(int episodeMultiplier)
		{
			return new TycheAgentCompetition(TyStateWeights.GetDefault(), true, episodeMultiplier, true);
		}

		public static TycheAgentCompetition GetTrainingAgent(float biasFactor = -1.0f, bool useSecrets = false)
		{
			const bool ADJUST_EPISODES = false;
			const bool HERO_BASED_WEIGHTS = false;

			var weights = TyStateWeights.GetDefault();

			if(biasFactor >= 0.0f)
				weights.SetWeight(TyStateWeights.WeightType.BiasFactor, biasFactor);

			var agent =  new TycheAgentCompetition(weights, HERO_BASED_WEIGHTS, 0, ADJUST_EPISODES);
			agent.UsedAlgorithm = Algorithm.Greedy;
			agent._analyzer.EstimateSecretsAndSpells = useSecrets;

			return agent;
		}

		public override void InitializeAgent() { }
		public override void FinalizeAgent() { }
		public override void FinalizeGame() { } 
	}
}
