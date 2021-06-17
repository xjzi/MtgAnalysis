library(DBI)

connect <- function()
{
	con <- dbConnect(RPostgres::Postgres(),
	dbname = "tournaments",
	host = "54.88.146.98",
	port = 5432,
	user = "analysis",
	password = Sys.getenv(x="PASSWORD"))
	return(con)
}