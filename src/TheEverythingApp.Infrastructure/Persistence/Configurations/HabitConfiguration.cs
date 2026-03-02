using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheEverythingApp.Application.Features.Habits;

namespace TheEverythingApp.Infrastructure.Persistence.Configurations;

public class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(h => h.Description)
            .HasMaxLength(50);

        builder.Property(h => h.TrackingType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(h => h.TargetValue)
            .HasPrecision(10, 2);

        builder.Property(h => h.TargetUnit)
            .HasMaxLength(10);

        builder.Property(h => h.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(h => h.CreatedAt)
            .IsRequired();

        builder.HasMany(h => h.Entries)
            .WithOne(e => e.Habit)
            .HasForeignKey(e => e.HabitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
