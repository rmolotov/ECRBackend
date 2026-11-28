FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Shared/Shared.Api/Shared.Api.csproj", "Shared/Shared.Api/"]
COPY ["src/Infrastructure/Gateway/OcelotGateway/OcelotGateway.csproj", "Infrastructure/Gateway/OcelotGateway/"]

RUN dotnet restore "Infrastructure/Gateway/OcelotGateway/OcelotGateway.csproj" \
    /clp:ErrorsOnly
COPY src/ .
WORKDIR "/src/Infrastructure/Gateway/OcelotGateway"
RUN dotnet build "OcelotGateway.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/build \
    /clp:ErrorsOnly

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OcelotGateway.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/publish  \
    /p:UseAppHost=false \
    /clp:ErrorsOnly

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotGateway.dll"]
