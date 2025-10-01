namespace SessionDb.Models;

public class SessionEntry
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? YijingCast { get; set; }
}
