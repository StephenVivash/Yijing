using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SessionDb.Data;

namespace SessionDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessionDatabase(this IServiceCollection services, string databasePath)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (string.IsNullOrWhiteSpace(databasePath))
        {
            throw new ArgumentException("A valid database path is required.", nameof(databasePath));
        }

        var directory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        services.AddDbContext<SessionContext>(options =>
        {
            options.UseSqlite($"Data Source={databasePath}");
        });

        return services;
    }
}
