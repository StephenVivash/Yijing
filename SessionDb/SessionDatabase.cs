using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SessionDb;

public static class SessionDatabase
{
    public const string DefaultFileName = "sessions.db";

    public static SessionContext Open(string databasePath)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
            throw new ArgumentException("A database path must be supplied.", nameof(databasePath));

        string fullPath = Path.GetFullPath(databasePath);
        string? directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        SessionContextOptions options = BuildOptions(fullPath);
        var context = new SessionContext(options);

        InitializeDatabase(context);
        return context;
    }

    public static SessionContextOptions BuildOptions(string databasePath)
    {
        var builder = new DbContextOptionsBuilder<SessionContext>();
        builder.UseSqlite($"Data Source={databasePath}");
#if DEBUG
        builder.EnableDetailedErrors();
        builder.EnableSensitiveDataLogging();
#endif
        return new SessionContextOptions(builder.Options);
    }

    private static void InitializeDatabase(SessionContext context)
    {
        if (context.Database.GetMigrations().Any())
        {
            context.Database.Migrate();
        }
        else
        {
            context.Database.EnsureCreated();
            SessionSeed.EnsureSeedData(context);
        }
    }
}
