using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheEverythingApp.Application.Features.Habits;

namespace TheEverythingApp.Infrastructure.Persistence.Configurations;

public class HabitEntryConfiguration : IEntityTypeConfiguration<HabitEntry>
{
    public void Configure(EntityTypeBuilder<HabitEntry> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompletedAt)
            .IsRequired();

        builder.Property(e => e.ActualValue)
            .HasPrecision(10, 2);

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        // Prevent duplicate entries for the same habit on the same day
        builder.HasIndex(e => new { e.HabitId, e.CompletedAt })
            .IsUnique();
    }
}
