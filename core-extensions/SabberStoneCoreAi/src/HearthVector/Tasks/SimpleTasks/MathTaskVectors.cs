using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class MathNumberIndexTaskVector : SimpleTaskVector
	{
		public int IndexA { get; private set; }
		public int IndexB { get; private set; }
		public int MathOperation { get; private set; }
		public int ResultIndex { get; private set; }

		public MathNumberIndexTaskVector(MathNumberIndexTask mathNumberIndexTask)
		{
			IndexA = mathNumberIndexTask.IndexA;
			IndexB = mathNumberIndexTask.IndexB;
			MathOperation = (int)mathNumberIndexTask.MathOperation;
			ResultIndex = mathNumberIndexTask.ResultIndex;
		}
	}

	public class MathRandTaskVector : SimpleTaskVector
	{
		public int Max { get; private set; }
		public int Min { get; private set; }

		public MathRandTaskVector(MathRandTask mathRandTask) : base(mathRandTask)
		{
			Max = mathRandTask.Max;
			Min = mathRandTask.Min;
		}
	}

	public class MathMultiplyTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Operation { get; private set; }

		public MathMultiplyTaskVector(MathMultiplyTask mathMultiplyTask) : base(mathMultiplyTask)
		{
			Amount = mathMultiplyTask.Amount;
			Operation = (int)MathOperation.MUL;
		}
	}

	public class MathAddTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Operation { get; private set; }

		public MathAddTaskVector(MathAddTask mathAddTask) : base(mathAddTask)
		{
			Amount = mathAddTask.Amount;
			Operation = (int)MathOperation.ADD;
		}
	}

	public class MathSubtractionTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public int Operation { get; private set; }
		//public int TagValue { get; private set; }
		//public int Type { get; private set; }

		public MathSubtractionTaskVector(MathSubtractionTask mathSubtractionTask/*, Controller c, IEntity source,
			IPlayable target,
			TaskStack stack = null*/) : base(mathSubtractionTask)
		{
			Amount = mathSubtractionTask.Amount;
			Operation = (int)MathOperation.SUB;
			//Type = (int)mathSubtractionTask.Type;

			//TagValue = IncludeTask.GetEntities(mathSubtractionTask.Type, c, source, target, stack?.Playables)[0][Tag];
		}
	}
}
