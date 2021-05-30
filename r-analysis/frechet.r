centroid <- function(f, classification, c)
{
	i <- which(classification == c)
	decks <- f[i,]

	d <- as.matrix(dist(decks))
	s <- as.matrix(rowSums(d))
	return(i[which.min((s))])
}