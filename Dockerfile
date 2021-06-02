FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY . ./
WORKDIR /app/MTA.API

ENTRYPOINT ["dotnet", "watch", "run"]
