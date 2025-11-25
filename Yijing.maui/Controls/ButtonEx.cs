
namespace Yijing.Controls;

class ButtonEx : Button
{
	private static Color _bgColor = App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White;
	private static Color _fgColor = App.Current.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;

	public ButtonEx()
	{
		BackgroundColor = _bgColor;
		TextColor = _fgColor;
		BorderColor = _bgColor;
		HeightRequest = 40;
	}

	public static readonly BindableProperty IsLoadingProperty =
		BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(ButtonEx), false, propertyChanged: OnIsLoadingChanged);

	public static readonly BindableProperty BorderProperty =
		BindableProperty.Create(nameof(Border), typeof(bool), typeof(ButtonEx), false, propertyChanged: OnBorderChanged);

	public bool IsLoading
	{
		get => (bool)GetValue(IsLoadingProperty);
		set => SetValue(IsLoadingProperty, value);
	}

	private static void OnIsLoadingChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var button = (ButtonEx)bindable;
		bool isLoading = (bool)newValue;
		if (isLoading)
		{
			//button.Text = "Loading...";
			button.IsEnabled = false;
		}
		else
		{
			//button.Text = "Submit";
			button.IsEnabled = true;
		}
	}

	public bool Border
	{
		get => (bool)GetValue(BorderProperty);
		set => SetValue(BorderProperty, value);
	}

	private static void OnBorderChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var button = (ButtonEx)bindable;
		bool border = (bool)newValue;
		if (border)
		{
			button.BorderColor = _fgColor;
		}
		else
		{
			button.BorderColor = _bgColor;
		}
	}
}

