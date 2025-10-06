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

public sealed class BoolToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null) return string.Empty;

		if (value is bool b)
			return b ? "Y" : string.Empty;

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
			if (eeg == eEegDevice.eNone) return string.Empty;
			else if (eeg == eEegDevice.eMuse) return "M";
			else if (eeg == eEegDevice.eEmotiv) return "E";

		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException(); // usually one-way
}
