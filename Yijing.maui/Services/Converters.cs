using System.Globalization;
using YijingData;

namespace Yijing.Services;

public sealed class YijingCastConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null) return string.Empty;

		if (value is string cast)
			return string.IsNullOrEmpty(cast) ? string.Empty : "Y";

		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException(); // usually one-way
}

public sealed class MeditationConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null) return string.Empty;

		if (value is bool b)
			return b ? "M" : "";

		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException(); // usually one-way
}

public sealed class EegDeviceConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null) return string.Empty;

		if (value is eEegDevice eeg)
			return eeg == eEegDevice.eNone ? "" : "E";

		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException(); // usually one-way
}

public sealed class AnalysisConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null) return string.Empty;

		if (value is bool b)
			return b ? "A" : "";

		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException(); // usually one-way
}

