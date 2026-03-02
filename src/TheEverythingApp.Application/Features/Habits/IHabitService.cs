namespace TheEverythingApp.Application.Features.Habits;

public interface IHabitService
{
    Task<IEnumerable<HabitResponse>> GetAllAsync(bool includeInactive = false);
    Task<HabitResponse?> GetByIdAsync(int id);
    Task<HabitResponse> CreateAsync(CreateHabitRequest request);
    Task<HabitResponse?> UpdateAsync(int id, UpdateHabitRequest request);
    Task<bool> DeleteAsync(int id);

    Task<HabitEntryResponse> LogEntryAsync(int habitId, LogEntryRequest request);
    Task<bool> DeleteEntryAsync(int habitId, int entryId);
    Task<IEnumerable<HabitEntryResponse>> GetEntriesAsync(int habitId, DateOnly? from = null, DateOnly? to = null);

    Task<HabitStatsResponse?> GetStatsAsync(int habitId);
}
