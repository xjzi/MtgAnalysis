using System;
using System.Collections.Generic;
using System.Linq;

namespace Scraper
{
	static class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 1 && int.TryParse(args[0], out int days))
			{
				Scrape(days);
			}
			else
			{
				Console.WriteLine("Invalid parameters.");
				Console.WriteLine("Running default.");
				Scrape(1);
			}
		}

		static void Scrape(int days)
		{
			TournamentFinder finder = new TournamentFinder();
			IEnumerable<Tournament> tournaments = finder.Back(days).ToArray();

			Console.WriteLine("Downloading {0} articles on {1}", tournaments.Count(), DateTime.Now);

			foreach (Tournament tournament in tournaments)
			{
				ArticleExtractor extractor = new ArticleExtractor(tournament.Url);

				tournament.Decks = extractor.GetDecks().ToArray();

				if (tournament.GameType == "challenge")
				{
					GameProvider gameProvider = new GameProvider(tournament.Decks);
					tournament.Games = gameProvider.GetGames().ToArray();
				}
				else
				{
					tournament.Games = Enumerable.Empty<Game>();
				}
			}

			DataUploader uploader = new DataUploader(tournaments);
			uploader.Upload();
			uploader.Dispose();
		}
	}
}
