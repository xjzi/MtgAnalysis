FROM r-base

COPY ./src /analysis/src

WORKDIR /analysis/src

RUN chmod +x packages.sh
RUN ./packages.sh
RUN Rscript packages.r

CMD Rscript main.r