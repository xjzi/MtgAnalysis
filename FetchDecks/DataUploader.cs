using Npgsql;
using Shared;
using System.Collections.Generic;
using static NpgsqlTypes.NpgsqlDbType;

namespace FetchDecks
{
	class DataUploader
	{
		readonly NpgsqlConnection _con = new NpgsqlConnection(ConnectionDetails.connection);
		readonly Tournament _tournament;
		readonly IEnumerable<Deck> _decks;
		readonly IEnumerable<Game> _games;
		int _tournamentID;

		public DataUploader(Tournament tournament, IEnumerable<Deck> decks, IEnumerable<Game> games)
		{
			_tournament = tournament;
			_decks = decks;
			_games = games;
			_con.Open();
		}

		public void UploadDecks()
		{
			const string getCurrentId = "SELECT MAX(id) FROM DECKS;";

			int nextId = 0;
			using NpgsqlCommand getId = new NpgsqlCommand(getCurrentId, _con);
			object result = getId.ExecuteScalar();
			if (result != System.DBNull.Value) { nextId = (int)result + 1; }

			using (NpgsqlBinaryImporter writer = _con.BeginBinaryImport("COPY decks FROM STDIN (FORMAT BINARY)"))
			{
				foreach (Deck deck in _decks)
				{
					deck.Id = nextId;
					writer.StartRow();
					writer.Write(deck.Id, Integer);
					writer.Write(_tournamentID, Integer);
					writer.Write(deck.Mainboard, Array | Varchar);
					writer.Write(deck.Sideboard, Array | Varchar);
					nextId++;
				}
				writer.Complete();
			}
		}

		public void UploadGames()
		{
			foreach (Game game in _games)
			{
				using (NpgsqlBinaryImporter writer = _con.BeginBinaryImport("COPY games FROM STDIN (FORMAT BINARY)"))
				{
					foreach (Deck deck in _decks)
					{
						writer.StartRow();
						writer.Write(game.Winner.Id, Integer);
						writer.Write(game.Loser.Id, Integer);
					}
					writer.Complete();
				}
			}
		}

		public void UploadTournament()
		{
			const string insertTournament = @"INSERT INTO tournaments(format, gametype, date)
VALUES (
@cardset,
@gametype,
@date )
returning ""id"";";

			using NpgsqlCommand cmd = new NpgsqlCommand(insertTournament, _con);
			cmd.Parameters.AddWithValue("cardset", _tournament.CardSet);
			cmd.Parameters.AddWithValue("gametype", _tournament.GameType);
			cmd.Parameters.AddWithValue("date", _tournament.Date);
			_tournamentID = (int)cmd.ExecuteScalar();
		}
	}
}
