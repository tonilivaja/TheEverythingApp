# Architecture & Project Structure

## Solution Structure

```
TheEverythingApp/
├── TheEverythingApp.slnx
├── .github/
│   ├── workflows/
│   │   └── backend.yml          # CI: restore + build on push/PR to main
│   └── skills/
│       └── theeverythingapp/    # This skill
├── docker/
│   └── docker-compose.yml       # Local PostgreSQL container
├── src/
│   ├── TheEverythingApp.Application/    # Business logic, entities, interfaces, DTOs
│   ├── TheEverythingApp.Infrastructure/ # EF Core, services, JWT, BCrypt
│   └── TheEverythingApp.WebAPI/         # Controllers, Program.cs, HTTP layer
├── tests/                       # Empty — to be added
└── client/                      # Empty — React frontend to be added
```

## Project References

```
WebAPI → Application
WebAPI → Infrastructure
Infrastructure → Application
Application → (nothing external)
```

`Application` has zero external NuGet dependencies. It only references .NET BCL.

## Layer Responsibilities

| Layer | Responsibility |
|---|---|
| `Application` | Entities, DTOs, service interfaces, enums, config models |
| `Infrastructure` | Service implementations, EF Core DbContext, migrations, EF configs, JWT, BCrypt |
| `WebAPI` | Controllers, Program.cs, middleware config, HTTP concern only |

## Folder Convention — Vertical Slices + Common

Each layer uses this structure:
```
Features/
  Auth/        ← all auth-related code
  Habits/      ← all habits-related code
  ...
Common/        ← shared cross-cutting concerns
```

## Key Packages

### Infrastructure
- `Microsoft.EntityFrameworkCore` 10.0.3
- `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.0
- `BCrypt.Net-Next` 4.0.3
- `Microsoft.IdentityModel.JsonWebTokens` 8.7.0

### WebAPI
- `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.3
- `Microsoft.AspNetCore.OpenApi` 10.0.3
- `Scalar.AspNetCore` 2.4.1
- `Microsoft.EntityFrameworkCore.Design` 10.0.3

## Database

- **Engine**: PostgreSQL 17
- **Local**: Docker container `theeverythingapp-db`, port 5432, DB `theeverythingapp`, user `appuser`
- **ORM**: EF Core with `ApplyConfigurationsFromAssembly` — all entity configs live in `Infrastructure/Persistence/Configurations/`
- **Migrations**: Located in `Infrastructure/Migrations/`

## Authentication

- JWT Bearer tokens
- HMAC-SHA256 signing
- Token claims: `sub` (user ID), `unique_name` (username), `jti`
- Configured via `Jwt` section in appsettings
- All feature endpoints use `[Authorize]` attribute
- Auth endpoints (`/api/auth/register`, `/api/auth/login`) are public

## OpenAPI

- Served at `/openapi/v1.json` (raw JSON)
- Interactive UI at `/scalar/v1` (development only)
- No Swashbuckle — uses .NET built-in OpenAPI + Scalar
