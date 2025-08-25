using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Layouts;
using Yijing.Views;

namespace Yijing.Pages;

public partial class WebPage : ContentPage
{
	public static WebView webview;

	public WebPage()
	{
		InitializeComponent();

		webview = new WebView();
		webview.Navigating += webview_Navigating;
		//webview.Navigated += webview_Navigated;

		Content = webview;
	}
	private void webview_Navigating(object sender, WebNavigatingEventArgs e)
	{
#if WINDOWS
		string s = "https://appdir/Hexagram";
#else
		string s = "file:///android_asset/Hexagram";
#endif
		//if (e.NavigationEvent == WebNavigationEvent.Back)
		if (e.Url.StartsWith(s))
		{
			s = e.Url.Substring(s.Length, e.Url.Length - s.Length);
			if (DiagramView.IsExploreMode() && !string.IsNullOrEmpty(s))
				DiagramView.SetHexagramValue(int.Parse(s));
			e.Cancel = true;
		}
	}

	private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (e.Result == WebNavigationResult.Success)
        {
            //BackButton.IsEnabled = webview.CanGoBack;
            //ForwardButton.IsEnabled = webview.CanGoForward;
        }
        else
        {
            //await DisplayAlert("Navigation failed", e.Result.ToString(), "OK");
        }
    }

}
