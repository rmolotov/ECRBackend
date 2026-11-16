# Notes
Web-API server for ElectricalContactResistance mobile game

# Run / Deploy

- **Rider:** multi-launch config (need PostgreSQL at `5432`, MongoDB at `27017`, Redis at `6379`)
- **Docker:** see [Local debug](#local-debug)
- **Proxy in Docker:** `` docker compose -f .\.ci\traefik-debug.yml -p ecr up -d``
- **Runner:** Containerize runner: 
  ``` bash
  docker run \
      --restart=always \
      --name="ecrbackend-runner" \
      -e RUNNER_TOKEN=$(curl -sX POST -H "Authorization: token <PAT_RUNNER>" https://api.github.com/repos/rmolotov/ECRBackend/actions/runners/registration-token | jq -r .token) \
      -d ghcr.io/actions/actions-runner:2.319.1 \
      sh -c './config.sh --url https://github.com/rmolotov/ECRBackend --token ${RUNNER_TOKEN} --unattended && ./run.sh'
  ```
  

# Tech Stack

## Core
* .NET 10.0
* Databases:
  * PostgreSQL 18
  * MongoDB
* Caching:
  * In-memory
  * Redis
* Docker + docker-compose (with `wget` healthchecks)
  * images: `mcr.microsoft.com/dotnet/sdk:10.0-alpine` / `aspnet:10.0-alpine`
* Swagger
* OAuth 2.0, JWT

## Frameworks
| Framework            | Version |
|----------------------|:--------|
| IdentityServer4      | 4.1.2   |
| EntityFramework Core | 10.0.15 |
| Npgsql EF Core       | 10.0.3  |
| MongoDB EF Core      | 10.0.5  |
| Ocelot               | 24.0.1  |
| Serilog.AspNetCore   | 10.0.0  |
| Automapper           | 15.0.1  |
| Swashbuckle          | 9.0.4   |
| Asp.Versioning       | 10.2.1  |
| MediatR              | 13.0.0  |
| FluentValidation     | 12.0.0  |

## Tools
* Rider
* Docker Desktop
* GitKraken
* Postman

# Architecture
* **Deploy**: micro-services
* **Services**: clean architecture

## Services:
* **Infrastructure.Identity** (IdentityServer4)
  * WebAPI - at `http, 5001`, health: `/.well-known/openid-configuration`
* **Infrastructure.Gateway** (Ocelot)
  * WebAPI - at `http, 5050`
  * `/UserManagement/{everything}` → UserManagement `/api/{everything}`
  * `/RemoteConfig/{everything}` → RemoteConfig `/api/{everything}`
* **UserManagement**
  * Core
  * Application
  * Persistence
  * WebAPI - at `http, 5002`, health: `/HealthCheck`
* **RemoteConfig**
  * Core
  * Application
  * Persistence
  * WebAPI - at `http, 5003`, health: `/HealthCheck`, Swagger UI: `/`
  * API version goes in the URL segment as a number: `/api/1.0/Hero` (not `v1`)

# Commands:
### Local debug

Networks are external, create them once:
- Common network for persistence and proxy, shared between projects:
  ``` bash 
      docker network create common-infrastructurecle
  ```
- Network for project services:
  ``` bash 
      docker network create ecr-services
  ```

### Build and run:

- Traefik
  ```bash
        docker-compose \
        -p common-infrastructure \
        -f docker-compose.traefik.yml \
        up -d --build
  ```
- Infrastructure
  ```bash
        docker-compose \
        -p ecr-backend \
        -f docker-compose.infrastructure.yml \
        --env-file .ci/databases.env \
        up -d --build
  ```
- Databases
  ``` bash
      docker-compose \
        -p common-infrastructure \
        -f docker-compose.databases.yml \
        --env-file .ci/databases.env \
        up -d --build
  ```
- Services
  ``` bash      
      docker-compose \
        -p ecr-backend \
        -f docker-compose.services.yml \
        --env-file .ci/services.env \
        up -d --build
  ```

Check status (all containers should be `healthy`):
``` bash
    docker-compose -p ecr -f docker-compose.databases.yml -f docker-compose.services.yml ps
```
Smoke test via gateway:
``` bash
    curl http://localhost:5050/RemoteConfig/1.0/Hero       # 200
    curl -i http://localhost:5050/UserManagement/Users     # 401 without token
```
### Add migration
```bash
    cd ./src/Services/RemoteConfig/RemoteConfig.Persistence/
```
```bash
    dotnet ef migrations add MigrationName -s ../RemoteConfig.Api/
```