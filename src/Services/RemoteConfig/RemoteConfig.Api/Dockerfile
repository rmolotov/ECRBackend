FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/RemoteConfig/RemoteConfig.Api/RemoteConfig.Api.csproj", "Services/RemoteConfig/RemoteConfig.Api/"]
COPY ["src/Services/RemoteConfig/RemoteConfig.Application/RemoteConfig.Application.csproj", "Services/RemoteConfig/RemoteConfig.Application/"]
COPY ["src/Services/RemoteConfig/RemoteConfig.Core/RemoteConfig.Core.csproj", "Services/RemoteConfig/RemoteConfig.Core/"]
COPY ["src/Services/RemoteConfig/RemoteConfig.Persistence/RemoteConfig.Persistence.csproj", "Services/RemoteConfig/RemoteConfig.Persistence/"]
COPY ["src/Shared/Shared.Identity/Shared.Identity.csproj", "Shared/Shared.Identity/"]
RUN dotnet restore "Services/RemoteConfig/RemoteConfig.Api/RemoteConfig.Api.csproj" \
    /clp:ErrorsOnly
COPY src/ .
WORKDIR "/src/Services/RemoteConfig/RemoteConfig.Api"
RUN dotnet build "./RemoteConfig.Api.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/build \
    /clp:ErrorsOnly

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RemoteConfig.Api.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/publish  \
    /p:UseAppHost=false \
    /clp:ErrorsOnly

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RemoteConfig.Api.dll"]
