FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/RemoteConfig/RemoteConfig.Api/RemoteConfig.Api.csproj", "Services/RemoteConfig/RemoteConfig.Api/"]
COPY ["Services/RemoteConfig/RemoteConfig.Application/RemoteConfig.Application.csproj", "Services/RemoteConfig/RemoteConfig.Application/"]
COPY ["Services/RemoteConfig/RemoteConfig.Core/RemoteConfig.Core.csproj", "Services/RemoteConfig/RemoteConfig.Core/"]
COPY ["Services/RemoteConfig/RemoteConfig.Persistence/RemoteConfig.Persistence.csproj", "Services/RemoteConfig/RemoteConfig.Persistence/"]
RUN dotnet restore "Services/RemoteConfig/RemoteConfig.Api/RemoteConfig.Api.csproj"
COPY . .
WORKDIR "/src/Services/RemoteConfig/RemoteConfig.Api"
RUN dotnet build "./RemoteConfig.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RemoteConfig.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RemoteConfig.Api.dll"]
