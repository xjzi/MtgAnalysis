using System.Collections.Generic;
using System.Linq;

namespace Scraper
{
	class GameProvider
	{
		readonly Deck[] _decks;

		public GameProvider(IEnumerable<Deck> decks)
		{
			_decks = decks.ToArray();
		}

		public IEnumerable<Game> GetGames()
		{
			for (int i = 32; i > 1; i /= 2)
			{
				for (int j = 0; j < i / 2; j++)
				{
					yield return new Game
					{
						Winner = _decks[j],
						Loser = _decks[i - j - 1]
					};
				}
			}
		}
	}
}
