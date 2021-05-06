using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Website.Shared;

namespace Website.Server.Controllers
{
	[ApiController]
	[Route("api")]
	public class DatabaseAccessController : ControllerBase
	{
		public DatabaseAccessController() { con.Open(); }
		readonly NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);

		[HttpGet("clusterpreviews")]
		public IEnumerable<ClusterPreview> GetClusterPreviews()
		{
			ClusterPreview[] previews;

			using (NpgsqlCommand cmd = new NpgsqlCommand("select id, frequency from clusters;", con))
			{
				using NpgsqlDataReader reader = cmd.ExecuteReader();
				IEnumerable<IDataRecord> drs = reader.Cast<IDataRecord>();
				previews = drs.Select(dr =>
				{
					int id = (int)dr.GetValue(0);
					double frequency = decimal.ToDouble((decimal)dr.GetValue(1));

					return new ClusterPreview
					{
						ClusterId = id,
						Frequency = frequency,
						Highlights = new List<string>()
					};
				}).ToArray();
			}

			using (NpgsqlCommand cmd = new NpgsqlCommand("select card, cluster from highlightcards;", con))
			{
				using NpgsqlDataReader reader = cmd.ExecuteReader();
				IEnumerable<IDataRecord> drs = reader.Cast<IDataRecord>();
				foreach(IDataRecord dr in drs)
				{
					string card = (string)dr.GetValue(0);
					int cluster = (int)dr.GetValue(1);
					previews[cluster].Highlights.Add(card);
				}
			}

			return previews;
		}
	}
}
