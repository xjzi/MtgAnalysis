using Shared;
using System;
using System.Collections.Generic;

namespace DeckAnalyzer
{
	class TournamentFinder
	{
		readonly string _linkBase;
		readonly string _cardSet;
		readonly string _gameType;

		public TournamentFinder(string cardset, string gametype)
		{
			const string linkBase = "https://magic.wizards.com/en/articles/archive/mtgo-standings/";
			_linkBase = string.Concat(linkBase, cardset, ' ', gametype).Replace(' ', '-').ToLower();
			_cardSet = cardset;
			_gameType = gametype;
		}

		public IEnumerable<Tournament> Update()
		{
			//See note about schedules below.

			DateTime now = DateTime.Now;
			string link = GetLink(now);

			if (LinkChecker.IsArticle(link))
			{
				yield return new Tournament
				{
					Url = link,
					Date = now,
					CardSet = _cardSet,
					GameType = _gameType
				};
			}
		}

		public IEnumerable<Tournament> FindByQuantity(int quantity)
		{
			//All tournament results are uploaded on regular schedules so this could be sped up.
			//Different gametypes have different schedules though, so this would not be very flexible.
			//Also, dependencies like this are confusing when the system changes.
			//This method is only used to initially populate a database, so optimization is not very important.

			int quantityFound = 0;
			DateTime date = DateTime.Now;
			while (quantityFound < quantity)
			{
				date = date.AddDays(-1);
				string link = GetLink(date);
				if (LinkChecker.IsArticle(link))
				{
					quantityFound++;
					yield return new Tournament
					{
						Url = link,
						Date = date,
						CardSet = _cardSet,
						GameType = _gameType
					};
				}
			}
		}

		string GetLink(DateTime date)
		{
			string formattedDate = date.ToString("-yyyy-MM-dd");
			return string.Concat(_linkBase, formattedDate);
		}
	}
}
