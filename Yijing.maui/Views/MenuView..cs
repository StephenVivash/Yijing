
using CommunityToolkit.Maui.Core;
using Yijing.Controls;
using Yijing.Services;

namespace Yijing.Views;

public partial class MenuView : ContentView
{
	private static Color _bgColor = App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White;
	private ePages _ePage = ePages.eNone;
	private StackLayout _slMenu;

	private ButtonEx _btnSession;
	private ButtonEx _btnDiagram;
	private ButtonEx _btnEeg;
	private ButtonEx _btnMeditation;


#if WINDOWS || MACCATALYST
	public const DockPosition VerticalDockPosition = DockPosition.Left;
#elif ANDROID || IOS
	public const DockPosition VerticalDockPosition = DockPosition.Right;
#endif

	public MenuView()
	{
		BindingContext = this;
		BackgroundColor = _bgColor;
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

		double w = width - 10;

		w = width - 40;

		w /= 2;
		w -= 10;

		w /= 2;

		base.OnSizeAllocated(width, height);
	}

	public void Create(ePages ePage, StackOrientation orientation)
	{
		var o = orientation == StackOrientation.Vertical ? "V" : "H";
		var b = new RegisterInViewDirectoryBehavior() { Key = $"{ePage}{o}Menu" };
		Behaviors.Add(b);

		_ePage = ePage;

		Border border = new();

		_slMenu = new StackLayout()
		{
			Orientation = orientation,
			BackgroundColor = _bgColor,
			Margin = new Thickness(0, 0, 0, 0),
			Spacing = 0,
		};
		_btnSession = new ButtonEx()
		{
			Icon = FluentIcons.SessionPage,

		};
		_btnDiagram = new ButtonEx()
		{
			Icon = FluentIcons.DiagramPage,
		};
		_btnEeg = new ButtonEx()
		{
			Icon = FluentIcons.EegPage,
		};
		_btnMeditation = new ButtonEx()
		{
			Icon = FluentIcons.MeditationPage,
		};

		if (orientation == StackOrientation.Vertical)
#if WINDOWS || MACCATALYST
			_slMenu.WidthRequest = 50;
#elif ANDROID || IOS
			_slMenu.WidthRequest = 55;
#endif
		else
			_slMenu.HeightRequest = 45;

		border.Content = _slMenu;

		_slMenu.Children.Add(_btnSession);
		_slMenu.Children.Add(_btnDiagram);
		_slMenu.Children.Add(_btnEeg);
		_slMenu.Children.Add(_btnMeditation);

		_btnSession.Clicked += btnSession_Clicked;
		_btnDiagram.Clicked += btnDiagram_Clicked;
		_btnEeg.Clicked += btnEeg_Clicked;
		_btnMeditation.Clicked += btnMeditation_Clicked;

		Content = border;
	}

	private async void btnSession_Clicked(object sender, EventArgs e)
	{
		if (_ePage != ePages.eSession)
			await Shell.Current.GoToAsync("//Session/SessionRoot", true);
	}

	private async void btnDiagram_Clicked(object sender, EventArgs e)
	{
		//await Shell.Current.Navigation.PushAsync(DiagramPage);
		if (_ePage != ePages.eDiagram)
			await Shell.Current.GoToAsync("///Diagram/DiagramRoot", true);
	}

	private async void btnEeg_Clicked(object sender, EventArgs e)
	{
		if (_ePage != ePages.eEeg)
			await Shell.Current.GoToAsync("//Eeg/EegRoot", true);
	}

	private async void btnMeditation_Clicked(object sender, EventArgs e)
	{
		if (_ePage != ePages.eMeditation)
			await Shell.Current.GoToAsync("//Meditation/MeditationRoot", true);
	}

	public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor),
		typeof(Color), typeof(MenuView), App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White);

	public Color CardColor
	{
		get => (Color)GetValue(CardColorProperty);
		set => SetValue(CardColorProperty, value);
	}
}
