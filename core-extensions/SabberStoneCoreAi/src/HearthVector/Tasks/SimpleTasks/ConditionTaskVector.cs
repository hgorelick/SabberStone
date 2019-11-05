using SabberStoneCore.Tasks.SimpleTasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class ConditionTaskVector : SimpleTaskVector
	{
		public bool ConditionsMet { get; private set; }
		public int Type { get; private set; }

		public ConditionTaskVector(ConditionTask conditionTask, Controller c, IEntity source,
			IPlayable target,
			TaskStack stack = null) : base(conditionTask)
		{
			Type = (int)conditionTask.Type;

			IList<IPlayable> entities = IncludeTask.GetEntities(conditionTask.Type, c, source, target, stack?.Playables);

			if (entities.Count == 0)
			{
				ConditionsMet = false;
				return;
			}

			var pSource = (IPlayable)source;

			foreach (IPlayable p in entities)
			{
				if (!conditionTask.SelfConditions.All(sc => sc.Eval(p)))
				{
					ConditionsMet = false;
					return;
				}

				if (!conditionTask.RelaConditions.All(sc => sc.Eval(pSource, p)))
				{
					ConditionsMet = false;
					return;
				}
			}

			ConditionsMet = true;
		}
	}

	public class NumberConditionTaskVector : SimpleTaskVector
	{
		public int Reference { get; private set; }
		public int RelaSign { get; private set; }

		public NumberConditionTaskVector(NumberConditionTask numberConditionTask) : base(numberConditionTask)
		{
			Reference = numberConditionTask.Reference;
			RelaSign = (int)numberConditionTask.Sign;
		}
	}

	public class FilterStackTaskVector : SimpleTaskVector
	{
		public int NumFiltered { get; private set; }
		public int Type { get; private set; }

		public FilterStackTaskVector(FilterStackTask filterStackTask, Controller c, IEntity source,
			IPlayable target,
			TaskStack stack = null) : base(filterStackTask)
		{
			Type = (int)filterStackTask.Type;

			var filtered = new List<IPlayable>(stack.Playables.Count);

			if (filterStackTask.RelaConditions != null)
			{
				IList<IPlayable> entities = IncludeTask.GetEntities(filterStackTask.Type, c, source, target, stack?.Playables);

				if (entities.Count == 0)
				{
					NumFiltered = 0;
					return;
				}

				foreach (IPlayable p in stack.Playables)
				{
					if (filterStackTask.RelaConditions.All(rc => rc.Eval(entities[0], p)))
						filtered.Add(p);
				}

				NumFiltered = filtered.Count;
				return;
			}

			if (filterStackTask.SelfConditions != null)
			{
				foreach (IPlayable p in stack?.Playables)
				{
					if (filterStackTask.SelfConditions.All(rc => rc.Eval(p)))
						filtered.Add(p);
				}

				NumFiltered = filtered.Count;
				return;
			}
		}
	}

}
