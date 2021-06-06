suppressPackageStartupMessages(library("analogue"))
library("stats")

chi <- function(features, pvar=0.9)
{
	d <- distance(features, method = "chi.square")
	d2 <- d^2
	C <- diag(nrow(d2)) - 1/nrow(d2)
	G <- -0.5 * C %*% d2 %*% C
	SG <- svd(G)
	V <- SG$d ^ 2
	P <- V / sum(V)
	R <- min(which(cumsum(P) > pvar))
	return(SG$u[,1:R])
}