using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Shared;
using static NpgsqlTypes.NpgsqlDbType;

namespace Clustering
{
	class Exporter
	{
		readonly NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
		public Exporter()
		{
			con.Open();
		}

		public void CleanExistingTables()
		{
			//Old analyses are useless
			DeleteFrom("clusters");
			DeleteFrom("cards");
			DeleteFrom("highlightcards");
			DeleteFrom("similarclusters");
		}

		public void ExportClusters(IEnumerable<Cluster> source)
		{
			int clusterIdOffset = 0;
			using (NpgsqlCommand getId = new NpgsqlCommand("SELECT MAX(id) from clusters;", con))
			{
				object result = getId.ExecuteScalar();
				if (result != DBNull.Value) { clusterIdOffset = (int)result + 1; }
			}
			Cluster[] clusters = source.ToArray();

			//Writer is very speedy compared to a command for each item
			using (NpgsqlBinaryImporter writer = con.BeginBinaryImport("COPY clusters FROM STDIN (FORMAT BINARY)"))
			{
				for (int i = 0; i < clusters.Length; i++)
				{
					clusters[i].Id = i + clusterIdOffset;
					writer.StartRow();
					writer.Write(clusters[i].Id, Integer);
					writer.Write(clusters[i].Middle.Mainboard, NpgsqlDbType.Array | Varchar);
					writer.Write(clusters[i].Middle.Sideboard, NpgsqlDbType.Array | Varchar);
					//Best way to write a double to a decimal
					writer.Write((decimal)clusters[i].Frequency);
				}
				writer.Complete();
			}
		}

		public void ExportCards(IEnumerable<Card> source)
		{
			using (NpgsqlBinaryImporter writer = con.BeginBinaryImport("COPY cards FROM STDIN (FORMAT BINARY)"))
			{
				foreach (Card card in source)
				{
					writer.StartRow();
					writer.Write(card.Name, Varchar);
					writer.Write((decimal)card.Frequency);
				}
				writer.Complete();
			}
		}

		public void ExportHighlights(IEnumerable<HighlightCard> source)
		{
			using (NpgsqlBinaryImporter writer = con.BeginBinaryImport("COPY highlightcards FROM STDIN (FORMAT BINARY)"))
			{
				foreach (HighlightCard highlight in source)
				{
					writer.StartRow();
					writer.Write(highlight.Cluster.Id, Integer);
					writer.Write(highlight.Card.Name, Varchar);
					writer.Write((decimal)highlight.Frequency);
				}
				writer.Complete();
			}
		}

		public void ExportSimilarClusters(IEnumerable<SimilarClusters> source)
		{
			using (NpgsqlBinaryImporter writer = con.BeginBinaryImport("COPY similarclusters FROM STDIN (FORMAT BINARY)"))
			{
				foreach (SimilarClusters similar in source)
				{
					writer.StartRow();
					writer.Write(similar.A.Id, Integer);
					writer.Write(similar.B.Id, Integer);
					writer.Write((int)similar.Distance, Integer);
				}
				writer.Complete();
			}
		}

		void DeleteFrom(string table)
		{
			using (NpgsqlCommand delete = new NpgsqlCommand(string.Format("DELETE FROM {0};", table), con))
			{
				delete.ExecuteNonQuery();
			}
		}
	}
}
