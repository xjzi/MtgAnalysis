library(DBI)

download <- function()
{
	source("connection.txt")
	con <- connect()
	
	names <- dbGetQuery(con, 'select distinct cardset from tournaments')[[1]]

	front <- 'select a.id, a.tournament, array_to_string(a.mainboard, \'\'), array_to_string(a.sideboard, \'\') from decks a inner join tournaments b on a.tournament = b.id where b.cardset = \''

	back <- '\';'

	mdata <- vector("list", length=length(names))
	for(i in 1:length(names))
	{
		query <- paste(front, names[i], back, sep = "")
		result <- dbGetQuery(con, query)
		
		item <- list()
		item$id <- result[[1]]
		item$tournament <- result[[2]]
		item$mboard <- lapply(result[[3]], function(x) as.character(unlist(strsplit(x, "", fixed=T))))
		item$sboard <- lapply(result[[4]], function(x) as.character(unlist(strsplit(x, "", fixed=T))))
		item$name <- names[i]
		
		mdata[[i]] <- item
	}

	dbDisconnect(con)
	return(mdata)
}