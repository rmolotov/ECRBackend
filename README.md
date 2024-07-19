# Notes
Web-API server for ElectricalContactResistance mobile game

# Run / Deploy

- **Rider:** multi-launch config (need MS SQL Server at `1433`)
- **Docker:** ``docker compose up -d --build``
- **Runner:** 
  - Containerize runner: ``docker run --restart=always -d ghcr.io/actions/actions-runner:2.317.0 sh -c './config.sh remove --token <TOKEN> && ./config.sh --url https://github.com/rmolotov/ECRBackend --token <TOKEN> --unattended && ./run.sh'``

# Tech Stack

## Core
* .NET Core 8.0
* Databases:
  * MS SQL Server 2022,
  * MongoDB 7.0.7
* Caching:
  * In-memory
  * Redis
* Docker + docker-compose.override
* Swagger
* OAuth 2.0, JWT

## Frameworks
| Framework            | Version |
|----------------------|:--------|
| IdentityServer       | 4       |
| EntityFramework Core | 8.0.0   |
| Serilog              | 8.0.1   |
| Automapper           | 13.0.0  |
| Swashbuckle          | 6.5.0   |
| MediatR              | 12.2.0  |
| FluentValidation     | 11.9.0  |
| xUnit                | 2.4.2   |
| Shouldly             | 4.2.1   |

## Tools
* Rider
* Docker Desktop
* GitKraken
* Postman

# Architecture
* **Deploy**: micro-services
* **Services**: clean architecture

## Services:
* **Infrastructure.Identity**
  * WebAPI - at `http, 5001`
* **Usermanagement**
  * Core
  * Application
  * Persistence
  * WebAPI - at `http, 5002`
* **StaticData**
  * Core
  * Application
  * Persistence
  * WebAPI - at `http, 5003`