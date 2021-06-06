library(DBI)

download <- function()
{
	source("connection.txt")
	con <- connect()

	cards <- dbGetQuery(con, 'select c.name, c.deck, t.cardset from cards c inner join decks d on c.deck = d.id inner join tournaments t on d.tournament = t.id;')
	
	cardsets <- split(cards, f = cards[[3]])
	cardtables <- lapply(cardsets, function(cardset)
	{
		x <- table(cardset$deck, cardset$name)
	})

	dbDisconnect(con)
	return(cardtables)
}