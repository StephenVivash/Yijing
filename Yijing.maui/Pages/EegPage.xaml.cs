using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;

using System.Collections.ObjectModel;

using Yijing.Models;
using Yijing.Services;

namespace Yijing.Pages;

public partial class EegPage : ContentPage
{
	private ObservableCollection<string> _timeAxisLabels = new();// { "0" };

	public Editor SessionLog() => edtSessionLog;
	public CartesianChart CartesianChart() => chaEeg;
	public ObservableCollection<string> TimeAxisLabels() => _timeAxisLabels;

	public EegPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();

		EegSeries eegChart = new();
		var xAxis = new Axis
		{
			Labels = _timeAxisLabels
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
