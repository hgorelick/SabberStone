using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;
using System;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class RecruitTaskVector : SimpleTaskVector
	{
		public bool AddToStack { get; private set; }
		public int Amount { get; private set; }
		public bool ConditionsMet { get; private set; }

		public RecruitTaskVector(RecruitTask recruitTask, Controller c) : base(recruitTask)
		{
			AddToStack = recruitTask.AddToStack;
			Amount = recruitTask.Amount;
			ConditionsMet = false;

			ReadOnlySpan<IPlayable> deck = c.DeckZone.GetSpan();

			int numMeetingConditions = 0;
			for (int i = 0; i < deck.Length; i++)
			{
				if (!(deck[i] is Minion))
					continue;

				bool flag = true;
				for (int j = 0; j < recruitTask.Conditions?.Length; j++)
					flag &= recruitTask.Conditions[j].Eval(deck[i]);

				if (flag)
					numMeetingConditions++;

				if (numMeetingConditions >= Amount)
				{
					ConditionsMet = true;
					return;
				}
			}
		}
	}
}
