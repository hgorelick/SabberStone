using System;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.Score;

namespace SabberStoneCoreAi.Agent
{
	public abstract class AbstractAgent
	{
		public string filename = "Sabber";
		protected IScore Score;

		public abstract void InitializeAgent();

		public abstract void InitializeGame();

		public virtual HearthNode PlayTurn(HearthNode state) { throw new NotImplementedException(); }

		public abstract void FinalizeGame();

		public abstract void FinalizeAgent();

		public void SetScoringMethod(Score.Score method)
		{
			Score = method;
		}

		//public virtual void WriteGames(CardClass oppClass = 0) { throw new NotImplementedException(); }

		//public virtual void ReadGames(int numGames = 0, CardClass oppClass = 0) { throw new NotImplementedException(); }
	}
}
