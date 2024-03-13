FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/RemoteConfig/RemoteConfig.WebApi/RemoteConfig.WebApi.csproj", "Services/RemoteConfig/RemoteConfig.WebApi/"]
RUN dotnet restore "Services/RemoteConfig/RemoteConfig.WebApi/RemoteConfig.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/RemoteConfig/RemoteConfig.WebApi"
RUN dotnet build "RemoteConfig.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RemoteConfig.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RemoteConfig.WebApi.dll"]
