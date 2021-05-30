source("download.r")
mdata <- download()

source("features.r")
source("cluster.r")

for(i in 1:length(mdata))
{
	features <- chi(cardtable(mdata[[2]]))
	classification <- rmnoise(cluster(features), features)
}