
// dotnet nuget why Yijing.maui.csproj Microsoft.SemanticKernel

using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

using Yijing.Services;
using YijingData;

namespace Yijing;

public static class MauiProgram
{
	static YijingDatabase _yd;

	public static MauiApp CreateMauiApp()
	{
		AiPreferences.PreferenceStore = new MauiAiPreferenceStore();
		AppSettings.Load();
		AppPreferences.Load();
		AiPreferences.Load();
		AppPreferences.AiChatService = AiPreferences.NormalizeServiceName(AppPreferences.AiChatService);
		AppPreferences.AiEegService = AiPreferences.NormalizeServiceName(AppPreferences.AiEegService);
		AudioPlayer.Load();

		_yd = new YijingDatabase(Path.Combine(AppSettings.DocumentHome(), "Yijing.db"));
		_yd.Initialse();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.UseLiveCharts()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("SegoeIcons.ttf", "Segoe Fluent Icons");
			});

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
