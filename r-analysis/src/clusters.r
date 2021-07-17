suppressPackageStartupMessages(library("mclust"))
source("features.r")
source("frechet.r")

getcluster <- function(f, g)
{
	return(Mclust(f, G=g, verbose=FALSE)$classification)
}

rmnoise <- function(c, f)
{
	d <- sapply(unique(c), function(i)
	{
		decks <- f[c == i,]
		return(mean(dist(decks)))
	})
	rm <- which(d > (median(d) * 2))
	c[which(c %in% rm)] <- 0
	return(c)
}

getclusters <- function(cardtables, g, top)
{
	return(unlist(lapply(1:length(cardtables), function(i)
	{
		cardtable <- cardtables[[i]]
		deckids <- names(cardtable[,1])

		chifeatures <- chi(cardtable)
		classification <- rmnoise(getcluster(chifeatures, g), chifeatures)
		
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