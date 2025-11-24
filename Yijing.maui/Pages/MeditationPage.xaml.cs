using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;

using Microsoft.EntityFrameworkCore;

using Yijing.Models;
using Yijing.Services;
using Yijing.Views;
using YijingData;

namespace Yijing.Pages;

public partial class MeditationPage : ContentPage
{
	private readonly MeditationSeries _histogram = new();
	private readonly ObservableCollection<string> _xAxisLabels = new();
	private readonly Axis _xAxis;
	private readonly Axis _yAxis;
	private List<Meditation> _meditations = new();
	private AggregationRange _range = AggregationRange.Week;
	private DateTime _rangeStart;
	private bool _dataLoaded;

	public MeditationPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();

		if (App.Current!.RequestedTheme == AppTheme.Dark)
		{
			chaMeditation.BackgroundColor = Colors.Black;
			hslSelection.BackgroundColor = Colors.Black;
		}

		BindingContext = _histogram;

		_xAxis = new Axis
		{
			Labels = _xAxisLabels,
			LabelsRotation = 0
		};

		_yAxis = new Axis
		{
			Labeler = value => value.ToString("0"),
			MinLimit = 0
		};

		chaMeditation.Series = _histogram.Series;
		chaMeditation.XAxes = new List<Axis> { _xAxis };
		chaMeditation.YAxes = new List<Axis> { _yAxis };
		chaMeditation.TooltipPosition = TooltipPosition.Hidden;
		chaMeditation.ZoomMode = ZoomAndPanMode.None;
		chaMeditation.LegendPosition = LegendPosition.Hidden;
		chaMeditation.AnimationsSpeed = TimeSpan.Zero;
		chaMeditation.AutoUpdateEnabled = true;

		picRange.SelectedIndex = 0;
		_rangeStart = StartOfWeek(DateTime.Today);

		horMenu.Create(ePages.eMeditation, StackOrientation.Horizontal);
		verMenu.Create(ePages.eMeditation, StackOrientation.Vertical);
	}

	private void Page_Loaded(object sender, EventArgs e)
	{
		RefreshChart();
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
			meditationView.WidthRequest = 200;
		}
		else
		{
			horMenu.IsVisible = true;
			verMenu.IsVisible = false;
			meditationView.WidthRequest = width;
		}
#else
		horMenu.IsVisible = false;
		verMenu.IsVisible = true;
#endif

		base.OnSizeAllocated(width, height);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (!_dataLoaded)
		{
			LoadMeditations();
			_dataLoaded = true;
		}

		RefreshChart();
	}

	private void LoadMeditations()
	{
		try
		{
			using var context = new YijingDbContext();
			_meditations = context.Meditations
					.AsNoTracking()
					.OrderBy(m => m.Start)
					.ToList();
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Failed to load meditations: {ex.Message}");
			_meditations = new List<Meditation>();
		}
	}

	private void RefreshChart()
	{
		if (picRange.SelectedIndex is int index && index >= 0)
			_range = (AggregationRange)index;

		AdjustRangeStart();

		var (rangeStart, rangeEnd) = GetRangeBounds();
		IEnumerable<double> values = BuildHistogram(rangeStart, rangeEnd);

		_histogram.SetValues(values);
		UpdateAxisLabels(rangeStart, rangeEnd);
		lblRange.Text = BuildRangeDescription(rangeStart, rangeEnd);
	}

	private void AdjustRangeStart()
	{
		DateTime today = DateTime.Today;
		switch (_range)
		{
			case AggregationRange.Week:
				_rangeStart = _rangeStart == default ? StartOfWeek(today) : StartOfWeek(_rangeStart);
				break;
			case AggregationRange.Month:
				_rangeStart = _rangeStart == default ? new DateTime(today.Year, today.Month, 1) : new DateTime(_rangeStart.Year, _rangeStart.Month, 1);
				break;
			case AggregationRange.Year:
				_rangeStart = _rangeStart == default ? new DateTime(today.Year, 1, 1) : new DateTime(_rangeStart.Year, 1, 1);
				break;
		}
	}

	private (DateTime Start, DateTime End) GetRangeBounds()
	{
		return _range switch
		{
			AggregationRange.Week => (_rangeStart, _rangeStart.AddDays(7)),
			AggregationRange.Month => (_rangeStart, _rangeStart.AddMonths(1)),
			AggregationRange.Year => (_rangeStart, _rangeStart.AddYears(1)),
			_ => (_rangeStart, _rangeStart.AddDays(7))
		};
	}

	private IEnumerable<double> BuildHistogram(DateTime start, DateTime end)
	{
		return _range switch
		{
			AggregationRange.Week => BuildWeeklyHistogram(start, end),
			AggregationRange.Month => BuildMonthlyHistogram(start, end),
			AggregationRange.Year => BuildYearlyHistogram(start, end),
			_ => Array.Empty<double>()
		};
	}

	private IEnumerable<double> BuildWeeklyHistogram(DateTime start, DateTime end)
	{
		double[] values = new double[7];
		foreach (var meditation in FilterMeditations(start, end))
		{
			int index = (int)(meditation.Start.Date - start.Date).TotalDays;
			if (index >= 0 && index < values.Length)
				values[index] += meditation.Duration;
		}
		return values;
	}

	private IEnumerable<double> BuildMonthlyHistogram(DateTime start, DateTime end)
	{
		int days = DateTime.DaysInMonth(start.Year, start.Month);
		double[] values = new double[days];
		foreach (var meditation in FilterMeditations(start, end))
		{
			int index = meditation.Start.Day - 1;
			if (index >= 0 && index < values.Length)
				values[index] += meditation.Duration;
		}
		return values;
	}

	private IEnumerable<double> BuildYearlyHistogram(DateTime start, DateTime end)
	{
		double[] values = new double[12];
		foreach (var meditation in FilterMeditations(start, end))
		{
			int index = meditation.Start.Month - 1;
			if (index >= 0 && index < values.Length)
				values[index] += meditation.Duration / 60;
		}
		return values;
	}

	private IEnumerable<Meditation> FilterMeditations(DateTime start, DateTime end)
	{
		return _meditations.Where(m => m.Start >= start && m.Start < end);
	}

	private void UpdateAxisLabels(DateTime start, DateTime end)
	{
		_xAxisLabels.Clear();
		int count = _range switch
		{
			AggregationRange.Week => 7,
			AggregationRange.Month => DateTime.DaysInMonth(start.Year, start.Month),
			AggregationRange.Year => 12,
			_ => 0
		};

		for (int i = 1; i <= count; i++)
			_xAxisLabels.Add(i.ToString());
	}

	private string BuildRangeDescription(DateTime start, DateTime end)
	{
		return _range switch
		{
			AggregationRange.Week => $"{start:MMM d, yyyy} - {end.AddDays(-1):MMM d, yyyy}",
			AggregationRange.Month => start.ToString("MMMM yyyy"),
			AggregationRange.Year => start.Year.ToString(),
			_ => string.Empty
		};
	}

	private void picRange_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (picRange.SelectedIndex == -1)
			return;

		_range = (AggregationRange)picRange.SelectedIndex;
		switch (_range)
		{
			case AggregationRange.Week:
				_rangeStart = StartOfWeek(DateTime.Today);
				break;
			case AggregationRange.Month:
				_rangeStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				break;
			case AggregationRange.Year:
				_rangeStart = new DateTime(DateTime.Today.Year, 1, 1);
				break;
		}

		RefreshChart();
	}

	private void btnPrev_Clicked(object sender, EventArgs e)
	{
		MoveRange(-1);
	}

	private void btnNext_Clicked(object sender, EventArgs e)
	{
		MoveRange(1);
	}

	private void MoveRange(int offset)
	{
		switch (_range)
		{
			case AggregationRange.Week:
				_rangeStart = _rangeStart.AddDays(7 * offset);
				break;
			case AggregationRange.Month:
				_rangeStart = _rangeStart.AddMonths(offset);
				break;
			case AggregationRange.Year:
				_rangeStart = _rangeStart.AddYears(offset);
				break;
		}

		RefreshChart();
	}

	private static DateTime StartOfWeek(DateTime date)
	{
		int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
		return date.AddDays(-1 * diff).Date;
	}

	private void MeditationView_MeditationCompleted(object sender, MeditationSessionCompletedEventArgs e)
	{
		_meditations.Add(e.Meditation);
		_meditations = _meditations.OrderBy(m => m.Start).ToList();
		RefreshChart();
	}

	private enum AggregationRange
	{
		Week = 0,
		Month = 1,
		Year = 2
	}
}
