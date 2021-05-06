using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Shared;

namespace FetchDecks
{
	static class NodeCleaner
	{
		public static string[] CleanInnerText(this HtmlNode node)
		{
			return node.InnerText.Trim()
					.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
					.Where(line => !string.IsNullOrWhiteSpace(line))
					.Select(line => line.Trim()).ToArray();
		}
	}

	class ArticleExtractor
	{
		const string decklistText = "/html/body/div[1]/div[2]/div/div/div/div/div/div/div[1]/article/div[2]/div[3]/*/div/div/div/div[3]/div[1]";
		const string mainboardPath = "./div[1]/*/span";
		const string sideboardPath = "./div[2]/span";

		readonly HtmlDocument root = new HtmlDocument();

		public ArticleExtractor(string url)
		{
			ChromeOptions options = new ChromeOptions();
			//Allows root user to run it
			options.AddArguments("--disable-dev-shm-usage");
			options.AddArgument("--headless");

			using IWebDriver driver = new RemoteWebDriver(new Uri("http://" + ConnectionDetails.host + ":4444/wd/hub"), options);
			driver.Navigate().GoToUrl(url);
			root.LoadHtml(driver.PageSource);
		}

		public IEnumerable<DeckWithRank> GetDecks()
		{
			HtmlNode[] deckNodes = root.DocumentNode.SelectNodes(decklistText).ToArray();
			for (int i = 0; i < deckNodes.Length; i++)
			{
				HtmlNode deckNode = deckNodes[i];

				yield return new DeckWithRank
				{
					Rank = i,
					Mainboard = GetCards(deckNode.SelectNodes(mainboardPath)).ToArray(),
					Sideboard = GetCards(deckNode.SelectNodes(sideboardPath)).ToArray()
				};
			}
		}

		IEnumerable<string> GetCards(IEnumerable<HtmlNode> entries)
		{
			foreach (HtmlNode card in entries)
			{
				string[] text = card.CleanInnerText();
				for (int i = 0; i < int.Parse(text[0]); i++)
				{
					yield return text[1];
				}
			}
		}
	}
}
