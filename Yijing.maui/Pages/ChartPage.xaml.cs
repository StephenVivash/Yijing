using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using Yijing.Models;

namespace Yijing.Pages;

public partial class ChartPage : ContentPage
{
	public static CartesianChart chaEeg;

	public ChartPage()
	{
		InitializeComponent();

                EegSeries eegSeries = new();
                var xAxis = new Axis
                {
                        Name = "Time",
                        Labels = new[] { "" }
                };

		chaEeg = new CartesianChart
		{
			Series = eegSeries.Series,
			XAxes = new List<Axis> { xAxis },
			TooltipPosition = TooltipPosition.Hidden,
			ZoomMode = ZoomAndPanMode.None,
			LegendPosition = LegendPosition.Hidden,
			AnimationsSpeed= new TimeSpan(),
			AutoUpdateEnabled = true,
			 
		};

		Content = chaEeg;
	}
}
