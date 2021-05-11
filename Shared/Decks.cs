using Shared;

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