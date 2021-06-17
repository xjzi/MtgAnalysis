using System;
using Npgsql;

namespace Scraper
{
	static class SqlConnection
	{
		public static NpgsqlConnection Connect()
		{
			string password = Environment.GetEnvironmentVariable("PASSWORD");
			string connection = string.Format("Host=db;Username=scraper;Password={0};Database=postgres", password);
			NpgsqlConnection con = new NpgsqlConnection(connection);
			con.Open();
			return con;
		}
	}
}
