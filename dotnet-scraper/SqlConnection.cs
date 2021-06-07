using System;
using Npgsql;
using System.IO;

namespace Scraper
{
	static class SqlConnection
	{
		public static NpgsqlConnection Connect()
		{
			string connection = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "connection.txt"));
			NpgsqlConnection con = new NpgsqlConnection(connection);
			con.Open();
			return con;
		}
	}
}
