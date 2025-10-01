using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SessionDb.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

builder.Services.AddDbContext<SessionContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Session"));
});

using var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SessionContext>();
    context.Database.Migrate();

    var sessions = context.Sessions
        .OrderBy(session => session.Id)
        .Select(session => new { session.Id, session.Name, session.Description, session.YijingCast })
        .ToList();

    Console.WriteLine("Seeded session entries:");
    foreach (var session in sessions)
    {
        Console.WriteLine($"[{session.Id}] {session.Name} - {session.Description} ({session.YijingCast})");
    }
}
