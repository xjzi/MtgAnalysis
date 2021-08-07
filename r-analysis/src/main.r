source("download.r")
cardtables <- download()

source("clusters.r")
clusters <- getclusters(cardtables, minPts=10, top=10)

source("upload.r")
upload(clusters)

message(length(clusters), " clusters were found and uploaded.")