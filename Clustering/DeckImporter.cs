using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Shared;

namespace Clustering
{
	class DeckImporter
	{
		public IEnumerable<Deck> Decks { get => _decksById; }
		public IEnumerable<Game> Games { get => _games; }
		Deck[] _decksById;
		IEnumerable<Game> _games;

		public void LoadGames()
		{
			_games = GetGames().ToArray();
		}

		IEnumerable<Game> GetGames()
		{
			using NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
			con.Open();

			const string getGames = "select * from games;";
			using NpgsqlCommand cmd = new NpgsqlCommand(getGames, con);
			NpgsqlDataReader reader = cmd.ExecuteReader();

			IEnumerable<IDataRecord> drs = reader.Cast<IDataRecord>();
			foreach (IDataRecord dr in drs)
			{
				int winnerId = (int)dr.GetValue(0);
				int loserId = (int)dr.GetValue(1);
				yield return new Game
				{
					Winner = _decksById[winnerId],
					Loser = _decksById[loserId]
				};
			}
		}

		public void LoadDecks()
		{
			_decksById = GetDecks().ToArray();
		}

		IEnumerable<Deck> GetDecks()
		{
			using NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
			con.Open();

			const string getDecks = "select mainboard, sideboard from decks;";
			using NpgsqlCommand cmd = new NpgsqlCommand(getDecks, con);
			NpgsqlDataReader reader = cmd.ExecuteReader();

			IEnumerable<IDataRecord> drs = reader.Cast<IDataRecord>();
			foreach (IDataRecord dr in drs)
			{
				yield return new Deck
				{
					Mainboard = (string[])dr.GetValue(0),
					Sideboard = (string[])dr.GetValue(1)
				};
			}
		}
	}
}
