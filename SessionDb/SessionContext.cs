using Microsoft.EntityFrameworkCore;
using SessionDb.Entities;

namespace SessionDb;

public sealed class SessionContext : DbContext
{
    public SessionContext(SessionContextOptions options)
        : base(options?.Options ?? throw new ArgumentNullException(nameof(options)))
    {
    }

    public SessionContext(DbContextOptions<SessionContext> options)
        : base(options)
    {
    }

    public DbSet<SessionRecord> Sessions => Set<SessionRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entity = modelBuilder.Entity<SessionRecord>();
        entity.ToTable("Sessions");
        entity.HasKey(record => record.Id);
        entity.Property(record => record.FileName)
              .HasMaxLength(256)
              .IsRequired();
        entity.Property(record => record.Name)
              .HasMaxLength(256)
              .IsRequired();
        entity.Property(record => record.Description)
              .HasMaxLength(2048);
        entity.Property(record => record.YijingCast)
              .HasMaxLength(128);
        entity.Property(record => record.CreatedAt)
              .IsRequired();
        entity.HasIndex(record => record.FileName)
              .IsUnique();
    }
}
