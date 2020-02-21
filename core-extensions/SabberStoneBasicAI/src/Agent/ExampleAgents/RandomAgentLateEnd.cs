//using System;
//using System.Collections.Generic;
//using System.Linq;
//using SabberStoneCore.Tasks.PlayerTasks;
//using SabberStoneCoreAi.POGamespace;
//using SabberStoneCoreAi.Agent;
//using SabberStoneCore.Enums;
//using SabberStoneCoreAi.HearthNodes;
//using SabberStoneCore.Model;

//namespace SabberStoneCoreAi.Agent.ExampleAgents
//{
//	class RandomAgentLateEnd : AbstractAgent
//	{
//		private Random Rnd = new Random();

//		public override void InitializeAgent()
//		{
//			Rnd = new Random();
//		}

//		public override void FinalizeAgent()
//		{
//			//Nothing to do here
//		}

//		public override void FinalizeGame()
//		{
//			//Nothing to do here
//		}

//		public override PlayerTask GetMove(EndTurnNode split, int option=0)
//		{			
//			List<PlayerTask> options = split._poGame.CurrentPlayer.Options();
//			if (options.Count > 1)
//			{
//				// filter all non EndTurn Tasks
//				List<PlayerTask> validTasks = new List<PlayerTask>();
//				foreach (PlayerTask task in options)
//				{
//					if (task.PlayerTaskType != PlayerTaskType.END_TURN)
//						validTasks.Add(task);
//				}
//				return validTasks[Rnd.Next(validTasks.Count)];
//			}
//			return options[0];

//		}

//		public override List<PlayerTask> GetMoves(POGame.POGame poGame)
//		{
//			return poGame.CurrentPlayer.Options();
//		}

//		public override void InitializeGame()
//		{
//			//Nothing to do here
//		}

//		public override List<Deck> GetAllOppDecks(CardClass oppClass)
//		{
//			return new List<Deck>();
//		}
//	}
//}
