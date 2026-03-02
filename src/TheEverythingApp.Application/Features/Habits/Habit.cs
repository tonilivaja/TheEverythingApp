namespace TheEverythingApp.Application.Features.Habits;

public class Habit
{
    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TrackingType TrackingType { get; set; }
    public decimal? TargetValue { get; set; }
    public string? TargetUnit { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HabitEntry> Entries { get; set; } = [];
}
