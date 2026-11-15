# ECRBackend — ревью и TODO

> Ревью от 2026-11-15 (ветка `fix/identity-settings`).
> Прочитан весь код, конфигурация, Docker и CI. Все проекты собраны по отдельности (`dotnet build`).
> Тестов в репозитории нет. Пункты с пометкой «не проверено» требуют подтверждения.
> Обновлено 2026-11-15 после запуска в Docker (colima, aarch64) — см. раздел 5. Исходный код перенесён в `src/`.

### Структура репозитория

```
.
├── docs/
│   ├── README.md              # описание проекта и команды запуска
│   └── TODO.md                # этот файл
├── src/
│   ├── Infrastructure/        # IdentityServer, OcelotGateway
│   ├── Services/              # RemoteConfig, UserManagement
│   └── Shared/                # Shared.Identity
├── .ci/                       # env-файлы, traefik, nexus
├── .github/                   # CI и self-hosted runner
├── .dockerignore                # контекст сборки образов — корень репозитория
├── docker-compose.databases.yml
├── docker-compose.services.yml  # build.context: ., dockerfile: src/...
├── docker-compose.dcproj        # в slnx — в папке Solution Items
└── ECRBackend.slnx              # проекты: src\...; Solution Items: docs\*.md, dcproj
```

---

## 1. Назначение и общая архитектура

Бэкенд мобильной игры *Electrical Contact Resistance*. Набор микросервисов на .NET 9; каждый сервис устроен по Clean Architecture: Core → Application → Persistence → Api.

```
                  ┌──────────────── Traefik (80/443, Let's Encrypt) ───────────────┐
                  │   /dev/auth → Identity           /dev/api → Ocelot Gateway     │
                  └──────────────┬──────────────────────────────┬──────────────────┘
                                 │                              │
                  ┌──────────────▼─────────┐      ┌─────────────▼──────────────┐
                  │ IdentityServer :5001   │      │ Ocelot Gateway :5050       │
                  │ IdentityServer4 + MVC  │      │ /RemoteConfig/* /UserMgmt/*│
                  └──────┬─────────────────┘      └──────┬───────────────┬─────┘
            JWT (OIDC discovery) │                       │               │
                  ┌──────────────┼───────────────────────▼───┐   ┌───────▼────────────┐
                  │              │  RemoteConfig.Api :5003   │   │ UserManagement.Api │
                  │              │  MediatR + FluentValidation│  │ :5002 (контроллеры │
                  │              │  + AutoMapper + кэш        │  │  → UserManager)    │
                  │              └──────┬──────────┬─────────┘   └───────┬────────────┘
                  │                     │          │                     │
           ┌──────▼──────────────┐ ┌────▼────┐ ┌───▼────┐                │
           │ MS SQL 2022         │ │ MongoDB │ │ Redis* │◄───────────────┘
           │ ECR.Users           │ └─────────┘ └────────┘   (ECR.Users)
           │ ECR.IdentityServer  │
           └─────────────────────┘
```

`*` Redis поднимается в compose, но код к нему не подключён (см. B4).

### Компоненты

| Компонент | Роль | Хранилище |
|---|---|---|
| **src/Infrastructure/Identity/IdentityServer** | OAuth2/OIDC-сервер на IdentityServer4 и ASP.NET Identity. UI из шаблона QuickStart (MVC, Bootstrap 4, jQuery). Клиенты, скоупы и гранты хранятся в БД через EF. | PostgreSQL: `ECR.IdentityServer`, `ECR.Users` |
| **src/Infrastructure/Gateway/OcelotGateway** | API Gateway: маршрутизация и JWT-аутентификация на маршрутах | — |
| **src/Services/RemoteConfig** | Статичные игровые данные: герои, враги, этапы (Stage), предметы инвентаря. CQRS через MediatR, pipeline-поведения Logging → Validation → Caching. Кэш — цепочка провайдеров Memory → Distributed. | MongoDB через EF Core-провайдер |
| **src/Services/UserManagement** | CRUD пользователей и ролей поверх `UserManager`/`RoleManager`. Слой Application пустой — только DTO. | PostgreSQL: `ECR.Users` |
| **src/Shared/Shared.Identity** | Общие `IdentityConfiguration` и `CurrentUserService`. В работе, пока **никем не подключён**. | — |

**Связи между проектами:** IdentityServer напрямую ссылается на все три проекта UserManagement (Core, Application, Persistence) и использует ту же БД `ECR.Users`. Identity и UserManagement связаны на уровне схемы данных, миграции применяются из двух мест.

**Аутентификация:**

- `Client.Api` — client_credentials, скоупы `ECR.Api` и `ECR.Web`.
- `Client.Web` — Resource Owner Password без секрета, скоуп `ECR.Web`.
- RemoteConfig требует `Bearer` только на запись.
- UserManagement требует скоуп `ECR.Api` на всё.

---

## 2. Внешние зависимости

### Инфраструктура (Docker)

| Сервис | Образ | Комментарий |
|---|---|---|
| PostgreSQL | `postgres:18` | Порт 5432 открыт на хост, пароль `postgres` лежит в репозитории |
| MongoDB | `mongo` (без тега) | Без аутентификации, порт 27017 открыт на хост |
| Redis | `redis` (без тега) | Без пароля, порт 6379 открыт на хост, кодом не используется |
| Traefik | `traefik:latest` | Обратный прокси, Let's Encrypt (TLS-ALPN), basic auth на дашборд |
| Nexus | `sonatype/nexus3:latest` | Реестр пакетов и образов (`.ci/nexus.yml`) |
| GitHub Actions runner | `actions-runner:2.319.1`, self-hosted | Деплой — `docker compose up` прямо на сервере |
| Google OAuth | — | `AddGoogle` подключён с заглушками вместо ClientId и ClientSecret |

### NuGet-пакеты

| Пакет | Версия | Где | Замечание |
|---|---|---|---|
| IdentityServer4 (+ AspNetIdentity, EntityFramework) | 4.1.2 | Identity | ⚠️ **EOL с конца 2022 года**, патчей безопасности нет |
| Cnblogs.IdentityServer4.EntityFramework.Storage | 4.2.1 | Identity | Форк поверх оригинального пакета; сборка выдаёт NU1608 |
| IdentityServer4.AccessTokenValidation | 3.0.1 | UserManagement | EOL, написан под netcoreapp3.x |
| AutoMapper | 13.0.0 / 14.0.0 / **15.0.1** | Identity / RC.Api (дубль!) / RC.Application | С 15.x **коммерческая лицензия** (нужен ключ) |
| MediatR | **13.0.0** | RC.Application | С 13.x **коммерческая лицензия** |
| FluentValidation | 12.0.0 | RC.Application | — |
| MongoDB.Driver / MongoDB.EntityFrameworkCore | 3.5.0 / 9.0.1 | RC.Persistence | — |
| Microsoft.Extensions.Caching.StackExchangeRedis | 9.0.9 | RC.Persistence | Подключён, но не используется |
| Newtonsoft.Json | 13.0.4 | RC.Persistence | Только для сериализации кэша, можно заменить на System.Text.Json |
| Npgsql.EntityFrameworkCore.PostgreSQL / Identity.EntityFrameworkCore | 9.0.4 / 9.0.9 | Identity, UM | — |
| Ocelot | 24.0.1 | Gateway | `Ocelot.Provider.Kubernetes` и `Ocelot.Cache.CacheManager` подключены, но закомментированы в коде |
| Swashbuckle / Asp.Versioning | 9.0.4 / 8.1.0 | RC, UM | — |
| Serilog (+AspNetCore, Sinks.File) | 4.3 / 9.0 / 7.0 | Identity, RC.Application | В RemoteConfig не сконфигурирован (см. B5) |
| Frontend (в репозитории) | Bootstrap 4, jQuery 3 | Identity wwwroot | Устаревший UI из QuickStart |

---

## 3. Найденные проблемы

### 🔴 Критичные (безопасность)

- [ ] **S1. Секреты закоммичены в git.**
  - `src/Infrastructure/Identity/IdentityServer/tempkey.jwk` — приватный RSA-ключ подписи токенов. Кто его знает, может выпустить любой JWT.
  - Секрет клиента `Client.Api` лежит в `Config.cs:35` и в `http-client.private.env.json`.
    - [x] Правило в `.gitignore` исправлено на `**/http-client.private.env.json`.
    - [ ] Убрать файл из индекса: `git rm --cached src/Infrastructure/Identity/IdentityServer/http-client.private.env.json`.
  - `.ci/databases.env` и `.ci/services.env` содержат пароли `sa` и Postgres, а также строки подключения.
  - Что сделать: ротировать ключ и секреты, вычистить историю (`git filter-repo`), передавать через secrets/env, использовать нормальный ключ подписи вместо `AddDeveloperSigningCredential`.
- [ ] **S2. Изменять этапы (Stage) может кто угодно без авторизации.** В `StageController.cs:14` атрибут `[AllowAnonymous]` висит на классе и отменяет `[Authorize]` на POST, PUT и DELETE. Компилятор предупреждает об этом (ASP0026). **Подтверждено в Docker:** `POST`/`DELETE /api/1.0/Stage` без токена → 201 / 204 (для сравнения `POST /api/1.0/Hero` → 401).
- [ ] **S3. В Docker-конфигурации Gateway не проверяет авторизацию.** В `Ocelot.Docker.json` нет `AuthenticationOptions` ни на одном маршруте. Сервисы 5002 и 5003 опубликованы на хост в обход gateway.
- [ ] **S4. Базы данных доступны снаружи.** PostgreSQL, Mongo и Redis публикуют порты на хост и не защищены паролями (кроме PostgreSQL со слабым паролем `postgres`). Деплой идёт на `[self-hosted, cloud]`, поэтому на публичном сервере базы, скорее всего, видны из интернета (порты на сервере не проверялись).
- [ ] **S5. UserManagement отдаёт сущность `ApplicationUser` целиком.**
  - `Get` и `GetAll` возвращают `PasswordHash`, `SecurityStamp` и другие внутренние поля.
  - `CreateUserCommand.User` принимает `ApplicationUser` из тела запроса (mass assignment): можно выставить `EmailConfirmed`, `Id`, `LockoutEnabled`.
  - Ролевой модели нет: любой токен со скоупом `ECR.Api` управляет всеми пользователями и ролями.
  - `IdentityModelEventSource.ShowPII = true` включён безусловно.
- [ ] **S6. `Client.Web` использует Resource Owner Password Credentials без секрета.** Грант исключён из OAuth 2.1. Для мобильного клиента — Authorization Code + PKCE.

### 🟠 Ошибки, которые ломают сборку и запуск

- [x] **B1. `.slnx` не открывается.** `dotnet build ECRBackend.slnx`: `Solution folder path '/Services/RemoteConfig' must start and end with '/'`. Исправлено: `/Services/RemoteConfig/` и `/Services/UserManagement/`.
- [ ] **B2. CI, скорее всего, не работает.** `build.yml` вызывает `docker compose up -d --build`, но в корне нет `docker-compose.yml` — есть только `docker-compose.databases.yml` и `docker-compose.services.yml`. Сети `ecr-db` и `ecr-services` объявлены `external` и должны быть созданы заранее.
- [x] **B3. Healthcheck MSSQL указывает на старый путь (не проверено).** Неактуально: MSSQL заменён на PostgreSQL с healthcheck `pg_isready`. Используется `/opt/mssql-tools/bin/sqlcmd`; в актуальных образах `2022-latest` утилита лежит в `/opt/mssql-tools18/bin/sqlcmd`. Если healthcheck не проходит, Identity и Gateway с условием `service_healthy` не стартуют.
- [x] **B3a. MSSQL не запускается на Apple Silicon (colima + qemu).** Решено: MSSQL заменён на `postgres:18` (нативный arm64), миграции Identity и UserManagement пересозданы под Npgsql. Образ только amd64; под qemu `sqlservr` падает с `Invalid mapping of address ... in reserved address space` и уходит в цикл перезапусков. Варианты: `colima start --vm-type vz --vz-rosetta --cpu 4 --memory 6` (VM сейчас 2 ГБ — это минимум MSSQL) или arm64-совместимая БД для локальной разработки. Из-за этого Identity и UserManagement в Docker не проверены.
- [ ] **B4. Redis фактически не используется.** В `RemoteConfig.Persistence/DI/DependencyInjection.cs` регистрируется `AddDistributedMemoryCache()`, `AddStackExchangeRedisCache` не вызывается. `RedisCacheProvider` работает с in-memory кэшем внутри процесса. `CacheUri` и `CacheConfiguration__Uri` никто не читает. **Подтверждено:** после кэшируемых запросов в Redis только ключ `ping` от healthcheck.
- [ ] **B5. Логи RemoteConfig не пишутся.** `LoggingBehavior` пишет через статический `Serilog.Log`, но в `RemoteConfig.Api/Program.cs` Serilog не настроен — сообщения уходят в пустой логгер.
- [x] **B6. Адреса Identity в Docker не совпадали с compose.**
  - [x] `Gateway/appsettings.Docker.json` и `RemoteConfig.Api/appsettings.Docker.json`: `infrastructure-identity` → `ecr-infrastructure-identity`.
  - [x] `IdentityServer/appsettings.Docker.json`: хост БД `usersdb` → `ecr-databases-mssql`.
  - [x] `RemoteConfig.Api/appsettings.Docker.json`: хост Mongo `configsdb` → `ecr-databases-mongo`. Без этого все запросы падали с 500 после 30 с ожидания сервера. Код читает `DatabaseSettings:ConnectionString`, поэтому `ConnectionStrings__RemoteConfig` из `services.env` не применяется — env-ключ стоит привести к коду.
  - [x] `.ci/services.env` задаёт `IdentitySettings__Uri`, а код читает `IdentityUri`. Исправлено: все сервисы читают секцию `IdentitySettings` через `Shared.Identity` (`GetIdentitySettings()`), ключ `IdentityUri` в appsettings заменён на `IdentitySettings:Uri`. Оставлено для `Shared.Identity`; там тоже расхождение: секция в коде `IdentityConfiguration`, в env — `IdentitySettings`.
- [ ] **B7. IdentityServer за Traefik.** Traefik отрезает префикс `/dev/auth`, а `UseForwardedHeaders`/`UsePathBase` не настроены. Discovery-документ отдаст адреса без префикса, внешние клиенты не найдут эндпоинты. `IssuerUri` зафиксирован на внутреннем адресе — валидация issuer у внешних клиентов тоже может не пройти.
- [ ] **B8. Ключ разработчика в контейнере.** Подтверждено: Identity падал с `Access to the path '/app/tempkey.jwk' is denied`; в Dockerfile выдано право записи в `/app`, пересоздание ключа при деплое остаётся. `AddDeveloperSigningCredential()` пишет `tempkey.jwk` в `/app`; образ запускается от `$APP_UID`, у которого может не быть права записи. В любом случае ключ пересоздаётся при каждом деплое, и выданные ранее токены перестают проходить проверку.
- [x] **B9. Маршруты Ocelot для Development были скопированы с ошибками.** UserManagement вёл на порт RemoteConfig (7219) и путь `/api/v1`. Исправлено в `Ocelot.Development.json` и `Ocelot.json`: `/UserManagement/{everything}` → `https://localhost:5003/api/{everything}`.
- [ ] **B10. `/HealthCheck` в RemoteConfig недоступен (404).** У контроллера нет версии, а API-версионирование без `AssumeDefaultVersionWhenUnspecified` не пускает к нему запрос. Пометить контроллер `[ApiVersionNeutral]` или перейти на `MapHealthChecks`.
- [x] **B11. Traefik выбирал не ту сеть.** Gateway и Identity состоят в двух сетях, а метки `traefik.docker.network` не было — Traefik взял IP из `ecr-db`, в которой сам не состоит, и запросы через `localhost/dev/api` висели до таймаута. Исправлено: в `docker-compose.services.yml` добавлена метка `traefik.docker.network=ecr-services`.
  - [ ] Traefik должен быть в сети `ecr-services`. Если он запущен другим проектом, подключить вручную: `docker network connect ecr-services traefik`.

### 🟡 Логика и качество кода

- [ ] **Нет глобальной обработки исключений.** `NotFoundException` и `ValidationException` превращаются в HTTP 500. Нужен `IExceptionHandler` / ProblemDetails с 404 и 400. **Подтверждено:** `GET /api/1.0/Hero/nope` и POST с невалидным телом → 500.
- [ ] **Кэш никогда не устаревает и не инвалидируется.** `GetEnemiesListQuery` кэшируется навсегда, `ExpiredAt` игнорируется. После Create/Update/Delete врагов список остаётся старым до перезапуска. **Подтверждено:** пустой список закэшировался до заливки данных, враги появились только после перезапуска контейнера.
- [ ] **Формат версии в URL.** Маршруты имеют вид `/api/{version:apiVersion}/...`; принимается `1` или `1.0`, а `v1` даёт 404. Клиенты и `http`-коллекции надо привести к одному формату (или сменить шаблон на `v{version:apiVersion}`).
- [ ] **Скрытый баг в `CacheService`.** При одном провайдере `_head.Successor = _tail` при `_head == _tail` — провайдер ссылается сам на себя, промах кэша даёт бесконечную рекурсию.
- [ ] **`InitializeDatabase` в Identity (`Startup.cs`).**
  - Используется `SqlException.ErrorCode == 1801`, а `ErrorCode` — HRESULT; номер ошибки лежит в `Number`.
  - `catch` всё равно делает `throw`, поэтому ничего не перехватывает.
  - Сид выполняется, только если таблица пустая — изменения в `Config.cs` в существующую БД не попадут.
- [ ] **Текущая ветка `fix/identity-settings`.**
  - `IdentityServerConfiguration` читается из конфигурации, но нигде не используется; секции в appsettings нет. При загрузке клиентов из конфига секреты нужно хешировать через `CreateClientSecret`.
  - `CustomCorsPolicyService` при каждом CORS-запросе загружает все origins из БД, достаёт контекст через `RequestServices` вместо DI, у логгера чужая категория `ILogger<DefaultCorsPolicyService>`.
  - `CustomClientConfigurationValidator` вызывает `CreateDomainUri`; для некорректного origin бросит `UriFormatException` вместо `SetError`.
- [ ] **Зависимости.**
  - В `RemoteConfig.Api.csproj` AutoMapper подключён дважды: 15.0.1 и 14.0.0 (NU1504).
  - Identity: NU1608 — EF Storage требует AutoMapper < 11, подключён 13. Риск падения `ToEntity()` во время работы.
- [ ] **Swagger в RemoteConfig.** `AddSecurityDefinition`/`AddSecurityRequirement` вызываются в цикле по версиям API — при второй версии будет дубликат ключа.
- [ ] **Контроллеры RemoteConfig.**
  - `Created($"[/api/heroes/{id}]", …)` формирует некорректный Location.
  - Возвращаемый тип `ActionResult<Guid>`, а id — строка.
  - `DELETE` принимает `[FromBody] string` — id стоит передавать в маршруте.
  - `BaseController.UserId` упадёт с NRE на токене без `sub` (client_credentials).
  - `UseHttpsRedirection` включён в контейнерах, где работает только HTTP.
- [ ] **Валидация.** `ValidatorBehavior` вызывает синхронный `Validate` вместо `ValidateAsync`. Есть пустые валидаторы. `CreateStageCommandValidator` упадёт с NRE при `PlayerSpawnPoint == null`.
- [ ] **Нарушение слоёв.** Application зависит от EF Core (`DbSet` в `IRemoteConfigContext`). Распространённый компромисс, но слой не изолирован от хранилища.
- [ ] **Дублирование кода.** `CurrentUserService` в двух копиях: RemoteConfig.Api и Shared.Identity. `Shared.Identity` собирается на `Sdk.Web`, хотя это библиотека.
- [ ] **Тестов нет.** README упоминает xUnit и Shouldly, тестовых проектов нет.

### ⚪ Гигиена репозитория

- [ ] README устарел: .NET 8, старые версии пакетов, сервис `StaticData` вместо RemoteConfig, другие порты.
- [ ] В `http-client.env.json` неверные порты и опечатка `RemoteCnfig`.
- [ ] `.ci/services.env` ссылается на несуществующие сервисы Economy, Inventory, Shop и на Postgres.
- [ ] Docker-образы без зафиксированных версий (`latest`); `actions/checkout@v3` устарел.
- [ ] `.claude/config.json` Claude Code не читает — правила должны лежать в `.claude/settings.json`. Шаблон `./*appsettings*.json` покрывает только корень; для вложенных файлов нужен `**/appsettings*.json`.

---

## 4. Приоритеты

1. **Срочно:** S1 (ротировать ключ и секреты, вычистить историю), S2, S3, S4, S5.
2. **Чтобы проект собирался и деплоился:** ~~B1~~, B2, B3, B3a, ~~B6~~, B7, B8, B10, ~~B11~~.
3. **Стратегически:**
   - Заменить IdentityServer4 на **Duende IdentityServer** (нужна лицензия) или **OpenIddict** (бесплатный).
   - Перейти с ROPC на Authorization Code + PKCE.
   - Решить вопрос с лицензиями MediatR и AutoMapper: зафиксировать 12.x и 14.x или получить ключи.
   - Разделить БД Identity и UserManagement.
   - Добавить глобальный обработчик ошибок, тесты и healthchecks через `MapHealthChecks`.

---

## 5. Проверка в Docker (2026-11-15)

Окружение: colima (aarch64, 2 CPU, 2 ГБ RAM, эмуляция amd64 через qemu), Traefik уже запущен другим проектом.

**Порядок запуска:**

```bash
docker network create ecr-db && docker network create ecr-services
# 1. базы данных
docker-compose -p ecr -f docker-compose.databases.yml -f docker-compose.services.yml \
  --env-file .ci/services.env up -d ecr-databases-postgres ecr-databases-mongo ecr-databases-redis
# 2. сервисы
docker-compose -p ecr -f docker-compose.databases.yml -f docker-compose.services.yml \
  --env-file .ci/services.env up -d --build ecr-infrastructure-identity ecr-services-usermanagement \
  ecr-services-remoteconfig ecr-infrastructure-gateway
# 3. общий Traefik в сеть сервисов
docker network connect ecr-services traefik
```

**Результат:**

| Компонент | Статус |
|---|---|
| Mongo, Redis | healthy |
| PostgreSQL 18 (вместо MSSQL) | healthy |
| IdentityServer, UserManagement | ✅ работают (миграции применяются при старте) |
| RemoteConfig, Gateway | ✅ работают |

| Запрос | Ответ |
|---|---|
| `GET /api/1.0/{Hero,Enemy,Stage,InventoryItem}` — напрямую `:5003`, через Gateway `:5050/RemoteConfig/...`, через Traefik `localhost/dev/api/RemoteConfig/...` | 200 |
| `GET /api/v1/Hero` | 404 (формат версии) |
| `GET /HealthCheck` | 404 (B10) |
| `POST /api/1.0/Hero` без токена | 401 |
| `POST`/`DELETE /api/1.0/Stage` без токена | 201 / 204 (S2) |
| `GET /api/1.0/Hero/nope`, POST с невалидным телом | 500 (нет обработчика ошибок) |
| `GET :5050/UserManagement/...` | 200 с токеном |
| `POST /connect/token` (client_credentials, `Client.Api`, `ECR.Api`) | 200 |
| `GET :5002/api/Users/all` без токена / с токеном | 401 / 200 |
| `POST :5002/api/Users`, затем `GET :5002/api/Users?username=...` | 200, пользователь сохранён в PostgreSQL |

Изначально записать данные через POST не удалось: IdentityServer не работал без MSSQL (после перехода на PostgreSQL токены выдаются). Тестовые документы (`hero-default`, `enemy-resistor`, `enemy-capacitor`, `enemy-transistor`) вставлены напрямую через `mongosh` в базу `ECR` и читаются через API, Gateway и Traefik.
