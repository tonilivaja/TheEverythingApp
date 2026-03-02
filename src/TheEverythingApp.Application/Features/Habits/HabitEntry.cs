namespace TheEverythingApp.Application.Features.Habits;

public class HabitEntry
{
    public int Id { get; private set; }
    public int HabitId { get; set; }
    public DateOnly CompletedAt { get; set; }
    public decimal? ActualValue { get; set; }
    public string? Notes { get; set; }

    public Habit Habit { get; set; } = null!;
}
