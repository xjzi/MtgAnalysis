library(DBI)

upload <- function(clusters)
{
	source("connection.r")
	con <- connect()
	
	# Old analysis is discarded
	dbExecute(con, "DELETE FROM clusters;")
	
	centroids <- unlist(lapply(clusters, function(x){ x$centroid }))
	sizes <- unlist(lapply(clusters, function(x){ x$frequency }))
	
	dbAppendTable(con, "clusters", data.frame("centroid"=centroids, "size"=sizes))
	dbDisconnect(con)
}
