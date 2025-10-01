using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SessionDb.Data;

public class SessionContextFactory : IDesignTimeDbContextFactory<SessionContext>
{
    public SessionContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables(prefix: "SESSIONDB_")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SessionContext>();
        var connectionString = configuration.GetConnectionString("Session");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var databasePath = configuration["PATH"];
            if (string.IsNullOrWhiteSpace(databasePath))
            {
                databasePath = Path.Combine(Directory.GetCurrentDirectory(), "Session.db");
            }

            var directory = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            connectionString = $"Data Source={databasePath}";
        }

        optionsBuilder.UseSqlite(connectionString);

        return new SessionContext(optionsBuilder.Options);
    }
}
