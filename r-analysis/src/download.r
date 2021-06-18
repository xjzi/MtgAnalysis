library(DBI)

download <- function()
{
	source("connection.r")
	con <- connect()

	cards <- dbGetQuery(con, 'SELECT c.name, c.deck, t.cardset FROM cards c INNER JOIN decks d ON c.deck = d.id INNER JOIN tournaments t ON d.tournament = t.id;')
	
	cardsets <- split(cards, f = cards[[3]])
	cardtables <- lapply(cardsets, function(cardset)
	{
		x <- table(cardset$deck, cardset$name)
	})

	dbDisconnect(con)
	return(cardtables)
}