FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
COPY ./src /src
WORKDIR /src
RUN dotnet build -c Release -o ../bin

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS runtime
COPY --from=build /bin /bin

# Add cron job file in the cron directory
COPY cron-job /etc/cron.d/cron-job

# Create the shell script and make it executable
COPY cron.sh /cron.sh
RUN chmod +x /cron.sh

# Give execution rights on the cron job
RUN chmod 0644 /etc/cron.d/cron-job

# Create the log file to be able to run tail
RUN touch /var/log/cron.log

# Install Cron
RUN apt-get update
RUN apt-get -y install cron

# Run the command on container startup
CMD cron && tail -f /var/log/cron.log
