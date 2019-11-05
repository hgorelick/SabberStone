using SabberStoneCore.Tasks.SimpleTasks;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ManaCrystalFullTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool Both { get; private set; }

		public ManaCrystalFullTaskVector(ManaCrystalFullTask manaCrystalFullTask) : base(manaCrystalFullTask)
		{
			Amount = manaCrystalFullTask.Amount;
			Both = manaCrystalFullTask.Both;
		}
	}

	public class ManaCrystalEmptyTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool Opponent { get; private set; }
		public bool UseNumber { get; private set; }

		public ManaCrystalEmptyTaskVector(ManaCrystalEmptyTask manaCrystalEmptyTask) : base(manaCrystalEmptyTask)
		{
			Amount = manaCrystalEmptyTask.Amount;
			Opponent = manaCrystalEmptyTask.Opponent;
			UseNumber = manaCrystalEmptyTask.UseNumber;
		}
	}

	public class ManaCrystalSetTaskVector : SimpleTaskVector
	{
		public int Amount { get; private set; }
		public bool Both { get; private set; }

		public ManaCrystalSetTaskVector(ManaCrystalSetTask manaCrystalSetTask) : base(manaCrystalSetTask)
		{
			Amount = manaCrystalSetTask.Amount;
			Both = manaCrystalSetTask.Both;
		}
	}
}
