suppressPackageStartupMessages(library("mclust"))
suppressPackageStartupMessages(library("dbscan"))
source("features.r")
source("frechet.r")

getclusters <- function(cardtables, minPts, top)
{
	return(unlist(lapply(1:length(cardtables), function(i)
	{
		cardtable <- cardtables[[i]]
		deckids <- names(cardtable[,1])

		chifeatures <- chi(cardtable)
		classification <- hdbscan(chifeatures, minPts = minPts)$cluster

		ctable <- table(classification[classification > 0])
		indices <- as.integer(names(ctable))
		clusterfreq <- as.vector(ctable)

		cardset <- names(cardtables)[[i]]

		clusters <- lapply(1:length(ctable), function(j)
		{
			cluster <- list()
			cluster$frequency <- clusterfreq[j]
			cluster$cardset <- cardset

			members <- which(classification == indices[j])

			features <- chifeatures[members , , drop=FALSE]
			centroidmember <- members[getcentroid(features)]
			cluster$centroid <- deckids[centroidmember]

			cardfreq <- colSums(cardtable[members , , drop=FALSE])
			sortedcardfreq <- sort(cardfreq[cardfreq > 0], decreasing=TRUE)
			cluster$topcards <- head(sortedcardfreq, top)

			return(cluster)
		})

		return(clusters)

	}), recursive=FALSE))
}