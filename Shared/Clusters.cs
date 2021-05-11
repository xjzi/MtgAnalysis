using Shared;
using System.Collections.Generic;

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