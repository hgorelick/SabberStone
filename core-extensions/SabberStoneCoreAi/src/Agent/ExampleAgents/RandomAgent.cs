using System;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.Utils;

namespace SabberStoneCoreAi.Agent.ExampleAgents
{
	class RandomAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void FinalizeAgent()
		{
			//Nothing to do here
		}

		public override void FinalizeGame()
		{
			//Nothing to do here
		}

		public override HearthNode PlayTurn(HearthNode state)
		{
			var rnd = new Random();
			int rndInd = rnd.Next(state.Frontier.Count);

			HearthNode selected = state.Frontier[rndInd];
			state.BirthPossibility(selected);
			//selected.Game.PowerHistory.Dump(@"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\SabberStone.log");
			//selected.Write(filename, false, false, true);

			return selected;
		}

		public override void InitializeGame()
		{
			//Nothing to do here
		}
	}
}
