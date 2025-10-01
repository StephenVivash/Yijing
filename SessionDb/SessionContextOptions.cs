using Microsoft.EntityFrameworkCore;

namespace SessionDb;

public sealed class SessionContextOptions
{
    public SessionContextOptions(DbContextOptions<SessionContext> options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public DbContextOptions<SessionContext> Options { get; }

    public static implicit operator DbContextOptions<SessionContext>(SessionContextOptions options) => options.Options;
}
