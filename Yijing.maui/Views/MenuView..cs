
using Yijing.Services;

namespace Yijing.Views;

public partial class MenuView : ContentView
{

	private ePages _ePage = ePages.eNone;
	private Color _bgColor;
	private StackLayout _slMenu;

	private Button _btnSession;
	private Button _btnDiagram;
	private Button _btnEeg;
	private Button _btnMeditation;

	public MenuView()
	{
		var b = new RegisterInViewDirectoryBehavior() { Key = "Menu" };
		Behaviors.Add(b);
		BindingContext = this;

		_bgColor = App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White;
		BackgroundColor = _bgColor;
	}

	//protected void Page_Loaded(object sender, EventArgs e)
	//{
	//}

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
		_ePage = ePage;

		_slMenu = new StackLayout()
		{
			Orientation = orientation,
		};
		_btnSession = new Button()
		{
			ImageSource = "iconlistdetail.png",
			BackgroundColor = _bgColor,
		};
		_btnDiagram = new Button()
		{
			ImageSource = "iconlistdetail.png",
			BackgroundColor = _bgColor,
		};
		_btnEeg = new Button()
		{
			ImageSource = "iconlistdetail.png",
			BackgroundColor = _bgColor,
		};
		_btnMeditation = new Button()
		{
			ImageSource = "iconlistdetail.png",
			BackgroundColor = _bgColor,
		};

		if (orientation == StackOrientation.Vertical)
			_slMenu.WidthRequest = 50;
		else
			_slMenu.HeightRequest = 50;

		_slMenu.Children.Add(_btnSession);
		_slMenu.Children.Add(_btnDiagram);
		_slMenu.Children.Add(_btnEeg);
		_slMenu.Children.Add(_btnMeditation);

		_btnSession.Clicked += btnSession_Clicked;
		_btnDiagram.Clicked += btnDiagram_Clicked;
		_btnEeg.Clicked += btnEeg_Clicked;
		_btnMeditation.Clicked += btnMeditation_Clicked;

		Content = _slMenu;
	}

	private async void btnSession_Clicked(object sender, EventArgs e)
	{
		if (_ePage != ePages.eSession)
			await Shell.Current.GoToAsync("//Session/SessionRoot", true);
	}

	private async void btnDiagram_Clicked(object sender, EventArgs e)
	{
		if (_ePage != ePages.eDiagram)
			await Shell.Current.GoToAsync("//Diagram/DiagramRoot", true);
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

	public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle),
		typeof(string), typeof(DiagramView), string.Empty);

	public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor),
		typeof(Color), typeof(DiagramView), App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White);

	public string CardTitle
	{
		get => (string)GetValue(CardTitleProperty);
		set => SetValue(CardTitleProperty, value);
	}

	public Color CardColor
	{
		get => (Color)GetValue(CardColorProperty);
		set => SetValue(CardColorProperty, value);
	}
}
