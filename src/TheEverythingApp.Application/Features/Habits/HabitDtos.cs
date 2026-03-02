using System.ComponentModel.DataAnnotations;

namespace TheEverythingApp.Application.Features.Habits;

public record CreateHabitRequest(
    [Required]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters.")]
    string Name,

    [MaxLength(50, ErrorMessage = "Description cannot exceed 50 characters.")]
    string? Description,

    [Required]
    TrackingType TrackingType,

    decimal? TargetValue,

    [MaxLength(50, ErrorMessage = "Unit cannot exceed 50 characters.")]
    string? TargetUnit);

public record UpdateHabitRequest(
    [Required]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters.")]
    string Name,

    [MaxLength(50, ErrorMessage = "Description cannot exceed 50 characters.")]
    string? Description,

    decimal? TargetValue,

    [MaxLength(10, ErrorMessage = "Unit cannot exceed 10 characters.")]
    string? TargetUnit,

    bool IsActive);

public record LogEntryRequest(
    [Required]
    DateOnly CompletedAt,

    decimal? ActualValue,

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    string? Notes);

public record HabitResponse(
    int Id,
    string Name,
    string? Description,
    TrackingType TrackingType,
    decimal? TargetValue,
    string? TargetUnit,
    bool IsActive,
    DateTime CreatedAt);

public record HabitEntryResponse(
    int Id,
    int HabitId,
    DateOnly CompletedAt,
    decimal? ActualValue,
    string? Notes);

public record HabitStatsResponse(
    int HabitId,
    string HabitName,
    int TotalCompletions,
    int CurrentStreak,
    int LongestStreak,
    double CompletionRateLast7Days,
    double CompletionRateLast30Days,
    bool CompletedToday);
