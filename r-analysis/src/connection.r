library(DBI)

connect <- function()
{
	con <- dbConnect(RPostgres::Postgres(),
	dbname = "tournaments",
	host = "db",
	port = 5432,
	user = "analysis",
	password = Sys.getenv(x="PASSWORD"))
	return(con)
}