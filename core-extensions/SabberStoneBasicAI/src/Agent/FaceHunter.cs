//using System;
//using System.Collections.Generic;
//using System.Linq;
//using SabberStoneCore.Tasks.PlayerTasks;
//using SabberStoneCoreAi.HearthNodes;
//using SabberStoneCore.Enums;

//namespace SabberStoneCoreAi.Agent
//{
//	class FaceHunter : AbstractAgent
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

//		//public override void FinalizeGame()
//		//{
//		//	//Nothing to do here
//		//}

//		public override HearthNode PlayTurn(HearthNode state)
//		{
//			List<PlayerTask> options = state.Game.CurrentPlayer.Options();
//			return GetBestMove(state);
//		}



//		public override void InitializeGame()
//		{
//			//Nothing to do here
//		}

//		HearthNode GetBestMove(HearthNode state)
//		{
//			var minionAttacks = new LinkedList<PlayerTask>();
//			foreach (PlayerTask task in options)
//			{
//				if (task.PlayerTaskType == PlayerTaskType.MINION_ATTACK && task.Target == split._poGame._game.CurrentOpponent.Hero)
//				{
//					minionAttacks.AddLast(task);
//				}
//			}
//			if (minionAttacks.Count > 0)
//				return minionAttacks.First.Value;

//			PlayerTask summonMinion = null;
//			foreach (PlayerTask task in options)
//			{
//				if (task.PlayerTaskType == PlayerTaskType.PLAY_CARD)
//				{
//					summonMinion = task;
//				}
//			}
//			if (summonMinion != null)
//				return summonMinion;

//			else
//				return options[0];
//		}

//	}
//}
