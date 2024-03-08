FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/UserManagement/UserManagement.Core/UserManagement.Core.csproj", "Services/UserManagement/UserManagement.Core/"]
COPY ["Services/UserManagement/UserManagement.Persistence/UserManagement.Persistence.csproj", "Services/UserManagement/UserManagement.Persistence/"]
COPY ["Infrastructure/Identity/Identity/Notes.Identity.csproj", "Infrastructure/Identity/"]
RUN dotnet restore "Infrastructure/Identity/Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/Identity"
RUN dotnet build "Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll"]
