suppressPackageStartupMessages(library("mclust"))

cluster <- function(f, g)
{
	return(Mclust(f, G=f, verbose=FALSE)$classification)
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