using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;

using Yijing.Models;
using Yijing.Services;

namespace Yijing.Pages;

public partial class EegPage : ContentPage
{

	private static EegPage _this;

	public static Editor SessionLog() { return _this.edtSessionLog; }
	public static CartesianChart CartesianChart() { return _this.chaEeg; }

        private const double SecondsPerSample = 0.15;

        public EegPage()
        {
                _this = this;

                InitializeComponent();

                EegSeries eegChart = new();
                int seriesMax = (AppPreferences.ChartTime + 1) * 1000;
                var xAxis = new Axis
                {
                        Name = "Time",
                        MinLimit = 0,
                        MaxLimit = seriesMax,
                        Labeler = value => TimeSpan.FromSeconds(value * SecondsPerSample).ToString(@"mm\\:ss")
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
