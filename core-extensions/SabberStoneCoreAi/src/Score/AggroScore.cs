using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model.Zones;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.Score
{
	public class AggroScore : Score
	{
		public override int Rate()
		{
			if (OpHeroHp < 1)
				return 1;

			if (HeroHp < 1)
				return 1;

			int result = 0;

			if (OpBoardZone.Count == 0 && BoardZone.Count > 0)
				result += 1;

			if (OpMinionTotHealthTaunt > 0)
				result += OpMinionTotHealthTaunt * -1;

			result += MinionTotAtk;

			result += (HeroHp - OpHeroHp);

			return result / 10;
		}

		public override Func<List<IPlayable>, List<int>> MulliganRule()
		{
			return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
		}
	}
}
