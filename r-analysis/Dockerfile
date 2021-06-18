FROM r-base

RUN R -e "install.packages(c('DBI', 'analogue', 'stats', 'mclust', 'RPostgres'))"

COPY ./src /analysis/src

CMD Rscript /analysis/src/main.r