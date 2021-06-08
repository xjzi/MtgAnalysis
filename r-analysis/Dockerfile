FROM rocker/r-ver:4.0.5

RUN R -e "install.packages(c('DBI', 'analogue', 'stats', 'mclust', 'RPostgres'))"

COPY . .

CMD R --slave -e "source('main.r')" 