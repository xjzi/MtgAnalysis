library("mclust")

cluster <- function(f)
{
	return(Mclust(f, G=32, verbose=FALSE)$classification)
}

rmnoise <- function(c, f)
{
	d <- sapply(1:length(unique(c)), function(i)
	{
		decks <- f[c == i,]
		return(mean(dist(decks)))
	})
	rm <- which(d > (median(d) * 2))
	c[which(c %in% rm)] <- 0
	return(c)
}