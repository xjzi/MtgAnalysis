using System;
using System.Collections.Generic;

namespace Scraper
{
	abstract class SqlReferencedItem
	{
		public int Id { get; set; }
	}

	class Deck : SqlReferencedItem
	{
		public string[] Mainboard { get; set; }
		public string[] Sideboard { get; set; }
	}

	class Game
	{
		public Deck Winner { get; set; }
		public Deck Loser { get; set; }
	}

	class Tournament : SqlReferencedItem
	{
		public string Url { get; set; }
		public string CardSet { get; set; }
		public string GameType { get; set; }
		public DateTime Date { get; set; }
		public IEnumerable<Deck> Decks { get; set; }
		public IEnumerable<Game> Games { get; set; }
	}
}
