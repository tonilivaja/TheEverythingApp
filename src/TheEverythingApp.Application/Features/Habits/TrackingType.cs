namespace TheEverythingApp.Application.Features.Habits;

/// <summary>
/// Represents the type of tracking for a habit.
/// Binary: The habit is either completed or not completed (e.g., "Did I exercise today?").
/// Quantified: The habit is tracked with a specific value (e.g., "How many glasses of water did I drink today?").
/// </summary>
public enum TrackingType
{
    Binary,
    Quantified
}
