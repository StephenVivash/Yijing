
// dotnet nuget why Yijing.maui.csproj Microsoft.SemanticKernel

using System.IO;
using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using SessionDb;
using SkiaSharp.Views.Maui.Controls.Hosting;

using Yijing.Services;
using Yijing.Views;

namespace Yijing;

public static class MauiProgram
{

	public static MauiApp CreateMauiApp()
	{
		AppSettings.SetDocumentHome();
		AppPreferences.Load();

		MauiAppBuilder builder = MauiApp.CreateBuilder();
#if WINDOWS || ANDROID
		builder
			.UseMauiApp<App>()
			.UseLiveCharts()
			.UseSkiaSharp()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//builder.Services.AddSingleton<Views.DiagramView>();
		//builder.Services.AddSingleton<Views.EegView>();
		//builder.Services.AddSingleton<Pages.DiagramPage>();
		//builder.Services.AddSingleton<Pages.EegPage>();

#if ANDROID || IOS
		//builder.Services.AddTransient<Services.SampleDataService>();
		//builder.Services.AddSingleton<Pages.WebPage>();
		//builder.Services.AddSingleton<Pages.ChartPage>();
		//builder.Services.AddSingleton<Pages.ListDetailPage>();
		//builder.Services.AddTransient<Pages.ListDetailDetailPage>();
#endif

                Services.AudioPlayer.Load();

                MauiApp app = builder.Build();
                InitializeSessionDatabaseIfNeeded();
                return app;
        }

        private static void InitializeSessionDatabaseIfNeeded()
        {
                if (!AppPreferences.PrefetchSessionDatabase)
                        return;

                string databasePath = GetSessionDatabasePath();
                using var context = SessionDatabase.Open(databasePath);
        }

        private static string GetSessionDatabasePath()
        {
                string documentHome = AppSettings.DocumentHome();
                if (!string.IsNullOrWhiteSpace(documentHome))
                        return Path.Combine(documentHome, SessionDatabase.DefaultFileName);

                string appDataDirectory = FileSystem.Current.AppDataDirectory;
                string directory = Path.Combine(appDataDirectory, "Yijing");
                return Path.Combine(directory, SessionDatabase.DefaultFileName);
        }
}
