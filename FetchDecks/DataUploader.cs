using Npgsql;
using System;
using System.Linq;
using System.Collections.Generic;
using Shared;

namespace FetchDecks
{
	class DataUploader
	{
		readonly Tournament _tournament;
		readonly IEnumerable<Deck> _decks;
		readonly IEnumerable<Game> _games;
		int _tournamentID;
		int _startingId = 0;

		public DataUploader(Tournament tournament, IEnumerable<Deck> decks, IEnumerable<Game> games)
		{
			_tournament = tournament;
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
			foreach(Deck deck in _decks)
			{
				deck.Id = i;
				using NpgsqlCommand cmd = new NpgsqlCommand(insertDeck, con);
				cmd.Parameters.AddWithValue("id", i);
				cmd.Parameters.AddWithValue("tournamentid", _tournamentID);
				cmd.Parameters.AddWithValue("mainboard", deck.Mainboard.ToArray());
				cmd.Parameters.AddWithValue("sideboard", deck.Sideboard.ToArray());
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

		public void UploadTournament()
		{
			const string insertTournament = @"INSERT INTO tournaments(format, gametype, date)
VALUES (
@cardset,
@gametype,
@date )
returning ""id"";";
			using NpgsqlConnection con = new NpgsqlConnection(ConnectionDetails.connection);
			con.Open();
			using NpgsqlCommand cmd = new NpgsqlCommand(insertTournament, con);
			cmd.Parameters.AddWithValue("cardset", _tournament.CardSet);
			cmd.Parameters.AddWithValue("gametype", _tournament.GameType);
			cmd.Parameters.AddWithValue("date", _tournament.Date);
			_tournamentID = (int)cmd.ExecuteScalar();
		}
	}
}
