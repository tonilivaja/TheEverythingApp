# Coding Conventions

## General

- **Target framework**: .NET 10
- **Nullable reference types**: enabled (use `?` explicitly for nullable, `string.Empty` over `null` for required strings)
- **Entity IDs**: `public int Id { get; private set; }` — DB sets it, application code never sets it
- **Async**: all service and repository methods are async, suffixed with `Async`
- **No comments** on obvious code; section dividers `// ── SECTION ───` used in longer files

## Entities

- Live in `Application/Features/{Feature}/`
- Navigation properties always paired with explicit FK: `public int HabitId { get; set; }` + `public Habit Habit { get; set; } = null!;`
- Collection navigation properties initialized with `= []`
- `CreatedAt` defaults to `DateTime.UtcNow`

## DTOs

- C# `record` types — one file per feature named `{Feature}Dtos.cs`
- Separate records for each direction and operation: `CreateXRequest`, `UpdateXRequest`, `XResponse`, `XStatsResponse`, etc.
- Data annotations on request records for validation (`[Required]`, `[MinLength]`, `[MaxLength]`)
- `[ApiController]` handles model validation automatically — no manual `ModelState.IsValid` checks

## Services

- Interface in `Application/Features/{Feature}/I{Feature}Service.cs`
- Implementation in `Infrastructure/Features/{Feature}/{Feature}Service.cs`
- Registered in `Infrastructure/ServiceCollectionExtension.cs` as `Scoped`
- Return `null` for not-found cases; throw `InvalidOperationException` for business rule violations
- Use `.AsNoTracking()` on all read-only queries

## Controllers

- In `WebAPI/Features/{Feature}/{Feature}Controller.cs`
- `[ApiController]`, `[Authorize]`, `[Route("api/[controller]")]` on class
- Auth controller is the only exception — no `[Authorize]` at class level
- Return `NotFound()` when service returns `null`; `Conflict(new { message })` for `InvalidOperationException`
- Use `CreatedAtAction` for POST responses

## EF Core

- All entity configuration via `IEntityTypeConfiguration<T>` classes in `Infrastructure/Persistence/Configurations/`
- Loaded via `ApplyConfigurationsFromAssembly` in `AppDbContext.OnModelCreating`
- `TrackingType` enum stored as string in DB (`.HasConversion<string>()`)
- DB-level constraints added via `HasCheckConstraint` for rules EF doesn't enforce natively
- Decimal columns use `.HasPrecision(10, 2)`

## Error Handling

- Not-found: return `null` from service → `NotFound()` in controller
- Business rule violation: throw `InvalidOperationException` from service → caught in controller → `Conflict(new { message })`
- No global exception handler yet (to be added)

## Dependency Injection

- All infrastructure registrations in `ServiceCollectionExtension.AddInfrastructure()`
- Called once in `Program.cs` — controllers should never `new` up services
