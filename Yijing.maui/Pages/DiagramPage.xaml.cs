
using Yijing.Services;
using Yijing.Views;

namespace Yijing.Pages;

public partial class DiagramPage : ContentPage
{
	public WebView WebView() => webview;

	public DiagramPage()
	{
		Behaviors.Add(new RegisterInViewDirectoryBehavior());
		InitializeComponent();

		horMenu.Create(ePages.eDiagram, StackOrientation.Horizontal);
		verMenu.Create(ePages.eDiagram, StackOrientation.Vertical);
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
			diagramView.WidthRequest = 200;
			diagramView.ButtonPadding(new Thickness(0, 5));
		}
		else
		{
			horMenu.IsVisible = true;
			verMenu.IsVisible = false;
			diagramView.WidthRequest = width;
			diagramView.ButtonPadding(new Thickness(5));
		}
#else
		horMenu.IsVisible = false;
		verMenu.IsVisible = true;
#endif

		base.OnSizeAllocated(width, height);
	}

	private void webview_Navigating(object sender, WebNavigatingEventArgs e)
	{
#if WINDOWS
		string s = "https://appdir/Hexagram";
#elif ANDROID
		string s = "file:///android_asset/Hexagram";
#elif MACCATALYST
		string s = "file:///xxx/Hexagram";
#elif IOS
		string s = "file:///xxx/Hexagram";
#endif

		//if (e.NavigationEvent == WebNavigationEvent.Back)
		if (e.Url.StartsWith(s))
		{
			s = e.Url.Substring(s.Length, e.Url.Length - s.Length);
			bool b = false;
			UI.Call<DiagramView>(v => b = v.IsExploreMode());
			if (b && !string.IsNullOrEmpty(s))
				UI.Call<DiagramView>(v => v.SetHexagramValue(int.Parse(s)));
			e.Cancel = true;
		}
	}

	/*
	private void webview_Navigated(object sender, WebNavigatedEventArgs e)
	{
		if (e.Result == WebNavigationResult.Success)
		{
			//BackButton.IsEnabled = webview.CanGoBack;
		}
		else
		{
			//await DisplayAlert("Navigation failed", e.Result.ToString(), "OK");
		}
	}

	private void Back_Clicked(object sender, EventArgs e)
	{
		webview.GoBack();
	}

	private void Forward_Clicked(object sender, EventArgs e)
	{
		webview.GoForward();
	}

	private void Refresh_Clicked(object sender, EventArgs e)
	{
		webview.Reload();
	}
	*/
}
