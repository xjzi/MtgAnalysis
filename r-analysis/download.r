library(DBI)

download <- function()
{
	source("connection.txt")
	con <- connect()
	
	names <- dbGetQuery(con, 'select distinct cardset from tournaments')[[1]]

	front <- 'select d.id, d.tournament, array_to_string(d.mainboard, \'\'), array_to_string(d.sideboard, \'\') from decks d inner join tournaments t on d.tournament = t.id where t.cardset = \''

	back <- '\';'

	mdata <- lapply(1:length(names), function(i)
	{
		query <- paste(front, names[i], back, sep = "")
		result <- dbGetQuery(con, query)
		
		cardset <- list()
		cardset$id <- result[[1]]
		cardset$tournament <- result[[2]]
		cardset$mboard <- lapply(result[[3]], function(x) as.character(unlist(strsplit(x, "", fixed=T))))
		cardset$sboard <- lapply(result[[4]], function(x) as.character(unlist(strsplit(x, "", fixed=T))))
		cardset$name <- names[i]
		return(cardset)
	})

	dbDisconnect(con)
	return(mdata)
}