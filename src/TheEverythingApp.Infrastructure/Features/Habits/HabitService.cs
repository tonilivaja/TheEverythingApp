using Microsoft.EntityFrameworkCore;
using TheEverythingApp.Application.Features.Habits;
using TheEverythingApp.Infrastructure.Persistence;

namespace TheEverythingApp.Infrastructure.Features.Habits;

public class HabitService : IHabitService
{
    private readonly AppDbContext _context;

    public HabitService(AppDbContext context)
    {
        _context = context;
    }

    // ── CRUD ─────────────────────────────────────────────────────────────────

    public async Task<IEnumerable<HabitResponse>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.Habits.AsNoTracking();

        if (!includeInactive)
            query = query.Where(h => h.IsActive);

        var habits = await query
            .OrderBy(h => h.Name)
            .ToListAsync();

        return habits.Select(ToResponse);
    }

    public async Task<HabitResponse?> GetByIdAsync(int id)
    {
        var habit = await _context.Habits
            .AsNoTracking()
            .SingleOrDefaultAsync(h => h.Id == id);

        return habit is null ? null : ToResponse(habit);
    }

    public async Task<HabitResponse> CreateAsync(CreateHabitRequest request)
    {
        var habit = new Habit
        {
            Name = request.Name,
            Description = request.Description,
            TrackingType = request.TrackingType,
            TargetValue = request.TrackingType == TrackingType.Quantified ? request.TargetValue : null,
            TargetUnit = request.TrackingType == TrackingType.Quantified ? request.TargetUnit : null
        };

        _context.Habits.Add(habit);
        await _context.SaveChangesAsync();

        return ToResponse(habit);
    }

    public async Task<HabitResponse?> UpdateAsync(int id, UpdateHabitRequest request)
    {
        var habit = await _context.Habits.SingleOrDefaultAsync(h => h.Id == id);

        if (habit is null)
            return null;

        habit.Name = request.Name;
        habit.Description = request.Description;
        habit.TargetValue = request.TargetValue;
        habit.TargetUnit = request.TargetUnit;
        habit.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return ToResponse(habit);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var habit = await _context.Habits.SingleOrDefaultAsync(h => h.Id == id);

        if (habit is null)
            return false;

        _context.Habits.Remove(habit);
        await _context.SaveChangesAsync();

        return true;
    }

    // ── ENTRIES ───────────────────────────────────────────────────────────────

    public async Task<HabitEntryResponse> LogEntryAsync(int habitId, LogEntryRequest request)
    {
        var exists = await _context.Habits.AnyAsync(h => h.Id == habitId);
        if (!exists)
            throw new InvalidOperationException($"Habit with id {habitId} not found.");

        var alreadyLogged = await _context.HabitEntries
            .AnyAsync(e => e.HabitId == habitId && e.CompletedAt == request.CompletedAt);

        if (alreadyLogged)
            throw new InvalidOperationException($"An entry for {request.CompletedAt} already exists.");

        var entry = new HabitEntry
        {
            HabitId = habitId,
            CompletedAt = request.CompletedAt,
            ActualValue = request.ActualValue,
            Notes = request.Notes
        };

        _context.HabitEntries.Add(entry);
        await _context.SaveChangesAsync();

        return ToEntryResponse(entry);
    }

    public async Task<bool> DeleteEntryAsync(int habitId, int entryId)
    {
        var entry = await _context.HabitEntries
            .SingleOrDefaultAsync(e => e.Id == entryId && e.HabitId == habitId);

        if (entry is null)
            return false;

        _context.HabitEntries.Remove(entry);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<HabitEntryResponse>> GetEntriesAsync(
        int habitId, DateOnly? from = null, DateOnly? to = null)
    {
        var query = _context.HabitEntries
            .AsNoTracking()
            .Where(e => e.HabitId == habitId);

        if (from.HasValue)
            query = query.Where(e => e.CompletedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(e => e.CompletedAt <= to.Value);

        var entries = await query
            .OrderByDescending(e => e.CompletedAt)
            .ToListAsync();

        return entries.Select(ToEntryResponse);
    }

    // ── STATISTICS ────────────────────────────────────────────────────────────

    public async Task<HabitStatsResponse?> GetStatsAsync(int habitId)
    {
        var habit = await _context.Habits
            .AsNoTracking()
            .SingleOrDefaultAsync(h => h.Id == habitId);

        if (habit is null)
            return null;

        var allDates = await _context.HabitEntries
            .AsNoTracking()
            .Where(e => e.HabitId == habitId)
            .Select(e => e.CompletedAt)
            .ToListAsync();

        var dateSet = new HashSet<DateOnly>(allDates);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return new HabitStatsResponse(
            HabitId: habitId,
            HabitName: habit.Name,
            TotalCompletions: allDates.Count,
            CurrentStreak: CalculateCurrentStreak(dateSet, today),
            LongestStreak: CalculateLongestStreak(allDates),
            CompletionRateLast7Days: CalculateCompletionRate(dateSet, today, 7),
            CompletionRateLast30Days: CalculateCompletionRate(dateSet, today, 30),
            CompletedToday: dateSet.Contains(today));
    }

    // ── HELPERS ───────────────────────────────────────────────────────────────

    private static int CalculateCurrentStreak(HashSet<DateOnly> dates, DateOnly today)
    {
        // Start from today if logged, otherwise from yesterday
        var cursor = dates.Contains(today) ? today : today.AddDays(-1);

        if (!dates.Contains(cursor))
            return 0;

        int streak = 0;
        while (dates.Contains(cursor))
        {
            streak++;
            cursor = cursor.AddDays(-1);
        }
        return streak;
    }

    private static int CalculateLongestStreak(IList<DateOnly> dates)
    {
        if (dates.Count == 0) return 0;

        var sorted = dates.OrderBy(d => d).ToList();
        int longest = 1;
        int current = 1;

        for (int i = 1; i < sorted.Count; i++)
        {
            if (sorted[i] == sorted[i - 1].AddDays(1))
            {
                current++;
                if (current > longest)
                    longest = current;
            }
            else
            {
                current = 1;
            }
        }

        return longest;
    }

    private static double CalculateCompletionRate(HashSet<DateOnly> dates, DateOnly today, int days)
    {
        int count = 0;
        for (int i = 0; i < days; i++)
        {
            if (dates.Contains(today.AddDays(-i)))
                count++;
        }
        return Math.Round((double)count / days * 100, 1);
    }

    private static HabitResponse ToResponse(Habit h) => new(
        h.Id, h.Name, h.Description, h.TrackingType,
        h.TargetValue, h.TargetUnit, h.IsActive, h.CreatedAt);

    private static HabitEntryResponse ToEntryResponse(HabitEntry e) => new(
        e.Id, e.HabitId, e.CompletedAt, e.ActualValue, e.Notes);
}
