using SabberStoneCore.Tasks.PlayerTasks;
using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCoreAi.POGamespace;

namespace SabberStoneCoreAi.Tyche2
{
    class TySimResult
    {	
		public POGame state;
		public PlayerTask task;
		public float value;

		public bool IsBuggy { get { return state == null; } }
		
		public TySimResult(POGame state, PlayerTask task, float value)
		{
			this.state = state;
			this.value = value;
			this.task = task;
		}
	}
}
