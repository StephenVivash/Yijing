
// dotnet nuget why Yijing.maui.csproj Microsoft.SemanticKernel

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

using Yijing.Services;
using Yijing.Views;

namespace Yijing;

public class YijingKernel
{

	//[KernelFunction]
	//public void SelectHexagram(int number)
	//{
	//	DiagramView.SetHexagramValue(number);
	//}

	//[KernelFunction("auto_cast_hexagram")]
	public static void auto_cast_hexagram()
	{
		DiagramView.AutoCastHexagram();
	}

	//[KernelFunction("set_hexagram")]
	public static void set_hexagram(int sequence)
	{
		DiagramView.SetHexagram(sequence);
	}

	//[KernelFunction("get_hexagram")]
	public static int get_hexagram()
	{
		return DiagramView.GetHexagram();
	}

	//[KernelFunction("first_hexagram")]
	public static void first_hexagram()
	{
		DiagramView.SetFirst();
	}

	//[KernelFunction("previous_hexagram")]
	public void previous_hexagram()
	{
		DiagramView.SetPrevious();
	}

	//[KernelFunction("next_hexagram")]
	public void next_hexagram()
	{
		DiagramView.SetNext();
	}

	//[KernelFunction("last_hexagram")]
	public static void last_hexagram()
	{
		DiagramView.SetLast();
	}

	//[KernelFunction("move_hexagram")]
	public static void move_hexagram()
	{
		DiagramView.SetMove();
	}

	//[KernelFunction("last_cast_hexagram")]
	public static void last_cast_hexagram()
	{
		DiagramView.SetHome();
	}

	//[KernelFunction("inverse_hexagram")]
	public static void inverse_hexagram()
	{
		DiagramView.SetInverse();
	}

	//[KernelFunction("opposite_hexagram")]
	public static void opposite_hexagram()
	{
		DiagramView.SetOpposite();
	}

	//[KernelFunction("transverse_hexagram")]
	public static void transverse_hexagram()
	{
		DiagramView.SetTransverse();
	}

	//[KernelFunction("nuclear_hexagram")]
	public static void nuclear_hexagram()
	{
		DiagramView.SetNuclear();
	}
}

public static class MauiProgram
{
	//AppSettings _appSettings = new AppSettings();

	public static MauiApp CreateMauiApp()
	{
		AppSettings.SetDocumentHome();
		AppSettings.EegCreate();
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
		return builder.Build();
	}

}
