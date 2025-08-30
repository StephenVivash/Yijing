
using Yijing.Services;
using Yijing.Views;

namespace Yijing.Pages;

public partial class DiagramPage : ContentPage
{

	private static DiagramPage _this;

	public static Editor SessionLog() { return _this.edtSessionLog; }
	public static WebView WebView() { return _this.webview; }

	public DiagramPage()
	{
		_this = this;
		InitializeComponent();

		picAiChatService.SelectedIndex = AppPreferences.AiChatService;
		picAiMode.SelectedIndex = 0;
		chbIncludeCast.IsChecked = true;

	}

	private void Page_Loaded(object sender, EventArgs e)
	{
		//var button = new Button { Text = "New Bottom", HeightRequest = 200 };
		//DockLayout.SetDockPosition(button, DockPosition.Bottom);
	}

	protected override void OnSizeAllocated(double width, double height)
	{
#if ANDROID || IOS
		//diagram.WidthRequest = 600;
#endif
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
			if (DiagramView.IsExploreMode() && !string.IsNullOrEmpty(s))
				DiagramView.SetHexagramValue(int.Parse(s));
			e.Cancel = true;
		}
	}

	protected void picAiChatService_SelectedIndexChanged(object sender, EventArgs e)
	{
		//picAiChatService.SelectedIndex = AppPreferences.AiChatService;
		AppPreferences.AiChatService = picAiChatService.SelectedIndex;
	}

	protected void picAiMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (picAiMode.SelectedIndex == 1)
		{
			//MauiProgram.BuildKernelMemory();
			//DiagramView.UpdateSessionLog(" ***** Started Kernel Memory", true, true);
		}
	}

	protected void btnAsk_Clicked(object sender, EventArgs e)
	{
		diagram.AiChat(picAiMode.SelectedIndex == 1, chbIncludeCast.IsChecked);
		chbIncludeCast.IsChecked = false;
	}

	private void chbIncludeCast_CheckedChanged(object sender, EventArgs e)
	{
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
