source("download.r")
mdata <- download()

source("features.r")
source("cluster.r")

mdata <- lapply(mdata, function(cardset)
{
	features <- cardtable(cardset)
	chifeatures <- chi(features)
	classification <- cluster(features)
	cardset$classification <- rmnoise(classification, features)
	return(cardset)
})