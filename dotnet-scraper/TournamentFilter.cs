using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static NpgsqlTypes.NpgsqlDbType;

namespace Scraper
{
	class TournamentFilter
	{
		readonly NpgsqlConnection _con;
		readonly Tournament[] _tournaments;

		public TournamentFilter(NpgsqlConnection con, IEnumerable<Tournament> tournaments)
		{
			_con = con;
			_tournaments = tournaments.ToArray();
		}

		public IEnumerable<Tournament> GetValidTournaments()
		{
			DeleteTemporaryTable();
			InsertTemporaryTable();
			IEnumerable<Tournament> unscrapedTournaments = GetUnscrapedTournaments();
			return unscrapedTournaments;
		}

		IEnumerable<Tournament> GetUnscrapedTournaments()
		{
			const string sql = @"SELECT tmp.index 
FROM tmp_tournaments tmp 
WHERE NOT EXISTS(SELECT NULL FROM tournaments t where tmp.cardset = t.cardset AND
tmp.gametype = t.gametype AND
tmp.date = t.date);";
			using NpgsqlCommand cmd = new NpgsqlCommand(sql, _con);
			using NpgsqlDataReader reader = cmd.ExecuteReader();
			IEnumerable<IDataRecord> drs = reader.Cast<IDataRecord>();
			IEnumerable<Tournament> unscrapedTourments = drs.Select(dr => _tournaments[(int)dr.GetValue(0)]).ToArray();
			return unscrapedTourments;
		}

		void InsertTemporaryTable()
		{
			using (NpgsqlBinaryImporter writer = _con.BeginBinaryImport("COPY tmp_tournaments FROM STDIN (FORMAT BINARY)"))
			{
				for (int i = 0; i < _tournaments.Length; i++)
				{
					writer.StartRow();
					writer.Write(i, Integer);
					writer.Write(_tournaments[i].CardSet, Varchar);
					writer.Write(_tournaments[i].GameType, Varchar);
					writer.Write(_tournaments[i].Date, Date);
				}
				writer.Complete();
			}
		}

		void DeleteTemporaryTable()
		{
			using NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM tmp_tournaments;", _con);
			cmd.ExecuteNonQuery();
		}
	}
}
