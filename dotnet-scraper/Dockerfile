# Multi-stage build saves space
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

COPY ./src scraper/src

WORKDIR /scraper/src

RUN dotnet build -c Release -o /scraper/bin

# Second stage only contains the runtime
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
COPY --from=build /scraper/bin /scraper/bin

WORKDIR /scraper/bin

CMD dotnet Scraper.dll 1
