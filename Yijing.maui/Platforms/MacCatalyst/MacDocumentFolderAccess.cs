using Foundation;
using UIKit;
using UniformTypeIdentifiers;

namespace Yijing.Services
{
	public sealed class MacDocumentFolderAccess : IDisposable
	{
		private const string BookmarkKey = "Yijing.DocumentFolderBookmark";
		private NSUrl _activeUrl;
		public static MacDocumentFolderAccess Shared { get; } = new MacDocumentFolderAccess();

		public string Restore()
		{
			if (_activeUrl != null)
				return ValidateFolder(_activeUrl);

			string saved = Preferences.Default.Get(BookmarkKey, "");
			if (string.IsNullOrEmpty(saved))
				return null;

			try
			{
				using var bookmark = NSData.FromArray(Convert.FromBase64String(saved));
				return Activate(bookmark, false);
			}
			catch (Exception ex)
			{
				// Preserve the bookmark: a temporarily disconnected volume may return.
				throw new IOException("Access to the saved folder is unavailable. Reconnect it and retry, or choose the folder again.", ex);
			}
		}

		public async Task<string> PickAsync()
		{
			var presenter = Microsoft.Maui.ApplicationModel.Platform.GetCurrentUIViewController()
				?? throw new InvalidOperationException("The folder picker needs an active window.");
			var completion = new TaskCompletionSource<NSUrl>(TaskCreationOptions.RunContinuationsAsynchronously);
			using var pickerDelegate = new FolderPickerDelegate(completion);
			using var picker = new UIDocumentPickerViewController(new[] { UTTypes.Folder }, false)
			{
				AllowsMultipleSelection = false,
				Delegate = pickerDelegate,
				ModalPresentationStyle = UIModalPresentationStyle.FullScreen
			};
			await presenter.PresentViewControllerAsync(picker, true);
			using var selected = await completion.Task;
			if (selected == null)
				return null;

			bool accessing = selected.StartAccessingSecurityScopedResource();
			try
			{
				if (!accessing)
					throw new UnauthorizedAccessException("macOS did not grant access to the selected folder.");

				string selectedPath = selected.Path;
				string dataPath = string.Equals(Path.GetFileName(Path.TrimEndingDirectorySeparator(selectedPath)), "Yijing", StringComparison.OrdinalIgnoreCase)
					? selectedPath : Path.Combine(selectedPath, "Yijing");
				Directory.CreateDirectory(dataPath);
				// Bookmark the data folder itself so moving/renaming it can be resolved on later launches.
				using var dataUrl = NSUrl.FromFilename(dataPath);
				using var bookmark = CreateBookmark(dataUrl);
				return Activate(bookmark, true);
			}
			finally
			{
				if (accessing)
					selected.StopAccessingSecurityScopedResource();
			}
		}

		private string Activate(NSData bookmark, bool save)
		{
			// Apple's NSURL.h supports WithSecurityScope on Mac Catalyst 13+, but the
			// .NET enum's platform annotation currently lists only macOS.
#pragma warning disable CA1416
			var url = NSUrl.FromBookmarkData(bookmark,
				NSUrlBookmarkResolutionOptions.WithSecurityScope | NSUrlBookmarkResolutionOptions.WithoutUI,
				null, out bool stale, out var error);
#pragma warning restore CA1416
			using (error)
			{
				if (url == null || error != null)
				{
					url?.Dispose();
					throw new IOException(error?.LocalizedDescription ?? "The saved folder could not be located.");
				}
			}

			bool accessing = false;
			try
			{
				accessing = url.StartAccessingSecurityScopedResource();
				if (!accessing)
					throw new UnauthorizedAccessException("macOS did not restore access to the data folder.");
				string path = ValidateFolder(url);
				if (save || stale)
				{
					using var refreshed = CreateBookmark(url);
					Preferences.Default.Set(BookmarkKey, Convert.ToBase64String(refreshed.ToArray()));
				}

				Dispose();
				// All database and EEG operations need this scope, including background work.
				// Keep the URL alive until application termination, not just picker dismissal.
				_activeUrl = url;
				return path;
			}
			catch
			{
				if (accessing)
					url.StopAccessingSecurityScopedResource();
				url.Dispose();
				throw;
			}
		}

		private static NSData CreateBookmark(NSUrl url)
		{
			// See the Mac Catalyst availability note in Activate.
#pragma warning disable CA1416
			var data = url.CreateBookmarkData(NSUrlBookmarkCreationOptions.WithSecurityScope, null, null, out var error);
#pragma warning restore CA1416
			using (error)
			{
				if (data == null || error != null)
				{
					data?.Dispose();
					throw new IOException(error?.LocalizedDescription ?? "Folder access could not be saved.");
				}
			}
			return data;
		}

		private static string ValidateFolder(NSUrl url)
		{
			string path = url.Path;
			if (!url.IsFileUrl || string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
				throw new DirectoryNotFoundException("The data folder is no longer available.");

			string probe = Path.Combine(path, $".yijing-access-{Guid.NewGuid():N}");
			using (var stream = new FileStream(probe, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 1, FileOptions.DeleteOnClose))
			{
				stream.WriteByte(0);
				stream.Flush();
			}
			return path;
		}

		public void Dispose()
		{
			if (_activeUrl == null)
				return;
			_activeUrl.StopAccessingSecurityScopedResource();
			_activeUrl.Dispose();
			_activeUrl = null;
		}

		private sealed class FolderPickerDelegate : UIDocumentPickerDelegate
		{
			private readonly TaskCompletionSource<NSUrl> _completion;

			public FolderPickerDelegate(TaskCompletionSource<NSUrl> completion)
			{
				_completion = completion;
			}

			public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
			{
				_completion.TrySetResult(urls.FirstOrDefault());
			}

			public override void WasCancelled(UIDocumentPickerViewController controller)
			{
				_completion.TrySetResult(null);
			}
		}
	}
}
