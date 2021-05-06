using System.Collections.Generic;

namespace Website.Shared
{
	public abstract class SqlReferencedItem
	{
		public int Id { get; set; }
	}

	public class Cluster : SqlReferencedItem
	{
		public double Frequency { get; set; }
		public DeckWithCluster Middle { get; set; }
	}

	public class ClusterPreview
	{
		public List<string> Highlights { get; set; }
		public double Frequency { get; set; }
		public int ClusterId { get; set; }
	}

	public class Deck : SqlReferencedItem
	{
		public string[] Mainboard { get; set; }
		public string[] Sideboard { get; set; }
	}

	public class DeckWithCluster : Deck
	{
		public Cluster Parent { get; set; }
	}

	public class DeckWithRank : Deck
	{
		public int Rank { get; set; }
	}

	public class Card
	{
		public string Name { get; set; }
		public double Frequency { get; set; }
	}

	public class Game
	{
		public Deck Winner { get; set; }
		public Deck Loser { get; set; }
	}

	public class SignificantMatchup
	{
		public Cluster Winner { get; set; }
		public Cluster Loser { get; set; }
		public double Winrate { get; set; }
	}

	public class SimilarClusters
	{
		public Cluster A { get; set; }
		public Cluster B { get; set; }
		public double Distance { get; set; }
	}

	public class HighlightCard
	{
		public Card Card { get; set; }
		public Cluster Cluster { get; set; }
		public double Frequency { get; set; }
	}

	public class SimilarCards
	{
		public Card A { get; set; }
		public Card B { get; set; }
		public int Distance { get; set; }
	}
}