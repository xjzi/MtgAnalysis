using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DeckAnalyzer
{
	static class LinkChecker
	{
		static readonly HttpClient client = new HttpClient();
		const string defaultTitle = "Article Archives | MAGIC: THE GATHERING";

		public static bool IsArticle(string url)
		{
			HttpRequestMessage request = new HttpRequestMessage { RequestUri = new Uri(url) };
			request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(75, 113);
			string response = client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
			return !response.Contains(defaultTitle);
		}
	}

	class ArticleFinder
	{
		const string linkBase = "https://magic.wizards.com/en/articles/archive/mtgo-standings/";
		readonly string linkBaseWithTitle;

		public ArticleFinder(string cardset, string competitionType)
		{
			linkBaseWithTitle = string.Concat(linkBase, cardset, ' ', competitionType).Replace(' ', '-').ToLower();
		}

		public IEnumerable<string> FindBySpan(DateTime start, DateTime end)
		{
			for (DateTime date = end; date >= start; date = date.AddDays(-1))
			{
				string link = GetLink(date);

				if (LinkChecker.IsArticle(link))
				{
					yield return link;
				}
			}
		}

		public IEnumerable<string> FindByQuantity(int quantity)
		{
			int quantityFound = 0;
			DateTime date = DateTime.Now;
			while (quantityFound < quantity)
			{
				date = date.AddDays(-1);
				string link = GetLink(date);
				if (LinkChecker.IsArticle(link))
				{
					quantityFound++;
					yield return link;
				}
			}
		}

		string GetLink(DateTime date)
		{
			string formattedDate = date.ToString("-yyyy-MM-dd");
			return string.Concat(linkBaseWithTitle, formattedDate);
		}
	}
}
