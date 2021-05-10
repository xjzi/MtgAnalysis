using DeckAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace FetchDecks
{
	static class Program
	{
		const string format = "modern";
		const string gametype = "challenge";

		static int Main(string[] args)
		{
			ArticleFinder finder = new ArticleFinder(format, gametype);
			switch (args[0])
			{
				case "span":
					{
						DateTime start = DateTime.ParseExact(args[1], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
						DateTime end = DateTime.ParseExact(args[2], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
						return Extract(finder.FindBySpan(start, end));
					}
				case "quantity":
					{
						return Extract(finder.FindByQuantity(int.Parse(args[1])));
					}
				case "update":
					{
						DateTime yesterday = DateTime.Now.AddDays(-1);
						return Extract (finder.FindBySpan(yesterday, yesterday));
					}
				default:
					{
						Console.WriteLine("syntax error");
						return -1;
					}
			}
		}

		static int Extract(IEnumerable<string> links)
		{
			int count = links.Count();
			Console.WriteLine("Downloading {0} articles on {1}", count, DateTime.Now);
			foreach (string link in links)
			{
				ArticleExtractor extractor = new ArticleExtractor(link);
				DeckWithRank[] decks = extractor.GetDecks().ToArray();
				IEnumerable<Game> games = GameProvider.GetGames(decks);
				DataUploader uploader = new DataUploader(decks, games);
				uploader.UploadTournament(format, gametype, DateTime.ParseExact(link[^10..], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
				uploader.UploadDecks();
				uploader.UploadGames();
			}
			return count;
		}
	}
}
