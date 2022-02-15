FROM mcr.microsoft.com/dotnet/sdk:5.0 AS installer-env



# Build requires 3.1 SDK
COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet

COPY . /src/dotnet-function-app

RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \	
    dotnet publish AzureDownloaders/*.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:3.0-dotnet-isolated5.0-appservice
#FROM mcr.microsoft.com/azure-functions/dotnet-isolated:3.0-dotnet-isolated5.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot
ENV AzureFunctionsJobHost__Logging__Console__IsEnabled=true
ENV RabbitMQ=aqmp://guest:guest@192.168.1.250:5672

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]
