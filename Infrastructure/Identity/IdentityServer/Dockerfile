FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Infrastructure/Identity/IdentityServer/IdentityServer.csproj", "Infrastructure/Identity/IdentityServer/"]
COPY ["Services/UserManagement/UserManagement.Application/UserManagement.Application.csproj", "Services/UserManagement/UserManagement.Application/"]
COPY ["Services/UserManagement/UserManagement.Core/UserManagement.Core.csproj", "Services/UserManagement/UserManagement.Core/"]
COPY ["Services/UserManagement/UserManagement.Persistence/UserManagement.Persistence.csproj", "Services/UserManagement/UserManagement.Persistence/"]
RUN dotnet restore "Infrastructure/Identity/IdentityServer/IdentityServer.csproj"
COPY . .
WORKDIR "/src/Infrastructure/Identity/IdentityServer"
RUN dotnet build "./IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]
