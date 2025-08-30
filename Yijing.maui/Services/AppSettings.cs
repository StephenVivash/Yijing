
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

namespace Yijing.Services;

public static class AppSettings
{

	private static string _documentHome = null;
	private static string _eegDataHome = null;

	public static DateTime _lastEegDataTime = DateTime.Now;

	//public static Kernel _kernel = null;
	public static MemoryServerless _memoryServerless = null; 

	private static Eeg _eeg = null;

	public static string DocumentHome() { return _documentHome; }
	public static string EegDataHome() { return _eegDataHome; }

	//public static Kernel Kernel() { return _kernel; }
	public static MemoryServerless MemoryServerless() { return _memoryServerless; }

	public static Eeg Eeg() { return _eeg; }

	public static async void SetDocumentHome()
	{
#if WINDOWS
		_documentHome = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif ANDROID
		_documentHome = FileSystem.Current.AppDataDirectory;
#else
		_documentHome = "";
#endif
		if (Directory.Exists(_documentHome))
		{
			_documentHome = Path.Combine(_documentHome, "Yijing");
			_eegDataHome = _documentHome;
			if (!Directory.Exists(_eegDataHome))
				Directory.CreateDirectory(_eegDataHome);

			String strTemp = Path.Combine(_eegDataHome, "Emotiv");
			if (!Directory.Exists(strTemp))
			{
				Directory.CreateDirectory(strTemp);
				string strSource = "Example-Emotiv.csv";
				string strDestination = Path.Combine(strTemp, strSource);
				CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}

			_eegDataHome = Path.Combine(_eegDataHome, AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? "Emotiv" : "Muse");
			if (!Directory.Exists(_eegDataHome))
			{
				Directory.CreateDirectory(_eegDataHome);
				string strSource = AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? "Example-Emotiv.csv" : "Example-Muse.csv";
				string strDestination = Path.Combine(_eegDataHome, strSource);
				CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}
		}
	}

	private static void CopyStream(Stream stream, string file)
	{
		using StreamReader reader = new StreamReader(stream);
		using StreamWriter writer = new StreamWriter(file);
		writer.Write(reader.ReadToEnd());
	}

	public static string ReverseDateString()
	{
		DateTime dt = DateTime.Now;
		return $"{dt.Year}-{dt.Month,2:#00}-{dt.Day,2:#00}-{dt.Hour,2:#00}-{dt.Minute,2:#00}-{dt.Second,2:#00}";
	}

	public static void EegCreate()
	{
		_eeg = AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? new EmotivEeg() : new MuseEeg();
	}

	public static bool EegIsConnected()
	{
		return _eeg.m_bConnected;
	}

	public static int EegReplaySpeed()
	{
		return _eeg.m_nReplaySpeed;
	}

	public static void EegSetTriggers(bool bHigh, bool bLow)
	{
		_eeg.SetTriggers(bHigh, bLow);
	}

	public static void EegCalculateTriggers()
	{
		_eeg.CalculateTriggers();
	}

	public static void EegIncreaseTriggers(float amount)
	{
		_eeg.IncreaseTriggers(amount);
	}

	public static void EegDecreaseTriggers(float amount)
	{
		_eeg.DecreaseTriggers(amount);
	}

	public static bool EegIsTriggerOn()
	{
		return _eeg.IsTriggerOn();
	}

	public static bool EegIsTriggerOff()
	{
		return _eeg.IsTriggerOff();
	}

	public static EegChannel EegChannel(int index)
	{
		return _eeg.m_eegChannel[index];
	}

}