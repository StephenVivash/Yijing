using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SessionDb.Data;

namespace SessionDb;

public static class SessionDatabaseInitializer
{
    public static void Initialize(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SessionContext>();
        context.Database.Migrate();
    }
}
