using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Yijing;

#nullable enable

public partial class App : Application
{

	public static App? GetApp()
	{
		Window[] w1 = Application.Current.Windows.ToArray();
		foreach (var item in w1)
		{
			var y = item.Parent;
			if (y is App) return (App)y;
		}
		//Page p = Application.Current.MainPage;
		//var x = Application.Current.Windows[0].Parent;
		return null; // (App)x;
	}

	public App()
	{
		InitializeComponent();

		//MainPage = new AppShell();

		LiveCharts.Configure(config => config
			.AddSkiaSharp()
			.AddDefaultMappers()
			.AddDarkTheme()
			//.AddLightTheme()

			//.HasMap<City>((city, point) =>
			//	{
			//	point.PrimaryValue = city.Population;
			//	point.SecondaryValue = point.Context.Index; // Entity.EntityIndex;
			//	})
			// .HasMap<Foo>( .... )
			// .HasMap<Bar>( .... )
			);

		//MainPage.Navigation.PushAsync(MainPage);
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		//Window window = base.CreateWindow(activationState);
		Window window = new Window(new AppShell());
		window.Created += (s, e) =>
		{
			//window.X = 100;
			//window.Y = 100;
			//window.Width = 1400;
			//window.Height = 800;
		};
		return window;
	}
}
