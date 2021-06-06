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

getclusters <- function(cardtables)
{
	return(unlist(lapply(1:length(cardtables), function(i)
	{
		g <- 32
		cardtable <- cardtables[[i]]

		chifeatures <- chi(cardtable)
		classification <- getcluster(chifeatures, g)
		classification <- rmnoise(classification, chifeatures)
		ctable <- table(classification[classification > 0])
		indices <- as.integer(names(ctable))
		frequencies <- as.vector(ctable)
		
		clusters <- lapply(1:length(ctable), function(j)
		{
			cluster <- list()
			centroid <- centroid(chifeatures, classification, indices[j])
			cluster$centroid <- rownames(cardtable)[centroid]
			cluster$frequency <- frequencies[j]
			cluster$cardset <- names(cardtables)[[i]]
			return(cluster)
		})
		
		return(clusters)
		
	}), recursive=FALSE))
}