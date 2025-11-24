using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Yijing;

public partial class App : Application
{

	public App()
	{
		InitializeComponent();

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
	}

	protected override Window CreateWindow(IActivationState activationState)
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
