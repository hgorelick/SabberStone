using SabberStoneCore.Enums;
using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.SimpleTasks;
using System.Collections.Generic;

namespace SabberStoneCoreAi.HearthVector.Tasks.SimpleTasks
{
	public class GetGameTagTaskVector : SimpleTaskVector
	{
		public int TagValue { get; private set; }
		public int Type { get; private set; }

		public GetGameTagTaskVector(GetGameTagTask gameTagTask, Controller c, IEntity source,
			IPlayable target,
			TaskStack stack = null) : base(gameTagTask)
		{
			Type = (int)gameTagTask.Type;
			TagValue = 0;

			IList<IPlayable> entities = IncludeTask.GetEntities(gameTagTask.Type, in c, source, target, stack?.Playables);

			if (entities == null || entities.Count == 0 || entities.Count <= gameTagTask.EntityIndex)
				return;

			if (gameTagTask.Tag == GameTag.ENTITY_ID)
				TagValue = entities[gameTagTask.EntityIndex].Id;

			else if (entities[gameTagTask.EntityIndex] is Character ch)
				switch (gameTagTask.Tag)
				{
					case GameTag.ATK:
						TagValue = ch.AttackDamage;
						return;
					case GameTag.HEALTH:
						TagValue = ch.BaseHealth;
						return;
					case GameTag.DAMAGE:
						TagValue = ch.Damage;
						return;
					case GameTag.EXTRA_ATTACKS_THIS_TURN:
						if (ch is Hero h)
							TagValue = h.ExtraAttacksThisTurn;
						else
							TagValue = 0;
						return;
					default:
						TagValue = ch[gameTagTask.Tag];
						return;
				}

			else
				TagValue = entities[gameTagTask.EntityIndex][gameTagTask.Tag];
		}
	}

	public class GetEventNumberTaskVector : SimpleTaskVector
	{
		public int NumberIndex { get; private set; }

		public GetEventNumberTaskVector(GetEventNumberTask getEventNumberTask) : base(getEventNumberTask)
		{
			NumberIndex = getEventNumberTask.NumberIndex;
		}
	}
}
