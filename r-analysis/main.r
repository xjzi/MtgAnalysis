source("download")
mdata <- download()

source("cluster")

test <- function(x)
{
	clustering <- getclusters(mdata[[x]])
	table <- table(clustering)
	important <- table[which(table > mean(table))]
	barplot(important)
}

for(i in range 1:length(x$classification))
{
	classification <- x$classification[i]

}


cplot <- function()
{
	i <- 1
	while(TRUE)
	{
		plot(1:length(clustering[[i]]), lengths(clustering[[i]]), pch=16, main=mdata[[i]]$name)
		i <- i + 1
		if(i > length(clustering)){i = 1}
		readline(prompt="")
	}
}
