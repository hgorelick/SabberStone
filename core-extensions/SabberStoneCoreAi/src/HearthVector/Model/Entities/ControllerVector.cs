using System.Linq;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Model.Zones;
using SabberStoneCoreAi.HearthVector.Enchants;
using SabberStoneCoreAi.HearthVector.Model.Zone;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public class ControllerVector : EntityVector
	{
		#region Properties
		public int AmountHeroHealedThisTurn { get; private set; }
		public int Armor { get; private set; }
		public int BaseClass { get; private set; }
		public BoardZoneVector BoardZone { get; private set; }
		public bool ChooseBoth { get; private set; }
		public ControllerAuraEffectsVector ControllerAuraEffects { get; private set; }
		public int CurrentSpellPower { get; private set; }
		public DeckZoneVector DeckZone { get; private set; }
		public bool DragonInHand { get; private set; }
		public bool ExtraBattlecry { get; private set; }
		public bool ExtraEndTurnEffect { get; private set; }
		public GraveyardZoneVector GraveyardZone { get; private set; }
		public HandZoneVector HandZone { get; private set; }
		public HeroVector Hero { get; private set; }
		//public int HeroId { get; private set; }
		public int HeroPowerActivationsThisTurn { get; private set; }
		public bool HeroPowerDisabled { get; private set; }
		public bool IsComboActive { get; private set; }
		public int JadeGolem { get; private set; }
		public CardVector LastCardDiscarded { get; private set; }
		public CardVector LastCardDrawn { get; private set; }
		public CardVector LastCardPlayed { get; private set; }
		public int NumAttacksThisTurn { get; private set; }
		public int NumCardsDrawnThisTurn { get; private set; }
		public int NumCardsPlayedThisTurn { get; private set; }
		public int NumDiscardedThisGame { get; private set; }
		public int NumElementalsPlayedLastTurn { get; private set; }
		public int NumElementalsPlayedThisTurn { get; private set; }
		public int NumFriendlyMinionsThatAttackedThisTurn { get; private set; }
		public int NumFriendlyMinionsThatDiedThisGame { get; private set; }
		public int NumFriendlyMinionsThatDiedThisTurn { get; private set; }
		public int NumHeroPowerDamageThisGame { get; private set; }
		public int NumMinionsPlayedThisTurn { get; private set; }
		//public int NumMinionsPlayerKilledThisGame { get; private set; }
		public int NumMinionsPlayerKilledThisTurn { get; private set; }
		public int NumMurlocsPlayedThisGame { get; private set; }
		public int NumOptionsPlayedThisTurn { get; private set; }
		public int NumSecretsPlayedThisGame { get; private set; }
		public int NumSpellsPlayedThisGame { get; private set; }
		public int NumTimesHeroPowerUsedThisGame { get; private set; }
		public int NumTotemSummonedThisGame { get; private set; }
		public int NumTurnsLeft { get; private set; }
		public int NumWeaponsPlayedThisGame { get; private set; }
		public int OverloadLocked { get; private set; }
		public int OverloadOwed { get; private set; }
		public int OverloadThisGame { get; private set; }
		public int PlayerId { get; private set; }
		//public int PlayState { get; private set; }
		public CardVector ProxyCthun { get; private set; }
		public int RemainingMana { get; private set; }
		public bool RestoreToDamage { get; private set; }
		public SecretZoneVector SecretZone { get; private set; }
		public bool SeenCthun { get; private set; }
		public int TemporaryMana { get; private set; }
		public bool TemporusFlag { get; private set; }
		public int TimeOut { get; private set; }
		public int TotalManaSpentThisGame { get; private set; }
		public int UsedMana { get; private set; }
		#endregion

		public ControllerVector(Controller c) : base(c)
		{
			AmountHeroHealedThisTurn = c.AmountHeroHealedThisTurn;
			Armor = c.Hero.Armor;
			BaseClass = (int)c.BaseClass;
			BoardZone = new BoardZoneVector(c.BoardZone);
			ChooseBoth = c.ChooseBoth;
			ControllerAuraEffects = ControllerAuraEffectsVector.Create(c.ControllerAuraEffects);
			CurrentSpellPower = c.CurrentSpellPower;
			DeckZone = new DeckZoneVector(c.DeckZone);
			DragonInHand = c.DragonInHand;
			ExtraBattlecry = c.ExtraBattlecry;
			ExtraEndTurnEffect = c.ExtraEndTurnEffect;
			GraveyardZone = new GraveyardZoneVector(c.GraveyardZone);
			HandZone = new HandZoneVector(c.HandZone);
			Hero = new HeroVector(c.Hero);
			//HeroId = c.HeroId;
			HeroPowerActivationsThisTurn = c.HeroPowerActivationsThisTurn;
			HeroPowerDisabled = c.HeroPowerDisabled;
			IsComboActive = c.IsComboActive;
			JadeGolem = c.JadeGolem;
			LastCardDiscarded = CardVector.Create(c.Game.IdEntityDic[c.LastCardDiscarded]);
			LastCardDrawn = CardVector.Create(c.Game.IdEntityDic[c.LastCardDrawn]);
			LastCardPlayed = CardVector.Create(c.Game.IdEntityDic[c.LastCardPlayed]);
			NumAttacksThisTurn = c.NumAttacksThisTurn;
			NumCardsDrawnThisTurn = c.NumCardsDrawnThisTurn;
			NumCardsPlayedThisTurn = c.NumCardsPlayedThisTurn;
			NumDiscardedThisGame = c.NumDiscardedThisGame;
			NumElementalsPlayedLastTurn = c.NumElementalsPlayedLastTurn;
			NumElementalsPlayedThisTurn = c.NumElementalsPlayedThisTurn;
			NumFriendlyMinionsThatAttackedThisTurn = c.NumFriendlyMinionsThatAttackedThisTurn;
			NumFriendlyMinionsThatDiedThisGame = c.NumFriendlyMinionsThatDiedThisGame;
			NumFriendlyMinionsThatDiedThisTurn = c.NumFriendlyMinionsThatDiedThisTurn;
			NumHeroPowerDamageThisGame = c.NumHeroPowerDamageThisGame;
			NumMinionsPlayedThisTurn = c.NumMinionsPlayedThisTurn;
			NumMinionsPlayerKilledThisTurn = c.NumMinionsPlayerKilledThisTurn;
			NumMurlocsPlayedThisGame = c.NumMurlocsPlayedThisGame;
			NumOptionsPlayedThisTurn = c.NumOptionsPlayedThisTurn;
			NumSecretsPlayedThisGame = c.NumSecretsPlayedThisGame;
			NumSpellsPlayedThisGame = c.NumSpellsPlayedThisGame;
			NumTimesHeroPowerUsedThisGame = c.NumTimesHeroPowerUsedThisGame;
			NumTotemSummonedThisGame = c.NumTotemSummonedThisGame;
			NumTurnsLeft = c.NumTurnsLeft;
			NumWeaponsPlayedThisGame = c.NumWeaponsPlayedThisGame;
			OverloadLocked = c.OverloadLocked;
			OverloadOwed = c.OverloadOwed;
			OverloadThisGame = c.OverloadThisGame;
			PlayerId = c.PlayerId;
			//PlayState = (int)c.PlayState;
			ProxyCthun = CardVector.Create(c.Game.IdEntityDic[c.ProxyCthun]);
			RemainingMana = c.RemainingMana;
			RestoreToDamage = c.RestoreToDamage;
			SecretZone = new SecretZoneVector(c.SecretZone);
			SeenCthun = c.SeenCthun;
			TemporaryMana = c.TemporaryMana;
			TemporusFlag = c.TemporusFlag;
			TimeOut = c.TimeOut;
			TotalManaSpentThisGame = c.TotalManaSpentThisGame;
			UsedMana = c.UsedMana;

			///foreach (string propName in GetPropertyNames())
			///{
			///	if (SetSpecialProperty(c, propName))
			///		continue;
			///
			///	else if (c.GetType().GetProperties().Select(pi => pi.Name).ToList().Contains(propName))
			///	{
			///		object prop = c.GetType().GetProperty(propName).GetValue(c);
			///		GetType().GetProperty(propName).SetValue(this, prop);
			///	}
			///}
		}

		public bool SetSpecialProperty(Controller c, string propName)
		{
			switch (propName)
			{
				case "Armor":
					Armor = c.Hero.Armor;
					return true;
				case "AppliedEnchantments":
					AppliedEnchantments = EnchantmentVector.Create(c.AppliedEnchantments);
					return true;
				case "BaseClass":
					BaseClass = (int)c.BaseClass;
					return true;
				case "BoardZone":
					BoardZone = new BoardZoneVector((BoardZone)c.GetType().GetProperty(propName).GetValue(c));
					return true;
				case "ControllerAuraEffects":
					ControllerAuraEffects = ControllerAuraEffectsVector.Create(c.ControllerAuraEffects);
					return true;
				case "DeckZone":
					DeckZone = new DeckZoneVector((DeckZone)c.GetType().GetProperty(propName).GetValue(c));
					return true;
				case "GraveyardZone":
					GraveyardZone = new GraveyardZoneVector((GraveyardZone)c.GetType().GetProperty(propName).GetValue(c));
					return true;
				case "HandZone":
					HandZone = new HandZoneVector((HandZone)c.GetType().GetProperty(propName).GetValue(c));
					return true;
				case "Hero":
					Hero = new HeroVector(c.Hero);
					return true;
				case "LastCardDiscarded":
					LastCardDiscarded = CardVector.Create(c.Game.IdEntityDic[c.LastCardDiscarded]);
					return true;
				case "LastCardDrawn":
					LastCardDrawn = CardVector.Create(c.Game.IdEntityDic[c.LastCardDrawn]);
					return true;
				case "LastCardPlayed":
					LastCardPlayed = CardVector.Create(c.Game.IdEntityDic[c.LastCardPlayed]);
					return true;
				//case "PlayState":
				//	PlayState = (int)c.PlayState;
				//	return true;
				case "ProxyCthun":
					ProxyCthun = new MinionVector((Minion)c.Game.IdEntityDic[c.ProxyCthun]);
					return true;
				case "SecretZone":
					SecretZone = new SecretZoneVector((SecretZone)c.GetType().GetProperty(propName).GetValue(c));
					return true;
				default:
					return false;
			}
		}
	}

}
