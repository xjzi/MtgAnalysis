using Npgsql;
using System;
using System.Linq;
using System.Collections.Generic;
using Website.Shared;

namespace FetchDecks
{
	class DataUploader
	{
		readonly Deck[] _decks;
		readonly IEnumerable<Game> _games;
		int _tournamentID;
		int _startingId = 0;

		public DataUploader(Deck[] decks, IEnumerable<Game> games)
		{
			_decks = decks;
			_games = games;
		}

		public void UploadDecks()
		{
			const string getCurrentId = "SELECT MAX(id) FROM DECKS;";
			const string insertDeck = @"INSERT INTO decks
VALUES (
@id,
@tournamentid,
@mainboard,
@sideboard);";

			using NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
			con.Open();

			using NpgsqlCommand getId = new NpgsqlCommand(getCurrentId, con);
			object result = getId.ExecuteScalar();
			if (result != DBNull.Value) { _startingId = (int)result + 1; }

			int i = _startingId;
			for(int j = 0; j < _decks.Length; j++)
			{
				_decks[j].Id = i;
				using NpgsqlCommand cmd = new NpgsqlCommand(insertDeck, con);
				cmd.Parameters.AddWithValue("id", i);
				cmd.Parameters.AddWithValue("tournamentid", _tournamentID);
				cmd.Parameters.AddWithValue("mainboard", _decks[j].Mainboard.ToArray());
				cmd.Parameters.AddWithValue("sideboard", _decks[j].Sideboard.ToArray());
				cmd.ExecuteNonQuery();
				i++;
			}
		}

		public void UploadGames()
		{
			const string insertGame = @"INSERT INTO GAMES
VALUES (
@winner,
@loser
); ";
			using NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
			con.Open();

			foreach (Game game in _games)
			{
				using NpgsqlCommand cmd = new NpgsqlCommand(insertGame, con);
				cmd.Parameters.AddWithValue("winner", game.Winner.Id);
				cmd.Parameters.AddWithValue("loser", game.Loser.Id);
				cmd.ExecuteNonQuery();
			}
		}

		public void UploadTournament(string format, string gametype, DateTime date)
		{
			const string insertTournament = @"INSERT INTO tournaments(format, gametype, date)
VALUES (
@format,
@gametype,
@date )
returning ""id"";";
			using NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
			con.Open();
			using NpgsqlCommand cmd = new NpgsqlCommand(insertTournament, con);
			cmd.Parameters.AddWithValue("format", format);
			cmd.Parameters.AddWithValue("gametype", gametype);
			cmd.Parameters.AddWithValue("date", date);
			_tournamentID = (int)cmd.ExecuteScalar();
		}
	}
}
