ARG environment

FROM mcr.microsoft.com/dotnet/sdk:6.0

COPY ./src ./src/

RUN dotnet publish \
    --configuration Release \
    --self-contained false \
    --runtime linux-x64 \
    --output /app/publish \
    src/CriThink.Server/CriThink.Server.Web/CriThink.Server.Web.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=0 /app/publish .
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=$environment
ENTRYPOINT ["dotnet", "CriThink.Server.Web.dll"]