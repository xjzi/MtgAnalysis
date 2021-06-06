getcentroid <- function(features)
{
	d <- as.matrix(dist(features))
	s <- as.matrix(rowSums(d))
	return(which.min((s)))
}