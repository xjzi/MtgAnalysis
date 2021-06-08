FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
COPY . ./src
WORKDIR /src/
RUN dotnet build -c Release -o ../bin

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS runtime
COPY --from=build /bin .

RUN apt-get update && apt-get -y install cron
COPY scraper-cron /etc/cron.d/scraper-cron
RUN chmod 0644 /etc/cron.d/scraper-cron
RUN crontab /etc/cron.d/scraper-cron

CMD cron && tail -f /var/log/cron.log