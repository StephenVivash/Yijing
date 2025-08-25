using System.Collections.ObjectModel;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Yijing.Models;

public class EegSeries
{

	public static float m_fThinStoke = 1.0f;
	public static float m_fMediumStoke = 1.5f;
	public static float m_fThickStoke = 2.0f;
	public EegSeries()
	{
	}

	public ISeries[] Series { get; set; }
		= new ISeries[]
		{
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Delta BL", // BetaL
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Delta FL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Delta BC",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Delta FR",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Delta BR",
			},


			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Theta BL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Theta FL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Theta BC",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Theta FR",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Theta BR",
			},


			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Alpha BL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Alpha FL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Alpha BC",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Alpha FR",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Alpha BR",
			},


			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Beta BL", // BetaH
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Beta FL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Beta BC",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Beta FR",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Beta BR",
			},


			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Gamma BL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Gamma FL",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Gamma BC",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Gamma FR",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Orange) { StrokeThickness = m_fThinStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Gamma BR",
			},


			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = m_fMediumStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="Zero",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = m_fMediumStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="OnTrigger",
			},
			new LineSeries<float>
			{
				Values = new ObservableCollection<float> { 0.0f },
				Stroke = new SolidColorPaint(SKColors.Aqua) { StrokeThickness = m_fMediumStoke },
				Fill = null,
				GeometryFill = null,
				GeometryStroke = null,
				IsHoverable = false,
				IsVisible = true,
				LineSmoothness = 1,
				Name="OffTrigger",
			},
		};
/*
	public Axis[] XAxes { get; set; }
		= new Axis[]
		{
			new Axis
			{
				Name = "X Axis",
				IsVisible = false,
				Labels = null,
				MaxLimit = 7,
				MinLimit = 0,
			},
		};

	public Axis[] YAxes { get; set; }
		= new Axis[]
		{
			new Axis
			{
				Name = "Y Axis",
				IsVisible = false,
				Labels = null,
				MaxLimit = 7,
				MinLimit = 0,
			},
		};
*/
}

