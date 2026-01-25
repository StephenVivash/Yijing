
// dotnet nuget why Yijing.maui.csproj Microsoft.SemanticKernel

using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

using Yijing.Services;
using YijingData;

namespace Yijing;

public static class MauiProgram
{
	static YijingDatabase _yd;

	public static MauiApp CreateMauiApp()
	{
		AppSettings.SetDocumentHome();
		AppPreferences.Load();
		AiPreferences.Load();
		AudioPlayer.Load();

		_yd = new YijingDatabase(Path.Combine(AppSettings.DocumentHome(), "Yijing.db"));
		_yd.Initialse();

		MauiAppBuilder builder = MauiApp.CreateBuilder();
		MauiAppBuilder mauiBuilder = builder.UseMauiApp<App>();
#pragma warning disable CA1416 // Validate platform compatibility
		mauiBuilder
			.UseLiveCharts()
			.UseSkiaSharp()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("SegoeIcons.ttf", "Segoe Fluent Icons");
			});
#pragma warning restore CA1416 // Validate platform compatibility

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if ANDROID || IOS
		//builder.Services.AddTransient<Services.SampleDataService>();
		//builder.Services.AddSingleton<Pages.WebPage>();
#endif

		return builder.Build();
	}
}
