using System.Collections.Generic;
using System.Data;
using System.Linq;
using Website.Shared;

namespace Clustering
{
	class Program
	{
		static void Main()
		{
			DeckImporter importer = new DeckImporter();
			importer.LoadDecks();
			importer.LoadGames();

			IEnumerable<Deck> decks = importer.Decks;
			IEnumerable<Game> games = importer.Games;

			DeckCollectionInfo deckCollection = new DeckCollectionInfo(decks);
			IDistanceMetric<Deck> deckDistances = deckCollection.GetDistanceMatrix();

			ClusterFinder<Deck> finder = new ClusterFinder<Deck>(deckDistances, decks.ToList(), 13, 5);
			List<List<Deck>> clusters = finder.GetClusters().ToList();

			ClusterCollectionInfo clusterCollection = new ClusterCollectionInfo(clusters);
			clusterCollection.LoadClusterItems();
			clusterCollection.LoadSimilar(5);
			clusterCollection.LoadCards();
			clusterCollection.LoadHighlights(20);

			Exporter exporter = new Exporter();

			exporter.CleanExistingTables();
			exporter.ExportClusters(clusterCollection.Clusters);
			exporter.ExportCards(clusterCollection.Cards);
			exporter.ExportHighlights(clusterCollection.Highlights);
			exporter.ExportSimilarClusters(clusterCollection.Similar);
		}
	}
}