FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["spotify-stats-api/spotify-stats-api.csproj", "spotify-stats-api/"]
RUN dotnet restore "spotify-stats-api/spotify-stats-api.csproj"
COPY . .
WORKDIR "/src/spotify-stats-api"
RUN dotnet build "spotify-stats-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "spotify-stats-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "spotify-stats-api.dll"]
