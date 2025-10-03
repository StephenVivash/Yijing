
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

		_yd = new YijingDatabase(Path.Combine(AppSettings.DocumentHome(), "Yijing.db"));
		_yd.Initialse();

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
		return builder.Build();
	}

}
