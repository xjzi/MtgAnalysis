using System;

namespace Shared
{
	public abstract class SqlReferencedItem
	{
		public int Id { get; set; }
	}

	public class Game
	{
		public Deck Winner { get; set; }
		public Deck Loser { get; set; }
	}

	public class HighlightCard
	{
		public Card Card { get; set; }
		public Cluster Cluster { get; set; }
		public double Frequency { get; set; }
	}


	public class Tournament
	{
		public string Url { get; set; }
		public string CardSet { get; set; }
		public string GameType { get; set; }
		public DateTime Date { get; set; }
	}
}