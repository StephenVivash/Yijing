namespace YijingData;

using System.ComponentModel.DataAnnotations;

public partial class Text
{

	public Text(int id, string name, string? description = null)
	{
		Id = id;
		Name = name;
		Description = description;
	}

	public int Id { get; set; }

	[Required, StringLength(100)]
	public string Name { get; set; }

	[StringLength(100)]
	public string? Description { get; set; }
}
