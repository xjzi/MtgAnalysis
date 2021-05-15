using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tomlyn;
using Tomlyn.Model;

namespace FetchDecks
{
	class TournamentFinder
	{
		//Schedule is in other file
		readonly IEnumerable<string> _cardSets;
		readonly IEnumerable<string> _gameTypes;
		readonly HttpClient _client = new HttpClient();

		public TournamentFinder()
		{
			string conf = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "targets.conf"));
			TomlTable toml = Toml.Parse(conf).ToModel();
			_gameTypes = ((TomlArray)toml["gametypes"]).Cast<string>();
			_cardSets = ((TomlArray)toml["cardsets"]).Cast<string>();
		}

		public IEnumerable<Tournament> Back(int days)
		{
			DateTime first = DateTime.Now.AddDays(-1).Date;
			DateTime last = first.AddDays(-days);
			for (DateTime i = first; i > last; i = i.AddDays(-1))
			{
				foreach (Tournament tournament in GetTargets(i))
				{
					yield return tournament;
				}
			}
		}

		IEnumerable<Tournament> GetTargets(DateTime date)
		{
			Tournament[] all = AllTargets(date).ToArray();
			IEnumerable<Task<bool>> checks = all.Select(x => IsArticle(x.Url));
			Task.WaitAll(Task.WhenAll(checks));
			bool[] results = checks.Select(x => x.Result).ToArray();

			for (int i = 0; i < all.Length; i++)
			{
				if (results[i]) { yield return all[i]; }
			}
		}

		IEnumerable<Tournament> AllTargets(DateTime date)
		{
			foreach (string cardSet in _cardSets)
			{
				foreach (string gameType in _gameTypes)
				{
					Tournament tournament = new Tournament
					{
						CardSet = cardSet,
						GameType = gameType,
						Date = date
					};

					const string linkBase = "https://magic.wizards.com/en/articles/archive/mtgo-standings/";
					string formattedDate = tournament.Date.ToString("yyyy-MM-dd");
					string url = string.Concat(linkBase, tournament.CardSet, '-', tournament.GameType, '-', formattedDate);
					tournament.Url = url.Replace(' ', '-').ToLower();

					yield return tournament;
				}
			}
		}

		async Task<bool> IsArticle(string url)
		{
			const string defaultTitle = "Article Archives | MAGIC: THE GATHERING";

			HttpRequestMessage request = new HttpRequestMessage { RequestUri = new Uri(url) };
			request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(75, 113);

			HttpResponseMessage response = await _client.SendAsync(request);
			string content = await response.Content.ReadAsStringAsync();

			return !content.Contains(defaultTitle);
		}
	}
}
