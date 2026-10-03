using Yijing.Services;
using YijingData;

namespace Yijing.Pages
{
	// Do not construct AppShell (and its data-dependent views) until storage is ready.
	public sealed class DataSetupPage : ContentPage
	{
		private readonly Label _status = new Label { Text = "Opening Yijing data…" };
		private readonly Button _retry = new Button { Text = "Retry", IsVisible = false };
#if MACCATALYST
		private readonly Button _choose = new Button { Text = "Choose folder…", IsVisible = false };
#endif
		private bool _busy;
		private bool _started;

		public DataSetupPage()
		{
			Title = "Yijing data";
			var layout = new VerticalStackLayout
			{
				Padding = 32,
				Spacing = 20,
				MaximumWidthRequest = 600,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Children = { new Label { Text = "Yijing data", FontSize = 28 }, _status, _retry }
			};
#if MACCATALYST
			layout.Children.Add(_choose);
			_choose.Clicked += async (_, _) => await InitializeAsync(true);
#endif
			Content = layout;
			_retry.Clicked += async (_, _) => await InitializeAsync(false);
			Loaded += async (_, _) =>
			{
				if (_started)
					return;
				_started = true;
				await InitializeAsync(false);
			};
		}

		private async Task InitializeAsync(bool chooseFolder)
		{
			if (_busy)
				return;
			_busy = true;
			_retry.IsVisible = false;
#if MACCATALYST
			_choose.IsVisible = false;
#endif
			string failureMessage = "Yijing could not open its data folder.";
			try
			{
				string documentHome = null;
#if MACCATALYST
				var access = MacDocumentFolderAccess.Shared;
				documentHome = chooseFolder ? await access.PickAsync() : access.Restore();
				if (documentHome == null)
				{
					_status.Text = "Choose your Documents folder to use Documents/Yijing, or choose an existing Yijing folder. " +
						"Your database, EEG recordings and logs will be stored there. Yijing will remember your choice for future launches.";
					_choose.IsVisible = true;
					return;
				}
#endif
				_status.Text = "Opening Yijing data…";
				await AppSettings.LoadAsync(documentHome);
				failureMessage = "Yijing could not initialize its database.";
				var database = new YijingDatabase(Path.Combine(AppSettings.DocumentHome(), "Yijing.db"));
				await Task.Run(database.Initialse);
				failureMessage = "Yijing could not open its main window.";
				Window.Page = new AppShell();
			}
			catch (Exception ex)
			{
				_status.Text = $"{failureMessage} {ex.Message}";
				_retry.IsVisible = true;
#if MACCATALYST
				_choose.IsVisible = true;
#endif
			}
			finally
			{
				_busy = false;
			}
		}
	}
}
