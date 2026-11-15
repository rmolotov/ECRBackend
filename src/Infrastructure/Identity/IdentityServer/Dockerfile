FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# AddDeveloperSigningCredential writes tempkey.jwk to the working directory
RUN chown $APP_UID /app
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Infrastructure/Identity/IdentityServer/IdentityServer.csproj", "Infrastructure/Identity/IdentityServer/"]
COPY ["src/Services/UserManagement/UserManagement.Application/UserManagement.Application.csproj", "Services/UserManagement/UserManagement.Application/"]
COPY ["src/Services/UserManagement/UserManagement.Core/UserManagement.Core.csproj", "Services/UserManagement/UserManagement.Core/"]
COPY ["src/Services/UserManagement/UserManagement.Persistence/UserManagement.Persistence.csproj", "Services/UserManagement/UserManagement.Persistence/"]
COPY ["src/Shared/Shared.Identity/Shared.Identity.csproj", "Shared/Shared.Identity/"]
RUN dotnet restore "Infrastructure/Identity/IdentityServer/IdentityServer.csproj"
COPY src/ .
WORKDIR "/src/Infrastructure/Identity/IdentityServer"
RUN dotnet build "./IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]
