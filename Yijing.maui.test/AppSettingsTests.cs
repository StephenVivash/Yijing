using Xunit;
using Yijing.Services;

namespace Yijing.Maui.Test
{
	public sealed class AppSettingsTests : IDisposable
	{
		private readonly string _folder = Path.Combine(Path.GetTempPath(), "Yijing-storage-tests", Guid.NewGuid().ToString("N"));

		[Fact]
		public async Task LoadAsyncWaitsForSampleFilesBeforeCompleting()
		{
			var sample = new TaskCompletionSource<Stream>();
			FileSystem.OpenPackageFile = _ => sample.Task;
			Task loading = AppSettings.LoadAsync(_folder);
			Assert.False(loading.IsCompleted);
			FileSystem.OpenPackageFile = _ => Task.FromResult<Stream>(new MemoryStream("emotiv"u8.ToArray()));
			sample.SetResult(new MemoryStream("muse"u8.ToArray()));
			await loading;

			Assert.Equal("muse", File.ReadAllText(Assert.Single(Directory.GetFiles(Path.Combine(_folder, "Muse")))));
			Assert.Equal("emotiv", File.ReadAllText(Assert.Single(Directory.GetFiles(Path.Combine(_folder, "Emotiv")))));
			foreach (string name in new[] { "Log", "Questions", "Answers" })
				Assert.True(Directory.Exists(Path.Combine(_folder, name)));
		}

		[Fact]
		public async Task LoadAsyncPreservesExistingDataAndDoesNotReseedExistingFolders()
		{
			Directory.CreateDirectory(Path.Combine(_folder, "Muse"));
			Directory.CreateDirectory(Path.Combine(_folder, "Emotiv"));
			string recording = Path.Combine(_folder, "Muse", "recording.csv");
			string database = Path.Combine(_folder, "Yijing.db");
			File.WriteAllText(recording, "existing recording");
			File.WriteAllText(database, "existing database");
			FileSystem.OpenPackageFile = _ => throw new InvalidOperationException("Existing folders must not be reseeded.");

			await AppSettings.LoadAsync(_folder);
			await AppSettings.LoadAsync(_folder);

			Assert.Equal("existing recording", File.ReadAllText(recording));
			Assert.Equal("existing database", File.ReadAllText(database));
			Assert.Single(Directory.GetFiles(Path.Combine(_folder, "Muse")));
		}

		[Fact]
		public async Task ChangingEegDeviceKeepsTheAuthorizedDataFolder()
		{
			FileSystem.OpenPackageFile = _ => Task.FromResult<Stream>(new MemoryStream("sample"u8.ToArray()));
			await AppSettings.LoadAsync(_folder);
			AppPreferences.EegDevice = (int)YijingData.eEegDevice.eEmotiv;
			AppSettings.UpdateEegDataHome();
			Assert.Equal(Path.Combine(_folder, "Emotiv"), AppSettings.EegDataHome());
			AppPreferences.EegDevice = (int)YijingData.eEegDevice.eMuse;
			AppSettings.UpdateEegDataHome();
			Assert.Equal(Path.Combine(_folder, "Muse"), AppSettings.EegDataHome());
			Assert.Equal(_folder, AppSettings.DocumentHome());
		}

		public void Dispose()
		{
			if (Directory.Exists(_folder))
				Directory.Delete(_folder, true);
		}
	}
}
