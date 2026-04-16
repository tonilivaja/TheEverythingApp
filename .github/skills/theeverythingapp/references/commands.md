# Useful Commands

## Local Development

### Start PostgreSQL
```
docker compose -f docker/docker-compose.yml up -d
```

### Stop PostgreSQL
```
docker compose -f docker/docker-compose.yml down
```

### Run the API
```
dotnet run --project src/TheEverythingApp.WebAPI
```

### Open Scalar UI
```
https://localhost:7126/scalar/v1
```

### Check DB via psql (inside container)
```
docker exec -it theeverythingapp-db psql -U appuser -d theeverythingapp
```

## EF Core Migrations

### Add migration
```
dotnet ef migrations add <MigrationName> --project src/TheEverythingApp.Infrastructure --startup-project src/TheEverythingApp.WebAPI
```

### Apply migrations
```
dotnet ef database update --project src/TheEverythingApp.Infrastructure --startup-project src/TheEverythingApp.WebAPI
```

### Remove last migration (if not yet applied)
```
dotnet ef migrations remove --project src/TheEverythingApp.Infrastructure --startup-project src/TheEverythingApp.WebAPI
```

## Build & Test

### Restore
```
dotnet restore
```

### Build
```
dotnet build --configuration Release
```

### Run all tests (once test projects exist)
```
dotnet test
```

## Dev Credentials (local only)

| What | Value |
|---|---|
| DB host | `localhost:5432` |
| DB name | `theeverythingapp` |
| DB user | `appuser` |
| DB password | `devpassword123` |
| JWT issuer/audience | `TheEverythingApp` |
| JWT expiry | 1440 minutes (24h) |
