using SessionDb.Entities;
using System.Collections.Generic;
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

        DateTime now = DateTime.UtcNow;

        var sessions = new List<SessionRecord>
        {
            new()
            {
                FileName = "introduction.json",
                Name = "Introduction to the Yijing",
                Description = "A guided walkthrough of the Yijing practice journal and how to record your first session.",
                YijingCast = "Hexagram 1 - 乾 (Qián)",
                Eeg = false,
                CreatedAt = now.AddDays(-21),
                UpdatedAt = now.AddDays(-14)
            },
            new()
            {
                FileName = "daily-reflection.json",
                Name = "Daily Reflection",
                Description = "A morning ritual focusing on intention setting and drawing insight from the changing lines.",
                YijingCast = "Hexagram 24 - 復 (Fù)",
                Eeg = false,
                CreatedAt = now.AddDays(-7),
                UpdatedAt = now.AddDays(-2)
            },
            new()
            {
                FileName = "evening-review.json",
                Name = "Evening Review",
                Description = "Close the day by reviewing prior guidance and journaling new observations for tomorrow.",
                YijingCast = "Hexagram 63 - 既濟 (Jì Jì)",
                Eeg = true,
                CreatedAt = now.AddDays(-1)
            }
        };

        context.Sessions.AddRange(sessions);
        context.SaveChanges();
    }
}
