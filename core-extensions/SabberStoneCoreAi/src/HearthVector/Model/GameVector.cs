using SabberStoneCore.Model;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;
using SabberStoneCoreAi.HearthNodes;
using SabberStoneCoreAi.HearthVector.Auras;
using SabberStoneCoreAi.HearthVector.Enchants;
using SabberStoneCoreAi.HearthVector.Model.Entities;
using SabberStoneCoreAi.HearthVector.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SabberStoneCoreAi.HearthVector.Model
{
	public class GameVector : HearthVector
	{
		public List<AuraVector> Auras { get; private set; }
		public List<int> DeadMinionAssetIds { get; private set; }
		public int NumMinionsKilledThisTurn { get; private set; } = 0;
		public List<EnchantmentVector> OneTurnEffectEnchantments { get; private set; }
		public EffectVector[] OneTurnEffects { get; private set; }
		public ControllerVector Player1 { get; private set; }
		public ControllerVector Player2 { get; private set; }
		public int ProposedAttackerAssetId { get; private set; } = 0;
		public int ProposedDefenderAssetId { get; private set; } = 0;
		public int State { get; private set; } = 0;
		public int Step { get; private set; } = 0;
		public List<TriggerVector> Triggers { get; private set; }
		public int Turn { get; private set; } = 0;

		public GameVector(HearthNode hearthNode)
		{
			Auras = AuraVector.Create(hearthNode.Game.Auras);
			DeadMinionAssetIds = hearthNode.Game.DeadMinions.Select(m => m.Card.AssetId).ToList();
			NumMinionsKilledThisTurn = hearthNode.Game.NumMinionsKilledThisTurn;
			OneTurnEffectEnchantments = EnchantmentVector.Create(hearthNode.Game.OneTurnEffectEnchantments);
			OneTurnEffects = EffectVector.Create(hearthNode.Game.OneTurnEffects.Select(e => e.effect).ToArray());
			Player1 = new ControllerVector(hearthNode.Game.Player1);
			Player2 = new ControllerVector(hearthNode.Game.Player2);
			ProposedAttackerAssetId = hearthNode.Game.IdEntityDic[hearthNode.Game.ProposedAttacker].Card.AssetId;
			ProposedDefenderAssetId = hearthNode.Game.IdEntityDic[hearthNode.Game.ProposedDefender].Card.AssetId;
			State = (int)hearthNode.Game.State;
			Step = (int)hearthNode.Game.Step;
			Triggers = TriggerVector.Create(hearthNode.Game.Triggers);
			Turn = hearthNode.Game.Turn;
		}
	}
}
