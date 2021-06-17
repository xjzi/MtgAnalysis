source("download.r")
cardtables <- download()

source("clusters.r")
clusters <- getclusters(cardtables)

source("upload.r")
upload(clusters)