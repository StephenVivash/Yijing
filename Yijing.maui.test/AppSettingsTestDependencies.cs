// Minimal MAUI dependencies for exercising the production storage initializer on net10.0.
namespace Yijing.Services
{
	internal static class FileSystem
	{
		public static Func<string, Task<Stream>> OpenPackageFile = _ => throw new InvalidOperationException();
		public static Task<Stream> OpenAppPackageFileAsync(string name) => OpenPackageFile(name);
	}

	internal static class AppPreferences
	{
		public static int EegDevice;
	}
}

namespace YijingData
{
	internal enum eEegDevice { eNone, eMuse, eEmotiv }
}
