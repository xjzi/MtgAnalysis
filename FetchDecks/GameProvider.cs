using System.Collections.Generic;
using System.Linq;
using Shared;

namespace FetchDecks
{
	static class GameProvider
	{
		public static IEnumerable<Game> GetGames(IEnumerable<DeckWithRank> decks)
		{
			for (int i = 32; i > 1; i /= 2)
			{
				for (int j = 0; j < i / 2; j++)
				{
					yield return new Game
					{
						Winner = decks.First(x => x.Rank == j),
						Loser = decks.First(x => x.Rank == i - j - 1)
					};
				}
			}
		}
	}
}
