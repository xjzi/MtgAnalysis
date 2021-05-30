source("download.r")
mdata <- download()

source("features.r")
source("cluster.r")
source("frechet.r")

mdata <- lapply(mdata, function(cardset)
{
	g <- 32
	
	features <- cardtable(cardset)
	chifeatures <- chi(features)
	classification <- cluster(chifeatures, g)
	classification <- rmnoise(classification, chifeatures)
	cardset$centroid <- sapply(unique(classification), function(c)
	{
		return(centroid(chifeatures, classification, c))
	})
	return(cardset)
})