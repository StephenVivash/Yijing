namespace YijingData;

using System.ComponentModel.DataAnnotations;

public partial class Session
{

	public Session(int id, string name, string? description = null, string? fileName = null,
		string? yijingCast = null)
	{
		Id = id;
		Name = name;
		Description = description;
		FileName = fileName;
		YijingCast = yijingCast;
	}

	public int Id { get; set; }

	[Required, StringLength(100)]
	public string Name { get; set; }

	public string? Description { get; set; }

	public string ?FileName { get; set; }

	public string? YijingCast { get; set; }
}
