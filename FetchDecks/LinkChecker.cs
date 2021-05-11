using System;
using System.Net.Http;

namespace DeckAnalyzer
{
	static class LinkChecker
	{
		static readonly HttpClient _client = new HttpClient();
		const string _defaultTitle = "Article Archives | MAGIC: THE GATHERING";

		public static bool IsArticle(string url)
		{
			HttpRequestMessage request = new HttpRequestMessage { RequestUri = new Uri(url) };
			request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(75, 113);
			string response = _client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
			return !response.Contains(_defaultTitle);
		}
	}
}
