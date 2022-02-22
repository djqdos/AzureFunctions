#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/runtime:6.0 as runtime6
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Copy .NET Core 3.1 runtime from the 3.1 image
COPY --from=runtime6 /usr/share/dotnet/host /usr/share/dotnet/host
COPY --from=runtime6 /usr/share/dotnet/shared /usr/share/dotnet/shared
WORKDIR /src
COPY . "AzureDownloaders"

RUN dotnet restore AzureDownloaders/AzureDownloaders/AzureDownloaders.csproj
COPY . .
WORKDIR "/src/AzureDownloaders"
RUN dotnet build "AzureDownloaders/AzureDownloaders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureDownloaders/AzureDownloaders.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV RabbitMQ=aqmp://guest:guest@192.168.1.250:5672
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true