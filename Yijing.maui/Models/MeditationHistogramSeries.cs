using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Yijing.Models;

public class MeditationHistogramSeries
{
        private readonly ObservableCollection<double> _values = new();

        public MeditationHistogramSeries()
        {
                Histogram = new ColumnSeries<double>
                {
                        Values = _values,
                        Stroke = null,
                        Fill = new SolidColorPaint(SKColors.SteelBlue),
                        Name = "Duration (minutes)",
                        MaxBarWidth = 45
                };

                Series = new ISeries[] { Histogram };
        }

        public ColumnSeries<double> Histogram { get; }

        public ISeries[] Series { get; }

        public void SetValues(IEnumerable<double> values)
        {
                _values.Clear();
                foreach (var value in values)
                        _values.Add(value);
        }
}
