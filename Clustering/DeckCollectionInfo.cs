using System.Collections.Generic;
using System.Linq;
using Website.Shared;

namespace Clustering
{
	class DistanceMatrix : IDistanceMetric<Deck>
	{
		public int[,] matrix;
		public List<Deck> indexes;

		public double DistanceSquared(Deck d1, Deck d2)
		{
			int i1 = indexes.FindIndex(x => x == d1);
			int i2 = indexes.FindIndex(x => x == d2);

			return matrix[i1, i2];
		}
	}

	class DeckCollectionInfo
	{
		public DeckCollectionInfo(IEnumerable<Deck> decks)
		{
			LoadMatrix(decks.ToArray());
		}

		int numDecks;
		int numCards;
		ushort[][] decksByCards;
		List<string> cardIndexes;
		List<Deck> deckIndexes;

		void LoadMatrix(Deck[] decks)
		{
			//Already sorted by decks which makes populating the matrix way faster
			cardIndexes = decks.SelectMany((Deck x) => x.Mainboard.Concat(x.Sideboard)).Distinct().ToList();
			deckIndexes = decks.ToList();
			numDecks = deckIndexes.Count;
			numCards = cardIndexes.Count;

			decksByCards = new ushort[numDecks][];

			for (int i = 0; i < numDecks; i++)
			{
				decksByCards[i] = new ushort[numCards];
				for (int j = 0; j < numCards; j++)
				{
					decksByCards[i][j] = (ushort)deckIndexes[i].Mainboard.Concat(deckIndexes[i].Sideboard).Where(x => x == cardIndexes[j]).Count();
				}
			}
		}

		public (double[,] matrix, List<Deck> keys) GetDeckSimilarities()
		{
			double[,] decks = new double[numDecks, numDecks];
			for (int i = 0; i < numDecks; i++)
			{
				for (int j = 0; j < numDecks; j++)
				{
					decks[i, j] = Dot(decksByCards[i], decksByCards[j]);
				}
			}
			return (decks, deckIndexes);
		}

		int Dot(ushort[] d1, ushort[] d2)
		{
			int sum = 0;
			for (int i = 0; i < d1.Length; i++)
			{
				sum += d1[i] * d2[i];
			}
			return sum;
		}

		public DistanceMatrix GetDistanceMatrix()
		{
			int[,] decks = new int[numDecks, numDecks];
			for (int i = 0; i < numDecks; i++)
			{
				for (int j = 0; j < numDecks; j++)
				{
					decks[i, j] = DistanceSquared(deckIndexes[i], deckIndexes[j]);
				}
			}
			return new DistanceMatrix
			{
				indexes = deckIndexes,
				matrix = decks
			};
		}

		int DistanceSquared(Deck d1, Deck d2)
		{
			ushort[] arr1 = decksByCards[deckIndexes.FindIndex(x => x == d1)];
			ushort[] arr2 = decksByCards[deckIndexes.FindIndex(x => x == d2)];

			int euclidean = 0;
			for (int i = 0; i < arr1.Length; i++)
			{
				int difference = arr1[i] - arr2[i];
				euclidean += difference * difference;
			}

			return euclidean;
		}

		public (int[,] matrix, List<string> keys) GetCardCooccurrences()
		{
			int[,] cards = new int[numCards, numCards];
			for (int i = 0; i < numDecks; i++)
			{
				ushort[] arr1 = decksByCards.Select(x => x[i]).ToArray();
				for (int j = 0; j < numDecks; j++)
				{
					ushort[] arr2 = decksByCards.Select(x => x[j]).ToArray();
					cards[i, j] = Enumerable.Range(0, arr1.Length).Sum(i => arr1[i] * arr2[i]);
				}
			}
			return (cards, cardIndexes);
		}
	}
}
