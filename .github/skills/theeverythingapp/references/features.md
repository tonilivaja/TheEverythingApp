# Implemented Features

## Auth (`/api/auth`)

Single-user authentication. No refresh tokens (personal app — 1440 min expiry).

**Entities:**
- `User` — `Id`, `Username`, `PasswordHash`, `CreatedAt`

**Endpoints:**
| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | No | Create account (username min 3, max 15; password min 6) |
| POST | `/api/auth/login` | No | Returns JWT token |

**Notes:**
- Passwords hashed with BCrypt
- Username uniqueness enforced in code + DB unique index

---

## Habits (`/api/habits`)

General-purpose habit tracking. Supports binary (did/didn't) and quantified (how much) habits. No reminders stored in DB — overdue habits surfaced in UI.

**Entities:**
- `Habit` — `Id (private set)`, `Name`, `Description?`, `TrackingType`, `TargetValue?`, `TargetUnit?`, `IsActive`, `CreatedAt`
- `HabitEntry` — `Id (private set)`, `HabitId`, `CompletedAt (DateOnly)`, `ActualValue?`, `Notes?`
- `TrackingType` enum — `Binary`, `Quantified`

**EF Constraints:**
- `Habit.Name` — required, max 25 chars, stored as string enum for `TrackingType`
- `HabitEntry` — unique index on `(HabitId, CompletedAt)` — one entry per habit per day
- Cascade delete: deleting a habit deletes all its entries

**Endpoints:**
| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/habits` | Yes | List active habits (`?includeInactive=true` for all) |
| GET | `/api/habits/{id}` | Yes | Get single habit |
| POST | `/api/habits` | Yes | Create habit |
| PUT | `/api/habits/{id}` | Yes | Update habit (TrackingType cannot be changed) |
| DELETE | `/api/habits/{id}` | Yes | Delete habit + all entries (cascade) |
| POST | `/api/habits/{id}/entries` | Yes | Log a completion for a date |
| DELETE | `/api/habits/{id}/entries/{entryId}` | Yes | Remove a log entry |
| GET | `/api/habits/{id}/entries` | Yes | List entries (`?from=&to=` date filters) |
| GET | `/api/habits/{id}/stats` | Yes | Stats: streak, completion rates, total |

**Statistics (computed on the fly):**
- `TotalCompletions` — count of all entries
- `CurrentStreak` — consecutive days up to today (if today logged) or yesterday
- `LongestStreak` — longest consecutive run ever
- `CompletionRateLast7Days` — % of last 7 days completed
- `CompletionRateLast30Days` — % of last 30 days completed
- `CompletedToday` — boolean

**Migrations applied:**
- `InitialCreate` — empty initial migration
- `AddUsers` — users table
- `AddHabits` — habits + habit_entries tables with unique index
