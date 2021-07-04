getcentroid <- function(features)
{
	if(length(features) == 1)
	{
		return(1)
	}
	
	d <- as.matrix(dist(features))
	s <- as.matrix(rowSums(d))
	return(which.min((s)))
}