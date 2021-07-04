source("download.r")
cardtables <- download()

source("clusters.r")
clusters <- getclusters(cardtables, g=32, top=10)

source("upload.r")
upload(clusters)