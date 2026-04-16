# Planned Features & Roadmap

## Priority Order (as discussed)

1. **Habits** ✅ — done
2. **Frontend** — Vite + React + TypeScript + MUI (client/ folder, empty)
3. **Tests** — unit + integration test projects (tests/ folder, empty)

## Planned Feature Domains

All follow the same slice pattern as Habits.

---

### Utility Bills
Track monthly bills by type (electricity, water, gas, internet, etc.).

**Data model sketch:**
- `UtilityType` — name (e.g. "Electricity"), unit (e.g. "kWh"), optional icon/color
- `UtilityBill` — UtilityTypeId, Year, Month, Amount, IsPaid, PaidAt?, Notes?
- One bill per type per month (unique index on `UtilityTypeId + Year + Month`)

**Key behaviors:**
- Mark a month as paid / unpaid
- Overview: current month status across all types at a glance
- History: all bills for a given type over time
- PDF receipts: upload to Azure Blob Storage, save URL reference in DB

---

### Goals
Annual goals set at the start of the year, progress tracked on the 1st of each month.

**Data model sketch:**
- `Goal` — Title, Description?, Year, TargetValue, Unit (e.g. "books", "kg", "%"), IsCompleted
- `GoalProgress` — GoalId, RecordedAt (DateOnly, expected = 1st of month), CurrentValue, Notes?

**Key behaviors:**
- Create goals for a given year
- Log monthly progress entries
- Progress visualization: value over time toward target
- Mark goal as completed
- Warn if a monthly check-in is missing (computed, not stored)

---

### Body Log
Track body measurements over time. Fully flexible — user defines which metrics to track and their units.

**Data model sketch:**
- `BodyMetric` — Name (e.g. "Weight", "Waist"), Unit (e.g. "kg", "cm"), IsActive
- `BodyEntry` — BodyMetricId, MeasuredAt (DateOnly), Value (decimal)

**Key behaviors:**
- Define any metric with any unit
- Log measurements per metric — intended weekly (e.g. every Monday), not daily
- One entry per metric per day (unique index — prevents accidental duplicates, doesn't enforce weekly schedule)
- Stats: latest value, min/max/avg over a period, trend over time

---

### Supplements
Track daily supplement intake.

**Data model sketch:**
- `Supplement` — Name, Dosage (e.g. "500mg"), Notes?, IsActive
- `SupplementEntry` — SupplementId, TakenAt (DateOnly), Notes?

**Key behaviors:**
- Define supplements with dosage
- Log daily intake (one entry per supplement per day)
- Dashboard: which supplements were/weren't taken today
- Simple history view

---

### Reading List
Track books — want to read, currently reading, finished.

**Data model sketch:**
- `Book` — Title, Author?, Status (`WantToRead | Reading | Finished`), StartedAt?, FinishedAt?, Rating (1–5, nullable), Notes?

**Key behaviors:**
- Add books to backlog
- Update status and dates as you progress
- Rating + notes on completion
- Simple list views filtered by status

---

### To-Do
Simple task list. No projects, no priorities, no due dates for now — just tasks to complete.

**Data model sketch:**
- `TodoItem` — Title, DueDate? (DateOnly), IsCompleted, CompletedAt?, CreatedAt

**Key behaviors:**
- Add, complete, delete tasks
- Optional due date — no enforcement, just a reference
- View open tasks and completed history
- Keep it simple — no subtasks, no labels, no recurring tasks (for now)

## Infrastructure Planned

- **Azure Blob Storage** — for PDF uploads (utility bills)
  - Pattern: API receives upload → streams to Blob → saves URL reference in DB
  - Or: frontend requests SAS URL from API → uploads directly from browser
- **Frontend (Azure Static Web Apps)** — MUI, React Router, JWT stored in memory/httpOnly cookie, `/api/*` proxy to backend
- **CI/CD expansion** — Docker build + push to ACR + deploy to Container Apps (backend.yml has stubs ready)
- **Tests**:
  - Unit tests: `tests/TheEverythingApp.UnitTests/`
  - Integration tests: `tests/TheEverythingApp.IntegrationTests/` — Testcontainers (PostgreSQL), real DB, `WebApplicationFactory`
- **Global exception handler** — middleware to catch unhandled exceptions and return consistent error responses
- **Staging environment** — second Container App + Static Web Apps preview environments

## Decisions Made

| Decision | Choice | Reason |
|---|---|---|
| Auth | Simple JWT, single user | Personal app, no complexity needed |
| Reminders | Not stored in DB | UI shows overdue badges, no push notifications |
| Stats | Computed on the fly | Single user, max ~1500 entries per habit, Postgres handles it trivially |
| TrackingType immutable after creation | Yes | Changing it would invalidate existing entries |
| ORM | EF Core | Migration story is worth it; Dapper available as escape hatch via `FromSqlRaw` |
| OpenAPI UI | Scalar | Built-in .NET OpenAPI + Scalar; no Swashbuckle |
| Frontend hosting | Azure Static Web Apps | Free tier, CDN, HTTPS, built-in proxy for `/api/*` |
| Backend hosting | Azure Container Apps | Scales to zero, cheap, simple Docker deployment |
| Image registry | Azure Container Registry | Native ACR + Container Apps integration |
