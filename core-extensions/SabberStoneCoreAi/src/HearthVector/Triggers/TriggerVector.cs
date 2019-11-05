using SabberStoneCore.Tasks;
using SabberStoneCore.Triggers;
using SabberStoneCoreAi.HearthVector.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Triggers
{
	public class TriggerVector : HearthVector
	{
		public bool EitherTurn { get; private set; } = false;
		public bool FastExecution { get; private set; } = false;
		public bool RemoveAfterTriggered { get; private set; } = false;
		public bool Removed { get; private set; } = false;
		public int SequenceType { get; private set; } = 0;
		public SimpleTaskVector[] SingleTask { get; private set; }
		public int SourceAssetId { get; private set; } = 0;
		public int TriggerActivation { get; private set; } = -1;
		public int TriggerSource { get; private set; } = -1;
		public int TriggerType { get; private set; } = 0;
		public bool Validated { get; private set; } = false;

		public static TriggerVector Create(Trigger trigger)
		{
			if (trigger == null)
				return new TriggerVector();
			return new TriggerVector(trigger);
		}

		public static List<TriggerVector> Create(List<Trigger> triggers = null)
		{
			if (triggers == null)
				return new List<TriggerVector>() { new TriggerVector() };

			var triggerVectors = new List<TriggerVector>();
			for (int i = 0; i < triggers.Count; ++i)
				triggerVectors.Add(Create(triggers[i]));

			return triggerVectors;
		}

		private TriggerVector(Trigger trigger)
		{
			EitherTurn = trigger.EitherTurn;
			FastExecution = trigger.FastExecution;
			RemoveAfterTriggered = trigger.RemoveAfterTriggered;
			Removed = trigger.Removed;
			SequenceType = (int)trigger.SequenceType;
			SingleTask = SimpleTaskVector.Create(trigger.SingleTask);
			SourceAssetId = trigger.Owner.Card.AssetId;
			TriggerActivation = (int)trigger.TriggerActivation;
			TriggerSource = (int)trigger.TriggerSource;
			TriggerType = (int)trigger.TriggerType;
			Validated = trigger.Validated;
		}

		public TriggerVector()
		{
			SingleTask = SimpleTaskVector.Create(null);
		}
	}
}
