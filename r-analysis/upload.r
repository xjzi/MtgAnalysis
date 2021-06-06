library(DBI)

upload <- function(clusters)
{
	source("connection.txt")
	con <- connect()
	centroids <- lapply(clusters, function(x){ x$centroid })
	sizes <- lapply(clusters, function(x){ x$frequency })
	
	dbAppendTable(con, "clusters", data.frame("centroid"=centroids, "size"=sizes))
	dbDisconnect(con)
}
