#Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

COPY ./src ./src/

RUN dotnet publish \
    --configuration Release \
    -o /app/publish \
    src/CriThink.Server/CriThink.Server.Web/CriThink.Server.Web.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "CriThink.Server.Web.dll"]