using DeckAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace FetchDecks
{
	static class Program
	{
		const string _format = "modern";
		const string _gametype = "challenge";

		static void Main(string[] args)
		{
			Fetch(new string[] { "quantity", "1" });
		}

		static void Fetch(string[] args)
		{
			TournamentFinder finder = new TournamentFinder(_format, _gametype);
			switch (args[0])
			{
				case "quantity":
					{
						Extract(finder.FindByQuantity(int.Parse(args[1])));
						break;
					}
				case "update":
					{
						Extract(finder.Update());
						break;
					}
				default:
					{
						Console.WriteLine("syntax error");
						break;
					}
			}
		}

		static void Extract(IEnumerable<Tournament> tournaments)
		{
			Console.WriteLine("Downloading {0} articles on {1}", tournaments.Count(), DateTime.Now);
			foreach (Tournament tournament in tournaments)
			{
				ArticleExtractor extractor = new ArticleExtractor(tournament.Url);
				IEnumerable<DeckWithRank> decks = extractor.GetDecks();
				IEnumerable<Game> games = GameProvider.GetGames(decks);
				DataUploader uploader = new DataUploader(tournament, decks, games);
				uploader.UploadTournament();
				uploader.UploadDecks();
				uploader.UploadGames();
			}
		}
	}
}
