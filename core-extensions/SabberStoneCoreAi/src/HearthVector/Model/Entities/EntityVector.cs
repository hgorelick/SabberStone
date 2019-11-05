using System.Text;
using System.Linq;
using SabberStoneCore.Model.Entities;
using System;
using SabberStoneCore.Model;
using System.Collections.Generic;

namespace SabberStoneCoreAi.HearthVector.Model.Entities
{
	public abstract class EntityVector : HearthVector
	{
		public int AssetId { get; protected set; } = 0;
		public List<EnchantmentVector> AppliedEnchantments { get; protected set; }
		//public bool ConditionsMet { get; private set; }
		public int HeroClass { get; protected set; } = 0;

		public EntityVector(IEntity e)
		{
			AssetId = e.Card.AssetId;
			HeroClass = (int)e.Card.Class;
		}

		public EntityVector(Card c)
		{
			AssetId = c.AssetId;
			HeroClass = (int)c.Class;
		}

		public EntityVector() : base()
		{
			AppliedEnchantments = EnchantmentVector.Create();
			AssetId = 0;
			HeroClass = 0;
		}

		public static EntityVector Create(IEntity e)
		{
			if (e is Controller)
				return new ControllerVector((Controller)e);

			if (e is IPlayable)
				return CardVector.Create((IPlayable)e);

			throw new NotImplementedException();
		}
	}
}
