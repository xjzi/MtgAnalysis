library(DBI)

upload <- function(clusters)
{
	source("connection.r")
	con <- connect()
	
	centroids <- sapply(clusters, function(clusters){ clusters$centroid })
	sizes <- sapply(clusters, function(clusters){ clusters$frequency })
	ids <- 1:length(clusters)

	topcards <- unlist(lapply(ids, function(id)
	{
		clustertopcards <- clusters[[id]]$topcards
		lapply(1:length(clustertopcards), function(i)
		{
			list("name"=names(clustertopcards[i]), "frequency"=clustertopcards[[i]], "cluster"=id)
		})
	}), recursive=FALSE)

	topcardnames <- sapply(topcards, function(topcard){ topcard$name })
	topcardfrequencies <- sapply(topcards, function(topcard){ topcard$frequency })
	topcardclusters <- sapply(topcards, function(topcard){ topcard$cluster })

	# Old analysis is discarded
	dbExecute(con, "DELETE FROM clusters;")
	dbExecute(con, "DELETE FROM top_cards;")

	dbAppendTable(con, "clusters", data.frame("centroid"=centroids, "size"=sizes, "id"=ids))
	dbAppendTable(con, "top_cards", data.frame("name"=topcardnames, "frequency"=topcardfrequencies, "cluster"=topcardclusters))
	dbDisconnect(con)
}
