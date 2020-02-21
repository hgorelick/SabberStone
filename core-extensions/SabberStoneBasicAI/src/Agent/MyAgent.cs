//using System;
//using System.Collections.Generic;
//using System.Text;
//using SabberStoneCore.Tasks.PlayerTasks;
//using SabberStoneCoreAi.Agent;
//using SabberStoneCoreAi.POGamespace;
//using SabberStoneCoreAi.HearthNodes;
//using SabberStoneCore.Model;

//namespace SabberStoneCoreAi.Agent
//{
//	class MyAgent : AbstractAgent
//	{
//		private Random Rnd = new Random();

//		public override void FinalizeAgent()
//		{
//		}

//		public override void FinalizeGame()
//		{
//		}

//		public override PlayerTask GetMove(EndTurnNode split, int option=0)
//		{
//			return split._poGame.CurrentPlayer.Options()[0];
//		}

//		public override List<PlayerTask> GetMoves(POGame.POGame poGame)
//		{
//			return poGame.CurrentPlayer.Options();
//		}

//		public override void InitializeAgent()
//		{
//			Rnd = new Random();
//		}

//		public override void InitializeGame()
//		{
//		}

//		public override List<Deck> GetAllOppDecks(CardClass oppClass)
//		{
//			return new List<Deck>();
//		}
//	}
//}
