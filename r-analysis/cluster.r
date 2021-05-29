library("meanShiftR")
library("analogue")
library(stats)

getclusters <- function(cardset)
{
	features <- allfeatures(cardset)
	return(cluster(topfeatures(features), 4))
}

cluster <- function(data, bandwidth)
{
	return(meanShift(data, bandwidth = rep(0.03, ncol(data)))$assignment)
}

allfeatures <- function(cardset)
{
	mboard <- cardset$mboard
	sboard <- cardset$sboard
	
	ucards <- unique(unlist(c(mboard, sboard), recursive = FALSE))
	cardtable <- matrix(nrow=length(mboard), ncol=length(ucards))
	
	for(j in 1:length(mboard))
	{
		deck <- table(c(ucards, mboard[[j]], sboard[[j]])) - 1
		cardtable[j,] <- deck
	}
	
	return(cardtable)
}

topfeatures <- function(allfeatures, pvar=0.9)
{
	d <- distance(allfeatures, method = "chi.square")
	G <- dist2gram(d)
	SG <- svd(G)
	V <- SG$d ^ 2
	P <- V / sum(V)
	R <- min(which(cumsum(P) > pvar))
	return(SG$u[,1:R])
}

dist2gram <- function(d)
{
	d2 <- d^2
	C <- diag(nrow(d2)) - 1/nrow(d2)
	return(-0.5 * C %*% d2 %*% C)
}