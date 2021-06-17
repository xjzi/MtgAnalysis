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
		classification <- rmnoise(getcluster(chifeatures, g), chifeatures)
		
		ctable <- table(classification[classification > 0])
		indices <- as.integer(names(ctable))
		frequencies <- as.vector(ctable)
		
		cardset <- names(cardtables)[[i]]
		
		clusters <- lapply(1:length(ctable), function(j)
		{
			cluster <- list()
			members <- which(classification == indices[j])
			
			if(length(members) == 1)
			{ 
				centroid <- members[1]
			}
			else
			{
				features <- chifeatures[members,]
				centroid <- members[getcentroid(features)]
			}

			cluster$centroid <- centroid
			cluster$frequency <- frequencies[j]
			cluster$cardset <- cardset

			return(cluster)
		})

		return(clusters)
		
	}), recursive=FALSE))
}