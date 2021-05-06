using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Shared;
using Npgsql;
using System.Data;

namespace Website.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SqlProxyController : ControllerBase
	{
		[HttpGet("clusters")]
		public Cluster Get()
		{
			/*
			using NpgsqlConnection con = new NpgsqlConnection(SqlDetails.connection);
			con.Open();

			using NpgsqlCommand cmd = new NpgsqlCommand("select mainboard, sideboard, frequency from clusters;", con);
			NpgsqlDataReader reader = cmd.ExecuteReader();
			IEnumerable<IDataRecord> drs = reader.Cast<IDataRecord>();
			foreach (IDataRecord dr in drs)
			{
				string[] mainboard = (string[])dr.GetValue(0);
				string[] sideboard = (string[])dr.GetValue(1);
				double frequency = (double)dr.GetValue(2);
				yield return new Cluster
				{
					middle = new DeckWithCluster
					{
						mainboard = mainboard,
						sideboard = sideboard
					},
					frequency = frequency
				};
			}*/
			return new Cluster
			{
				Middle = new DeckWithCluster
				{
					Mainboard = new string[]{ "hello" },
					Sideboard = new string[] { "it worked" }
				},
				Frequency = 5
			};
		}
	}
}
