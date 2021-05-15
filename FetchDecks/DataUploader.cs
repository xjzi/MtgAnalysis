using Npgsql;
using Shared;
using System.Collections.Generic;
using System.Linq;
using static NpgsqlTypes.NpgsqlDbType;

namespace FetchDecks
{
	class DataUploader
	{
		readonly NpgsqlConnection _con = new NpgsqlConnection(ConnectionDetails.connection);
		readonly Tournament[] _tournaments;

		public DataUploader(IEnumerable<Tournament> tournaments)
		{
			_tournaments = tournaments.ToArray();
			_con.Open();
		}

		public void Upload()
		{
			UploadTournaments();
			UploadDecks();
			UploadGames();
		}

		void UploadDecks()
		{
			int id = GetNextId("decks");

			using (NpgsqlBinaryImporter writer = _con.BeginBinaryImport("COPY decks FROM STDIN (FORMAT BINARY)"))
			{
				foreach (Tournament tournament in _tournaments)
				{
					foreach (Deck deck in tournament.Decks)
					{
						deck.Id = id;
						writer.StartRow();
						writer.Write(deck.Id, Integer);
						writer.Write(tournament.Id, Integer);
						writer.Write(deck.Mainboard, Array | Varchar);
						writer.Write(deck.Sideboard, Array | Varchar);
						id++;
					}
				}
				writer.Complete();
			}
		}

		void UploadGames()
		{
			using (NpgsqlBinaryImporter writer = _con.BeginBinaryImport("COPY games FROM STDIN (FORMAT BINARY)"))
			{
				foreach (Game game in _tournaments.SelectMany(x => x.Games))
				{
					writer.StartRow();
					writer.Write(game.Winner.Id, Integer);
					writer.Write(game.Loser.Id, Integer);
				}
				writer.Complete();
			}
		}

		void UploadTournaments()
		{
			int id = GetNextId("tournaments");
			using (NpgsqlBinaryImporter writer = _con.BeginBinaryImport("COPY tournaments FROM STDIN (FORMAT BINARY)"))
			{
				for (int i = 0; i < _tournaments.Length; i++)
				{
					_tournaments[i].Id = id;
					writer.StartRow();
					writer.Write(_tournaments[i].Id, Integer);
					writer.Write(_tournaments[i].CardSet, Varchar);
					writer.Write(_tournaments[i].GameType, Varchar);
					writer.Write(_tournaments[i].Date, Date);
					id++;
				}
				writer.Complete();
			}
		}

		public int GetNextId(string table, string column = "id")
		{
			string sql = string.Format("SELECT COALESCE(MAX({0}), -1) FROM {1};", column, table);
			NpgsqlCommand command = new NpgsqlCommand(sql, _con);
			return (int)command.ExecuteScalar() + 1;
		}
	}
}
