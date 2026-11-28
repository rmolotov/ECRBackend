FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Shared/Shared.Api/Shared.Api.csproj", "Shared/Shared.APi/"]
COPY ["src/Services/UserManagement/UserManagement.Core/UserManagement.Core.csproj", "Services/UserManagement/UserManagement.Core/"]
COPY ["src/Services/UserManagement/UserManagement.Application/UserManagement.Application.csproj", "Services/UserManagement/UserManagement.Application/"]
COPY ["src/Services/UserManagement/UserManagement.Persistence/UserManagement.Persistence.csproj", "Services/UserManagement/UserManagement.Persistence/"]
COPY ["src/Services/UserManagement/UserManagement.Api/UserManagement.Api.csproj", "Services/UserManagement/UserManagement.Api/"]

RUN dotnet restore "Services/UserManagement/UserManagement.Api/UserManagement.Api.csproj" \
    /clp:ErrorsOnly
COPY src/ .
WORKDIR "/src/Services/UserManagement/UserManagement.Api"
RUN dotnet build "UserManagement.Api.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/build \
    /clp:ErrorsOnly

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserManagement.Api.csproj"  \
    -c $BUILD_CONFIGURATION  \
    -o /app/publish  \
    /p:UseAppHost=false \
    /clp:ErrorsOnly

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]
