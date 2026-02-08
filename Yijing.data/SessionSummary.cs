namespace YijingData;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Table("SessionSummary")]
[Index(nameof(SessionId), IsUnique = true)]
public class SessionSummary
{
	public int Id { get; set; }

	[Required]
	public int SessionId { get; set; }

	[Required]
	public string Summary { get; set; } = string.Empty;

	[Required]
	public string Keywords { get; set; } = string.Empty; // comma-separated

	public Session Session { get; set; } = null!;
}
