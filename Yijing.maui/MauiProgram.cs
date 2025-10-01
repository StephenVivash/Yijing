
// dotnet nuget why Yijing.maui.csproj Microsoft.SemanticKernel

using System.IO;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Maui.Storage;
using SessionDb;

using Yijing.Services;
using Yijing.Views;

namespace Yijing;

public static class MauiProgram
{

        public static IServiceProvider? Services
        {
                get;
                private set;
        }

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
                string databasePath = Path.Combine(FileSystem.AppDataDirectory, "Session.db");
                builder.Services.AddSessionDatabase(databasePath);
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
                SessionDatabaseInitializer.Initialize(app.Services);
                Services = app.Services;
                return app;
        }

}
