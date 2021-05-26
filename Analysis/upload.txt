library(DBI)
dbAppendTable(con, "tmp", data.frame("field"=c(1, 5, 7, 3)), row.names = NULL)