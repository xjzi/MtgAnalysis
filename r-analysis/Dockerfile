FROM r-base

COPY ./src /analysis/src

WORKDIR /analysis/src

RUN ./packages.sh

CMD Rscript main.r