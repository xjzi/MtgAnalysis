﻿using AngleSharp;
using AngleSharp.Dom;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FetchDecks
{
	static class HtmlCleaner
	{
		public static string[] HtmlClean(this string element)
		{
			return element.Trim()
				 .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
				 .Where(line => !string.IsNullOrWhiteSpace(line))
				 .Select(line => line.Trim()).ToArray();
		}
	}

	class ArticleExtractor
	{
		static readonly HttpClient http = new HttpClient();
		static readonly IBrowsingContext context = BrowsingContext.New(Configuration.Default);
		readonly IDocument document;

		public ArticleExtractor(string url)
		{
			string source = http.GetStringAsync(url).Result;
			document = context.OpenAsync(req => req.Content(source)).Result;
		}

		public IEnumerable<DeckWithRank> GetDecks()
		{
			IElement[] decks = document.QuerySelectorAll(".decklists > div").ToArray();

			for (int i = 0; i < decks.Length; i++)
			{
				IElement mainboard = decks[i].QuerySelector(".sorted-by-overview-container");
				IElement sideboard = decks[i].QuerySelector(".sorted-by-sideboard-container");

				yield return new DeckWithRank
				{
					Rank = i,
					Mainboard = GetCards(mainboard).ToArray(),
					Sideboard = GetCards(sideboard).ToArray()
				};
			}
		}

		IEnumerable<string> GetCards(IElement container)
		{
			IEnumerable<string[]> cards = container.QuerySelectorAll("*.row").Select(x => x.TextContent.HtmlClean());
			foreach (string[] card in cards)
			{
				for (int i = 0; i < int.Parse(card[0]); i++)
				{
					yield return card[1];
				}
			}
		}
	}
}
