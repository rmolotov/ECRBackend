FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Infrastructure/Gateway/Gateway.Ocelot/Gateway.Ocelot.csproj", "Infrastructure/Gateway/Gateway.Ocelot/"]
RUN dotnet restore "Infrastructure/Gateway/Gateway.Ocelot/Gateway.Ocelot.csproj"
COPY . .
WORKDIR "/src/Infrastructure/Gateway/Gateway.Ocelot"
RUN dotnet build "Gateway.Ocelot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gateway.Ocelot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Ocelot.dll"]
