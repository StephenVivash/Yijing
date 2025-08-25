using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;

using Yijing.Models;

namespace Yijing.Pages;

public partial class EegPage : ContentPage
{

	private static EegPage _this;

	public static Editor SessionLog() { return _this.edtSessionLog; }
	public static CartesianChart CartesianChart() { return _this.chaEeg; }

	public EegPage()
	{
		_this = this;

		InitializeComponent();

		EegSeries eegChart = new();
		var xAxis = new Axis
		{
			Labels = new[] { "" }
		};

		chaEeg.Series = eegChart.Series;
		chaEeg.XAxes = new List<Axis> { xAxis };
		chaEeg.TooltipPosition = TooltipPosition.Hidden;
		chaEeg.ZoomMode = ZoomAndPanMode.None;
		chaEeg.LegendPosition = LegendPosition.Hidden;
		chaEeg.AnimationsSpeed = new TimeSpan();
		chaEeg.AutoUpdateEnabled = true;

	}

	private void Page_Loaded(object sender, EventArgs e)
	{
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		//diagram.WidthRequest = 600;
	}

}
