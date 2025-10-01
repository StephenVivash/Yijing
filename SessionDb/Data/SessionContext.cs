using Microsoft.EntityFrameworkCore;
using SessionDb.Models;

namespace SessionDb.Data;

public class SessionContext(DbContextOptions<SessionContext> options) : DbContext(options)
{
    public DbSet<SessionEntry> Sessions => Set<SessionEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<SessionEntry>();

        entity.ToTable("Sessions");
        entity.HasKey(session => session.Id);

        entity.Property(session => session.Name)
            .IsRequired()
            .HasMaxLength(20);

        entity.Property(session => session.Description)
            .HasMaxLength(100);

        entity.Property(session => session.YijingCast)
            .HasMaxLength(50);

        entity.HasData(
            new SessionEntry
            {
                Id = 1,
                Name = "Dawn Study",
                Description = "Morning meditation focusing on hexagram insight.",
                YijingCast = "Hexagram 24"
            },
            new SessionEntry
            {
                Id = 2,
                Name = "Lunar Notes",
                Description = "Evening reflections guided by the I Ching.",
                YijingCast = "Hexagram 15"
            },
            new SessionEntry
            {
                Id = 3,
                Name = "Oracle Circle",
                Description = "Group reading exploring seasonal change.",
                YijingCast = "Hexagram 49"
            }
        );
    }
}
