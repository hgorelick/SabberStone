using System.Linq;
using SabberStoneCore.Tasks;
using SabberStoneCore.Tasks.SimpleTasks;
using System;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;

namespace SabberStoneCoreAi.HearthVector
{
	public class ValueGetter
	{
		internal static readonly ValueGetter GetArmor =
			new ValueGetter(st => ((ArmorTask)st).Amount);

		internal static readonly ValueGetter GetDamage =
			new ValueGetter(st => ((DamageTask)st).Amount);

		internal static readonly ValueGetter GetDiscard =
			new ValueGetter(st => ((RandomTask)st).Amount);

		internal static readonly ValueGetter GetDraw =
			new ValueGetter(st => ((DrawTask)st).Count);

		internal static readonly ValueGetter GetHeal =
			new ValueGetter(st => ((HealTask)st).Amount);

		internal static readonly ValueGetter GetRandom =
			new ValueGetter(st => Convert.ToInt32(st is RandomTask));

		readonly Func<ISimpleTask, int> _function;

		internal ValueGetter(Func<ISimpleTask, int> function)
		{
			_function = function;
		}

		internal int GetAmount(ISimpleTask simpleTask)
		{
			return _function(simpleTask);
		}

		internal int GetAmount<T>(ISimpleTask simpleTask, Controller c = null)
		{
			var stateTaskList = simpleTask as StateTaskList;
			if (stateTaskList.TaskList.Any(t => t is T))
			{
				if (typeof(T) == typeof(DiscardTask))
				{
					var discardTask = (DiscardTask)stateTaskList.TaskList.Where(t => t is DiscardTask).ToList()[0];

					if (stateTaskList.TaskList.Any(t => t is RandomTask))
						return _function(stateTaskList.TaskList.Where(t => t is RandomTask).ToList()[0]);

					else if (discardTask.Type == EntityType.STACK)
						return 1;

					else if (discardTask.Type == EntityType.HAND && c != null)
						return c.HandZone.Count;
				}

				return _function(stateTaskList.TaskList.Where(t => t is T).ToList()[0]);
			}
			return 0;
		}

		internal static bool ControllerNeeded<T>(ISimpleTask simpleTask)
		{
			var stateTaskList = simpleTask as StateTaskList;
			if (stateTaskList.TaskList.Any(t => t is T))
			{
				if (typeof(T) == typeof(DiscardTask))
				{
					var discardTask = (DiscardTask)stateTaskList.TaskList.Where(t => t is DiscardTask).ToList()[0];
					return discardTask.Type == EntityType.HAND;
				}
			}

			return false;
		}

		//internal int GetAmount<T>(Controller c, ISimpleTask simpleTask, IZone zone)
		//{
		//	var stateTaskList = simpleTask as StateTaskList;
		//	if (stateTaskList.TaskList.Any(t => t is T))
		//		return _function(stateTaskList.TaskList.Where(t => t is T).ToList()[0]);
		//	return 0;
		//}
	}
}
