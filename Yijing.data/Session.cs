namespace YijingData;

using System.ComponentModel.DataAnnotations;

public enum eEegDevice { eMuse, eEmotiv, eNone };

public partial class Session
{

	public Session(int id, string name, string? description = null, string? fileName = null,
		string? yijingCast = null, bool meditation = false, eEegDevice eegDevice = eEegDevice.eNone, bool eegAnalysis = false)
	{
		Id = id;
		Name = name;
		Description = description;
		FileName = fileName;
		YijingCast = yijingCast;
		Meditation = meditation;
		EegDevice = eegDevice;
		EegAnalysis = eegAnalysis;
	}

	public int Id { get; set; }

	[Required, StringLength(100)]
	public string Name { get; set; }

	public string? Description { get; set; }

	public string? FileName { get; set; }

	public string? YijingCast { get; set; }
	
	public bool Meditation { get; set; }

	public eEegDevice EegDevice { get; set; }

	public bool EegAnalysis { get; set; }
}
