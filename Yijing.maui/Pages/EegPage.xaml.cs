using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;

using System.Collections.ObjectModel;

using Yijing.Models;
using Yijing.Services;
using Yijing.Views;

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

		if (App.Current!.RequestedTheme == AppTheme.Dark)
		{
			edtSessionLog.BackgroundColor = Colors.Black;
			chaEeg.BackgroundColor = Colors.Black;
		}

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

		horMenu.Create(ePages.eEeg, StackOrientation.Horizontal);
		verMenu.Create(ePages.eEeg, StackOrientation.Vertical);
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

#if ANDROID || IOS
		if (width > height)
		{
			horMenu.IsVisible = false;
			verMenu.IsVisible = true;
			eegView.WidthRequest = 200;
		}
		else
		{
			horMenu.IsVisible = true;
			verMenu.IsVisible = false;
			eegView.WidthRequest = width - 10;
		}
#else
		horMenu.IsVisible = false;
		verMenu.IsVisible = true;
#endif

		base.OnSizeAllocated(width, height);
	}

	public void ShowSessionLog(bool show)
	{
		edtSessionLog.IsVisible = show;
	}
}
