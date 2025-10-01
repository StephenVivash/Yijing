using SessionDb.Entities;
using System.Linq;

namespace SessionDb;

public static class SessionSeed
{
    public static void EnsureSeedData(SessionContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        if (context.Sessions.Any())
            return;

        // Intentionally left empty for now. Add default records here when required.
    }
}
