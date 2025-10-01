using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SessionDb.Data;

namespace SessionDb;

public static class SessionDatabaseInitializer
{
    private static readonly SemaphoreSlim InitializationLock = new(1, 1);
    private static bool _initialized;

    public static void Initialize(IServiceProvider services) =>
        InitializeAsync(services).GetAwaiter().GetResult();

    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (_initialized)
        {
            return;
        }

        await InitializationLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (_initialized)
            {
                return;
            }

            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SessionContext>();
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

            _initialized = true;
        }
        finally
        {
            InitializationLock.Release();
        }
    }
}
