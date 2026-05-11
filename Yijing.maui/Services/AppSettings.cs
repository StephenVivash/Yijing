
using YijingData;

namespace Yijing.Services;

public static class AppSettings
{
	private static string _documentHome = null;
	private static string _eegDataHome = null;
	public static DateTime _lastEegDataTime = DateTime.Now;
	public static int TriggerIndex;

	public static string DocumentHome() { return _documentHome; }
	public static string EegDataHome() { return _eegDataHome; }
	public static string LogHome() { return Path.Combine(_documentHome, "Log"); }

	public static void Load()
	{
		SetDocumentHome();
	}

	public static async void SetDocumentHome()
	{
#if WINDOWS
		_documentHome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Yijing");
#elif ANDROID
		_documentHome = GetAndroidDocumentHome();
#else
		_documentHome = "";
#endif
		if (!string.IsNullOrWhiteSpace(_documentHome))
		{
			Directory.CreateDirectory(_documentHome);
			_eegDataHome = Path.Combine(_documentHome, AppPreferences.EegDevice == (int)eEegDevice.eEmotiv ? "Emotiv" : "Muse");
			Directory.CreateDirectory(LogHome());
			/*
			String strTemp = Path.Combine(_documentHome, "appsettings.json");
			if (!File.Exists(strTemp))
			{
				string strSource = "appsettings.json";
				string strDestination = strTemp;
				CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}
			*/
			String strTemp = Path.Combine(_documentHome, "Muse");
			if (!Directory.Exists(strTemp))
			{
				Directory.CreateDirectory(strTemp);
				string strSource = "Example-Muse.csv";
				string strDestination = Path.Combine(strTemp, ReverseDateString() + "-Muse.csv");
				CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}
			strTemp = Path.Combine(_documentHome, "Emotiv");
			if (!Directory.Exists(strTemp))
			{
				Directory.CreateDirectory(strTemp);
				string strSource = "Example-Emotiv.csv";
				string strDestination = Path.Combine(strTemp, ReverseDateString(1) + "-Emotiv.csv");
				CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}
			strTemp = Path.Combine(_documentHome, "Questions");
			if (!Directory.Exists(strTemp))
			{
				Directory.CreateDirectory(strTemp);
				//string strSource = "111.txt";
				//string strDestination = Path.Combine(strTemp, strSource);
				//CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}
			strTemp = Path.Combine(_documentHome, "Answers");
			if (!Directory.Exists(strTemp))
			{
				Directory.CreateDirectory(strTemp);
				//string strSource = "111.txt";
				//string strDestination = Path.Combine(strTemp, strSource);
				//CopyStream(await FileSystem.OpenAppPackageFileAsync(strSource), strDestination);
			}
		}
	}

#if ANDROID
	private static string GetAndroidDocumentHome()
	{
		string[] candidates =
		[
			GetAndroidPublicFolder(Android.OS.Environment.DirectoryDocuments),
			GetAndroidPublicFolder(Android.OS.Environment.DirectoryDownloads),
			Path.Combine(FileSystem.Current.AppDataDirectory, "Yijing")
		];

		foreach (string candidate in candidates)
		{
			if (TryUseDocumentHome(candidate))
				return candidate;
		}

		return Path.Combine(FileSystem.Current.AppDataDirectory, "Yijing");
	}

	private static string GetAndroidPublicFolder(string folderType)
	{
		Java.IO.File folder = Android.OS.Environment.GetExternalStoragePublicDirectory(folderType);
		return Path.Combine(folder.AbsolutePath, "Yijing");
	}

	private static bool TryUseDocumentHome(string directory)
	{
		if (string.IsNullOrWhiteSpace(directory))
			return false;

		try
		{
			Directory.CreateDirectory(directory);
			string probe = Path.Combine(directory, ".yijing-write-test");
			File.WriteAllText(probe, "");
			File.Delete(probe);
			return true;
		}
		catch
		{
			return false;
		}
	}
#endif

	private static void CopyStream(Stream stream, string file)
	{
		using StreamReader reader = new StreamReader(stream);
		using StreamWriter writer = new StreamWriter(file);
		writer.Write(reader.ReadToEnd());
	}

	public static string ReverseDateString(int minutes = 0)
	{
		DateTime dt = DateTime.Now.AddMinutes(minutes);
		return $"{dt.Year}-{dt.Month,2:#00}-{dt.Day,2:#00}-{dt.Hour,2:#00}-{dt.Minute,2:#00}-{dt.Second,2:#00}";
	}
}
