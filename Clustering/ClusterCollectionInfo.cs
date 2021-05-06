using System.Linq;
using System.Collections.Generic;
using System.Text;
using Website.Shared;

namespace Clustering
{
	class ClusterCollectionInfo
	{
		public IEnumerable<SimilarClusters> Similar { get => _similar; }
		public IEnumerable<HighlightCard> Highlights { get => _highlights; }
		public IEnumerable<Card> Cards { get => _cards; }
		public IEnumerable<Cluster> Clusters { get => _clusters; }

		readonly List<IEnumerable<Deck>> _deckLists;
		readonly int _totalDecks;
		readonly List<Cluster> _clusters;
		readonly List<SimilarClusters> _similar = new List<SimilarClusters>();
		readonly List<HighlightCard> _highlights = new List<HighlightCard>();
		readonly List<Card> _cards = new List<Card>();

		public ClusterCollectionInfo(IEnumerable<IEnumerable<Deck>> clusters)
		{
			_clusters = Enumerable.Range(0, clusters.Count()).Select(_ => new Cluster()).ToList();
			_deckLists = clusters.ToList();
			_totalDecks = _deckLists.Select(x => x.Count()).Sum();
		}

		// TODO : Load significant matchups

		// TODO : Load similar cards

		public void LoadClusterItems()
		{
			for (int k = 0; k < _deckLists.Count; k++)
			{
				DeckCollectionInfo info = new DeckCollectionInfo(_deckLists[k]);
				var (matrix, keys) = info.GetDeckSimilarities();

				//The minimum row in this array will be the most normal deck
				double minDistance = double.MaxValue;
				int middleIndex = 0;
				for (int i = 0; i < matrix.GetLength(0); i++)
				{
					double distance = 0;
					for (int j = 0; j < matrix.GetLength(1); j++)
					{
						distance += matrix[i, j];
					}
					if (distance < minDistance)
					{
						minDistance = distance;
						middleIndex = i;
					}
				}
				_clusters[k].Middle = new DeckWithCluster();
				_clusters[k].Middle.Mainboard = keys[middleIndex].Mainboard;
				_clusters[k].Middle.Sideboard = keys[middleIndex].Sideboard;
				_clusters[k].Middle.Parent = _clusters[k];
				_clusters[k].Frequency = (double)_deckLists[k].Count() / _totalDecks;
			}
		}

		public void LoadCards()
		{
			IEnumerable<Deck> decks = _deckLists.SelectMany(x => x);
			double frequencyPerCard = 1d / decks.Count();
			foreach (Deck deck in decks)
			{
				foreach (string c in deck.Mainboard.Concat(deck.Sideboard).Distinct())
				{
					Card card = _cards.FirstOrDefault(x => x.Name == c);
					if (card == null)
					{
						_cards.Add(new Card
						{
							Name = c,
							Frequency = frequencyPerCard
						});
					}
					else
					{
						card.Frequency += frequencyPerCard;
					}
				}
			}
		}

		public void LoadSimilar(int amount)
		{
			DeckCollectionInfo info = new DeckCollectionInfo(_clusters.Select(x => x.Middle));
			var (matrix, keys) = info.GetDeckSimilarities();
			for (int k = 0; k < _deckLists.Count; k++)
			{
				double[] row = new double[_deckLists.Count];
				for (int i = 0; i < _deckLists.Count; i++)
				{
					row[i] = matrix[k, i];
				}
				//Linked list to order while keeping indexes
				int[] similarIndexes = Enumerable.Range(0, _deckLists.Count).Select(x => x).OrderByDescending(x => row[x]).ToArray();

				//Don't record the deck being similar to itself
				Cluster self = _clusters[k];
				for (int i = 1; i <= amount; i++)
				{
					if (i >= similarIndexes.Count()) { break; }
					Cluster other = ((DeckWithCluster)keys[similarIndexes[i]]).Parent;
					SimilarClusters entry = new SimilarClusters()
					{
						A = other,
						B = self,
						Distance = row[similarIndexes[i]]
					};
					if (_similar.Exists(x => x.A == self && x.B == other)) { break; }
					_similar.Add(entry);
				}
			}
		}

		public void LoadHighlights(int amount)
		{
			for (int i = 0; i < _deckLists.Count; i++)
			{
				Dictionary<string, int> cards = new Dictionary<string, int>();
				foreach (Deck d in _deckLists[i])
				{
					foreach (string c in d.Mainboard.Concat(d.Sideboard).Distinct())
					{
						if (cards.ContainsKey(c)) { cards[c]++; }
						else { cards.Add(c, 1); }
					}
				}
				int totalDecksInCluster = _deckLists[i].Count();
				foreach (KeyValuePair<string, int> entry in cards.OrderByDescending(x => x.Value).Take(amount))
				{
					HighlightCard highlight = new HighlightCard()
					{
						Card = _cards.Find(x => x.Name == entry.Key),
						Cluster = _clusters[i],
						Frequency = (double)entry.Value / totalDecksInCluster
					};
					_highlights.Add(highlight);
				}
			}
		}

	}
}
